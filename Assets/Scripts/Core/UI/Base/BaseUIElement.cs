using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Core.UI.Base
{
    public abstract class BaseUIElement : MonoBehaviour
    {
        [SerializeField] protected bool showOnAwake = false;
        [SerializeField] protected bool keepAlive = false;

        public UnityEvent onShow;
        public UnityEvent onHide;
        public UnityEvent onInitialize;

        protected Dictionary<string, object> elementData = new Dictionary<string, object>();
        protected bool isInitialized = false;

        protected virtual void Awake()
        {
            if (showOnAwake)
                Show();
            else
                Hide();
        }

        public virtual void Show()
        {
            if (!isInitialized)
                Initialize();
            
            gameObject.SetActive(true);
            onShow?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            onHide?.Invoke();

            if (!keepAlive)
                CleanUp();
        }

        public virtual void Initialize()
        {
            isInitialized = true;
            onInitialize?.Invoke();
        }

        public virtual void SetData<T>(string key, T value)
        {
            elementData[key] = value;
            OnDataUpdated(key);
        }

        public virtual T GetData<T>(string key)
        {
            return elementData.ContainsKey(key) ? (T)elementData[key] : default;
        }

        protected virtual void OnDataUpdated(string key) { }

        protected virtual void CleanUp()
        {
            elementData.Clear();
            isInitialized = false;
        }

        protected virtual void OnDestroy()
        {
            CleanUp();
        }

        // Animation related methods (for future use)
        public virtual void PlayShowAnimation() { }
        public virtual void PlayHideAnimation() { }
        
        // Transition related methods (for future use)
        public virtual void OnTransitionIn() { }
        public virtual void OnTransitionOut() { }
        
        // State management (for future use)
        public virtual void SaveState() { }
        public virtual void RestoreState() { }
    }
} 