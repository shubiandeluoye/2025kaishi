using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Base.Event
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager instance;
        private Dictionary<string, Action<object[]>> eventDictionary;

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("EventManager");
                    instance = go.AddComponent<EventManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                eventDictionary = new Dictionary<string, Action<object[]>>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddListener(string eventName, Action<object[]> listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = listener;
            }
            else
            {
                eventDictionary[eventName] += listener;
            }
        }

        public void RemoveListener(string eventName, Action<object[]> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= listener;
            }
        }

        public void TriggerEvent(string eventName, params object[] parameters)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName]?.Invoke(parameters);
            }
        }
    }
}
