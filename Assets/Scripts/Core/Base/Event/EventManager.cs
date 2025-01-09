using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Base.Event
{
    /// <summary>
    /// 静态事件管理器
    /// 处理游戏中所有的事件通信
    /// </summary>
    public static class EventManager
    {
        #region 内部结构
        private static Dictionary<string, List<Delegate>> eventHandlers = new Dictionary<string, List<Delegate>>();
        private static Queue<QueuedEventData> eventQueue = new Queue<QueuedEventData>();
        private static bool isProcessingQueue = false;

        private class QueuedEventData
        {
            public string EventName;
            public object Data;
        }
        #endregion

        #region 核心方法
        /// <summary>
        /// 订阅事件 - 泛型版本
        /// </summary>
        public static void Subscribe<T>(string eventName, Action<T> handler)
        {
            if (!eventHandlers.ContainsKey(eventName))
            {
                eventHandlers[eventName] = new List<Delegate>();
            }
            eventHandlers[eventName].Add(handler);
        }

        /// <summary>
        /// 订阅事件 - 非泛型版本
        /// </summary>
        public static void Subscribe(string eventName, Action<object> handler)
        {
            if (!eventHandlers.ContainsKey(eventName))
            {
                eventHandlers[eventName] = new List<Delegate>();
            }
            eventHandlers[eventName].Add(handler);
        }

        /// <summary>
        /// 取消订阅事件 - 泛型版本
        /// </summary>
        public static void Unsubscribe<T>(string eventName, Action<T> handler)
        {
            if (eventHandlers.ContainsKey(eventName))
            {
                eventHandlers[eventName].Remove(handler);
            }
        }

        /// <summary>
        /// 取消订阅事件 - 非泛型版本
        /// </summary>
        public static void Unsubscribe(string eventName, Action<object> handler)
        {
            if (eventHandlers.ContainsKey(eventName))
            {
                eventHandlers[eventName].Remove(handler);
            }
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        public static void Publish<T>(string eventName, T data, bool immediate = true)
        {
            if (immediate)
            {
                PublishImmediate(eventName, data);
            }
            else
            {
                QueueEvent(eventName, data);
            }
        }

        private static void PublishImmediate<T>(string eventName, T data)
        {
            if (!eventHandlers.ContainsKey(eventName)) return;

            foreach (var handler in eventHandlers[eventName].ToArray())
            {
                try
                {
                    if (handler is Action<T> typedHandler)
                    {
                        typedHandler(data);
                    }
                    else if (handler is Action<object> objectHandler)
                    {
                        objectHandler(data);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error publishing event {eventName}: {e}");
                }
            }
        }

        private static void QueueEvent<T>(string eventName, T data)
        {
            eventQueue.Enqueue(new QueuedEventData { EventName = eventName, Data = data });
            ProcessEventQueue();
        }

        private static void ProcessEventQueue()
        {
            if (isProcessingQueue) return;
            isProcessingQueue = true;

            while (eventQueue.Count > 0)
            {
                var eventData = eventQueue.Dequeue();
                PublishImmediate(eventData.EventName, eventData.Data);
            }

            isProcessingQueue = false;
        }

        /// <summary>
        /// 清除所有事件
        /// </summary>
        public static void ClearAllEvents()
        {
            eventHandlers.Clear();
            eventQueue.Clear();
            isProcessingQueue = false;
        }
        #endregion

        #region 事件名称常量
        public static class EventNames
        {
            // 交互事件
            public const string INTERACTION_START = "INTERACTION_START";
            public const string INTERACTION_END = "INTERACTION_END";
            public const string HIGHLIGHT_START = "HIGHLIGHT_START";
            public const string HIGHLIGHT_END = "HIGHLIGHT_END";
            
            // 战斗事件
            public const string BULLET_FIRED = "BULLET_FIRED";
            public const string BULLET_HIT = "BULLET_HIT";
            public const string BULLET_DESTROYED = "BULLET_DESTROYED";
            
            // 游戏状态事件
            public const string GAME_PAUSED = "GAME_PAUSED";
            public const string GAME_RESUMED = "GAME_RESUMED";
            public const string LEVEL_STARTED = "LEVEL_STARTED";
            public const string LEVEL_COMPLETED = "LEVEL_COMPLETED";
        }
        #endregion

        #region 事件数据类型
        public class InteractionEventData
        {
            public GameObject Interactor { get; set; }
            public GameObject Target { get; set; }
            public string InteractionType { get; set; }
        }

        public class BulletEventData
        {
            public Vector3 Position { get; set; }
            public Vector3 Direction { get; set; }
            public float Damage { get; set; }
            public int Level { get; set; }
            public Vector3[] PathPoints { get; set; }
            public float Duration { get; set; }
        }

        public class ScreenEventData
        {
            public float ShakeDuration { get; set; }
            public float ShakeIntensity { get; set; }
            public bool UseLetterbox { get; set; }
            public float LetterboxDuration { get; set; }
            public AnimationCurve TransitionCurve { get; set; }
        }

        public class TimeEventData
        {
            public float SlowdownFactor { get; set; }
            public float Duration { get; set; }
            public bool Smooth { get; set; }
        }
        #endregion
    }
}

