using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Core.UI.Base
{
    public abstract class BaseUIElement : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] protected bool startVisible = true;
        [SerializeField] protected bool useAnimation = false;
        [SerializeField] protected float animationDuration = 0.5f;
        
        [Header("Events")]
        public UnityEvent onShow;
        public UnityEvent onHide;
        
        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;
        protected bool isVisible;
        protected float currentAnimationTime;

        protected virtual void Awake()
        {
            // Get required components
            canvasGroup = GetComponent<CanvasGroup>();
            if (!canvasGroup)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            rectTransform = GetComponent<RectTransform>();
            if (!rectTransform)
            {
                Debug.LogError($"BaseUIElement: {gameObject.name} requires a RectTransform component!");
            }
            
            // Set initial state
            isVisible = startVisible;
            SetVisibility(startVisible, false);
        }

        public virtual void Show(bool animate = true)
        {
            if (isVisible) return;
            SetVisibility(true, animate);
            onShow?.Invoke();
        }

        public virtual void Hide(bool animate = true)
        {
            if (!isVisible) return;
            SetVisibility(false, animate);
            onHide?.Invoke();
        }

        protected virtual void SetVisibility(bool visible, bool animate)
        {
            isVisible = visible;
            
            if (!animate || !useAnimation)
            {
                canvasGroup.alpha = visible ? 1f : 0f;
                canvasGroup.blocksRaycasts = visible;
                canvasGroup.interactable = visible;
                return;
            }

            // Reset animation time
            currentAnimationTime = visible ? 0f : animationDuration;
            
            // Start animation in Update
            enabled = true;
        }

        protected virtual void Update()
        {
            if (!useAnimation) return;

            if (isVisible)
            {
                currentAnimationTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(currentAnimationTime / animationDuration);
                canvasGroup.alpha = normalizedTime;
                
                if (normalizedTime >= 1f)
                {
                    enabled = false;
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                }
            }
            else
            {
                currentAnimationTime -= Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(currentAnimationTime / animationDuration);
                canvasGroup.alpha = normalizedTime;
                
                if (normalizedTime <= 0f)
                {
                    enabled = false;
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.interactable = false;
                }
            }
        }

        public virtual bool IsVisible()
        {
            return isVisible;
        }

        public virtual void SetAnimationDuration(float duration)
        {
            animationDuration = Mathf.Max(0.1f, duration);
        }
    }
}
