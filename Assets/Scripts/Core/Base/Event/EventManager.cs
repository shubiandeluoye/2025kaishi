using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Base.Event
{
    /// <summary>
    /// Core event system that handles event registration, dispatch, and cleanup
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        #region Singleton
        private static EventManager instance;
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EventManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("EventManager");
                        instance = go.AddComponent<EventManager>();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Event Storage
        private Dictionary<string, List<EventListener>> eventListeners = new Dictionary<string, List<EventListener>>();
        private Dictionary<string, List<Action<EventData>>> eventActions = new Dictionary<string, List<Action<EventData>>>();
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            ClearAllEvents();
        }
        #endregion

        #region Event Registration
        public void AddListener(string eventName, EventListener listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new List<EventListener>();
            }
            
            if (!eventListeners[eventName].Contains(listener))
            {
                eventListeners[eventName].Add(listener);
            }
        }

        public void AddListener(string eventName, Action<EventData> action)
        {
            if (!eventActions.ContainsKey(eventName))
            {
                eventActions[eventName] = new List<Action<EventData>>();
            }
            
            if (!eventActions[eventName].Contains(action))
            {
                eventActions[eventName].Add(action);
            }
        }

        public void RemoveListener(string eventName, EventListener listener)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Remove(listener);
            }
        }

        public void RemoveListener(string eventName, Action<EventData> action)
        {
            if (eventActions.ContainsKey(eventName))
            {
                eventActions[eventName].Remove(action);
            }
        }
        #endregion

        #region Event Dispatch
        public void Dispatch(string eventName, EventData eventData = null)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                // Create a copy of the list to avoid modification during iteration
                var listeners = new List<EventListener>(eventListeners[eventName]);
                foreach (var listener in listeners)
                {
                    if (listener != null)
                    {
                        listener.OnEventRaised(eventData);
                    }
                }
            }

            if (eventActions.ContainsKey(eventName))
            {
                // Create a copy of the list to avoid modification during iteration
                var actions = new List<Action<EventData>>(eventActions[eventName]);
                foreach (var action in actions)
                {
                    action?.Invoke(eventData);
                }
            }
        }
        #endregion

        #region Event Cleanup
        public void ClearEventListeners(string eventName)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Clear();
            }

            if (eventActions.ContainsKey(eventName))
            {
                eventActions[eventName].Clear();
            }
        }

        public void ClearAllEvents()
        {
            eventListeners.Clear();
            eventActions.Clear();
        }
        #endregion
    }

    /// <summary>
    /// Base class for event data
    /// </summary>
    public class EventData
    {
        public string EventName { get; private set; }
        public object Sender { get; private set; }
        public Dictionary<string, object> Parameters { get; private set; }

        public EventData(string eventName, object sender = null)
        {
            EventName = eventName;
            Sender = sender;
            Parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string key, object value)
        {
            Parameters[key] = value;
        }

        public T GetParameter<T>(string key)
        {
            if (Parameters.TryGetValue(key, out object value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }
            }
            return default;
        }
    }

    /// <summary>
    /// Component for listening to events
    /// </summary>
    public class EventListener : MonoBehaviour
    {
        [SerializeField]
        private string[] eventNames;


        public event Action<EventData> OnEvent;

        private void OnEnable()
        {
            foreach (var eventName in eventNames)
            {
                EventManager.Instance.AddListener(eventName, this);
            }
        }

        private void OnDisable()
        {
            foreach (var eventName in eventNames)
            {
                EventManager.Instance.RemoveListener(eventName, this);
            }
        }

        public void OnEventRaised(EventData eventData)
        {
            OnEvent?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Component for dispatching events
    /// </summary>
    public class EventDispatcher : MonoBehaviour
    {
        public void Dispatch(string eventName, EventData eventData = null)
        {
            EventManager.Instance.Dispatch(eventName, eventData);
        }

        public void DispatchWithData(string eventName, object sender = null, params (string key, object value)[] parameters)
        {
            var eventData = new EventData(eventName, sender);
            foreach (var (key, value) in parameters)
            {
                eventData.AddParameter(key, value);
            }
            EventManager.Instance.Dispatch(eventName, eventData);
        }
    }
}
