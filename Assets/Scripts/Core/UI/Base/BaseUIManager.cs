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

        #region UI Layer Management
        protected Dictionary<UILayer, List<BaseUIElement>> uiLayers = new Dictionary<UILayer, List<BaseUIElement>>();
        
        public enum UILayer
        {
            Background,
            Game,
            HUD,
            Popup,
            Modal,
            Tutorial,
            Loading,
            System
        }

        protected virtual void RegisterToLayer(BaseUIElement element, UILayer layer)
        {
            if (!uiLayers.ContainsKey(layer))
                uiLayers[layer] = new List<BaseUIElement>();
            
            uiLayers[layer].Add(element);
            SetLayerOrder(element, layer);
        }

        protected virtual void SetLayerOrder(BaseUIElement element, UILayer layer)
        {
            Canvas canvas = element.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = (int)layer * 100;
        }
        #endregion

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
                EventManager.Publish(EventNames.UI_SHOW, new UIShowEvent(elementName));
            }
        }

        public virtual void HideUI(string elementName)
        {
            if (uiElements.TryGetValue(elementName, out BaseUIElement element))
            {
                element.Hide();
                if (uiStack.Count > 0 && uiStack.Peek() == element)
                    uiStack.Pop();
                EventManager.Publish(EventNames.UI_HIDE, new UIHideEvent(elementName));
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

        #region UI Navigation
        protected virtual void NavigateBack()
        {
            if (uiStack.Count > 1)
            {
                var current = uiStack.Pop();
                current.PlayHideAnimation();
                var previous = uiStack.Peek();
                previous.PlayShowAnimation();
            }
        }

        protected virtual void NavigateForward(string elementName)
        {
            if (uiElements.TryGetValue(elementName, out BaseUIElement element))
            {
                if (uiStack.Count > 0)
                    uiStack.Peek().PlayHideAnimation();
                
                element.PlayShowAnimation();
                uiStack.Push(element);
            }
        }
        #endregion

        #region UI State Management
        [System.Serializable]
        protected class UIState
        {
            public string activeElement;
            public Dictionary<string, object> elementData;
        }

        protected Stack<UIState> stateHistory = new Stack<UIState>();

        public virtual void PushState()
        {
            var state = new UIState
            {
                activeElement = uiStack.Count > 0 ? uiStack.Peek().name : null,
                elementData = new Dictionary<string, object>()
            };
            
            foreach (var element in uiElements.Values)
                element.SaveState();
            
            stateHistory.Push(state);
        }

        public virtual void PopState()
        {
            if (stateHistory.Count > 0)
            {
                var state = stateHistory.Pop();
                foreach (var element in uiElements.Values)
                    element.RestoreState();
                
                if (!string.IsNullOrEmpty(state.activeElement))
                    ShowUI(state.activeElement);
            }
        }
        #endregion
    }
} 