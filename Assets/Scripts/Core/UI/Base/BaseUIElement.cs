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
            EventManager.Publish(EventNames.UI_SHOW, new UIShowEvent(gameObject.name));
            onShow?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            EventManager.Publish(EventNames.UI_HIDE, new UIHideEvent(gameObject.name));
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

        #region UI Events
        protected virtual void OnUIClick(Vector2 position) { }
        protected virtual void OnUIDrag(Vector2 position, Vector2 delta) { }
        protected virtual void OnUIScroll(float value) { }
        protected virtual void OnUIHover(bool isHovering) { }
        protected virtual void OnUIFocus(bool isFocused) { }
        #endregion

        #region Animation Events
        public virtual void OnAnimationStart() { }
        public virtual void OnAnimationComplete() { }
        #endregion

        #region Transition Events
        public virtual void OnTransitionInStart() { }
        public virtual void OnTransitionInComplete() { }
        public virtual void OnTransitionOutStart() { }
        public virtual void OnTransitionOutComplete() { }
        #endregion
    }
} 