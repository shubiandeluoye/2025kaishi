using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Core.UI.Base
{
    public abstract class BaseUIManager : MonoBehaviour
    {
        protected Dictionary<string, BaseUIElement> uiElements = new Dictionary<string, BaseUIElement>();
        protected Stack<BaseUIElement> uiStack = new Stack<BaseUIElement>();

        [SerializeField] protected bool initializeOnAwake = true;
        
        public UnityEvent onManagerInitialized;
        public UnityEvent onManagerDestroyed;

        protected virtual void Awake()
        {
            if (initializeOnAwake)
                Initialize();
        }

        public virtual void Initialize()
        {
            RegisterUIElements();
            RegisterEvents();
            onManagerInitialized?.Invoke();
        }

        protected virtual void RegisterUIElements()
        {
            var elements = GetComponentsInChildren<BaseUIElement>(true);
            foreach (var element in elements)
            {
                if (!uiElements.ContainsKey(element.name))
                    uiElements.Add(element.name, element);
            }
        }

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        public virtual T GetUIElement<T>(string elementName) where T : BaseUIElement
        {
            if (uiElements.TryGetValue(elementName, out BaseUIElement element))
                return element as T;
            return null;
        }

        public virtual void ShowUI(string elementName)
        {
            if (uiElements.TryGetValue(elementName, out BaseUIElement element))
            {
                element.Show();
                uiStack.Push(element);
            }
        }

        public virtual void HideUI(string elementName)
        {
            if (uiElements.TryGetValue(elementName, out BaseUIElement element))
            {
                element.Hide();
                if (uiStack.Count > 0 && uiStack.Peek() == element)
                    uiStack.Pop();
            }
        }

        public virtual void HideAllUI()
        {
            foreach (var element in uiElements.Values)
                element.Hide();
            uiStack.Clear();
        }

        public virtual void ShowPrevious()
        {
            if (uiStack.Count > 1)
            {
                var current = uiStack.Pop();
                current.Hide();
                var previous = uiStack.Peek();
                previous.Show();
            }
        }

        protected virtual void OnDestroy()
        {
            UnregisterEvents();
            onManagerDestroyed?.Invoke();
        }

        // UI Transition methods (for future use)
        protected virtual void TransitionTo(string elementName) { }
        protected virtual void TransitionBack() { }

        // UI Animation methods (for future use)
        protected virtual void PlayTransitionAnimation(BaseUIElement from, BaseUIElement to) { }
        
        // UI State management (for future use)
        public virtual void SaveUIState() { }
        public virtual void RestoreUIState() { }
    }
} 