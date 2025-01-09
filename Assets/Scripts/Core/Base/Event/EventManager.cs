using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Base.Event
{
    public static class EventManager
    {
        private static Dictionary<Type, List<Delegate>> eventHandlers = new Dictionary<Type, List<Delegate>>();
        private static Dictionary<string, List<Action<object>>> namedEventHandlers = new Dictionary<string, List<Action<object>>>();
        private static Queue<EventData> eventQueue = new Queue<EventData>();
        private static bool isProcessingQueue = false;

        private class EventData
        {
            public Type EventType { get; set; }
            public object Data { get; set; }
            public string EventName { get; set; }
        }

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!eventHandlers.ContainsKey(type))
                eventHandlers[type] = new List<Delegate>();
            eventHandlers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (eventHandlers.ContainsKey(type))
                eventHandlers[type].Remove(handler);
        }

        public static void Subscribe(string eventName, Action<object> handler)
        {
            if (!namedEventHandlers.ContainsKey(eventName))
                namedEventHandlers[eventName] = new List<Action<object>>();
            namedEventHandlers[eventName].Add(handler);
        }

        public static void Unsubscribe(string eventName, Action<object> handler)
        {
            if (namedEventHandlers.ContainsKey(eventName))
                namedEventHandlers[eventName].Remove(handler);
        }

        public static void Publish<T>(T eventData, bool immediate = true)
        {
            if (immediate)
                PublishImmediate(eventData);
            else
                QueueEvent(eventData);
        }

        public static void Publish(string eventName, object eventData = null, bool immediate = true)
        {
            if (immediate)
                PublishNamedImmediate(eventName, eventData);
            else
                QueueNamedEvent(eventName, eventData);
        }

        private static void PublishImmediate<T>(T eventData)
        {
            var type = typeof(T);
            if (!eventHandlers.ContainsKey(type)) return;

            foreach (var handler in eventHandlers[type].ToArray())
            {
                try
                {
                    if (handler is Action<T> action)
                        action(eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error publishing event of type {type}: {e}");
                }
            }
        }

        private static void PublishNamedImmediate(string eventName, object eventData)
        {
            if (!namedEventHandlers.ContainsKey(eventName)) return;

            foreach (var handler in namedEventHandlers[eventName].ToArray())
            {
                try
                {
                    handler(eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error publishing named event {eventName}: {e}");
                }
            }
        }

        private static void QueueEvent<T>(T eventData)
        {
            eventQueue.Enqueue(new EventData { EventType = typeof(T), Data = eventData });
            ProcessEventQueue();
        }

        private static void QueueNamedEvent(string eventName, object eventData)
        {
            eventQueue.Enqueue(new EventData { EventName = eventName, Data = eventData });
            ProcessEventQueue();
        }

        private static void ProcessEventQueue()
        {
            if (isProcessingQueue) return;
            isProcessingQueue = true;

            while (eventQueue.Count > 0)
            {
                var eventData = eventQueue.Dequeue();
                if (eventData.EventName != null)
                    PublishNamedImmediate(eventData.EventName, eventData.Data);
                else if (eventData.EventType != null)
                    PublishImmediate(eventData.Data);
            }

            isProcessingQueue = false;
        }

        public static void ClearAllEvents()
        {
            eventHandlers.Clear();
            namedEventHandlers.Clear();
            eventQueue.Clear();
            isProcessingQueue = false;
        }
    }
}
