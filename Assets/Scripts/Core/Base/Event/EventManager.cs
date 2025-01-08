using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Base.Event
{
    public class EventManager : MonoBehaviour
    {
        private static Dictionary<string, List<Action<object[]>>> eventDictionary = new Dictionary<string, List<Action<object[]>>>();

        public static void StartListening(string eventName, Action<object[]> listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = new List<Action<object[]>>();
            }
            eventDictionary[eventName].Add(listener);
        }

        public static void StopListening(string eventName, Action<object[]> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName].Remove(listener);
            }
        }

        public static void TriggerEvent(string eventName, params object[] parameters)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                foreach (var listener in eventDictionary[eventName])
                {
                    listener.Invoke(parameters);
                }
            }
        }

        public static void ClearEventListeners()
        {
            eventDictionary.Clear();
        }
    }
}
