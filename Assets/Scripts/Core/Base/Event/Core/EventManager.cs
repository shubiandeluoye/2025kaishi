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
    }
}

