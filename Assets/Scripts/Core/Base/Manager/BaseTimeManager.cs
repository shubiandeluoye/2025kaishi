using UnityEngine;
using Core.Base.Event;

namespace Core.Base.Manager
{
    /// <summary>
    /// 时间管理器基类
    /// 处理游戏中所有时间相关的功能
    /// </summary>
    public abstract class BaseTimeManager : BaseManager
    {
        [Header("Time Settings")]
        [SerializeField] protected float defaultTimeScale = 1f;
        [SerializeField] protected float minTimeScale = 0.1f;
        [SerializeField] protected float maxTimeScale = 2f;

        protected float currentTimeScale;
        protected bool isPaused;

        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            currentTimeScale = defaultTimeScale;
            Time.timeScale = currentTimeScale;
        }

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<float>(EventNames.TIME_SCALE_CHANGE, OnTimeScaleChange);
            EventManager.Subscribe<object>(EventNames.TOGGLE_PAUSE, OnTogglePause);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<float>(EventNames.TIME_SCALE_CHANGE, OnTimeScaleChange);
            EventManager.Unsubscribe<object>(EventNames.TOGGLE_PAUSE, OnTogglePause);
        }

        private void OnTimeScaleChange(float newTimeScale)
        {
            SetTimeScale(newTimeScale);
            EventManager.Publish("TIME_SCALE_CHANGED", Time.timeScale);
        }

        private void OnTogglePause(object data)
        {
            TogglePause();
        }

        /// <summary>
        /// 设置时间缩放
        /// </summary>
        protected virtual void SetTimeScale(float scale, bool smooth = false, float duration = 0.5f)
        {
            scale = Mathf.Clamp(scale, minTimeScale, maxTimeScale);
            
            if (smooth)
                StartCoroutine(SmoothTimeScale(scale, duration));
            else
            {
                Time.timeScale = scale;
                EventManager.Publish(EventNames.TIME_SCALE_CHANGED, 
                    new TimeEventData { SlowdownFactor = scale, Duration = 0, Smooth = false });
            }
            
            currentTimeScale = scale;
        }

        protected System.Collections.IEnumerator SmoothTimeScale(float targetScale, float duration)
        {
            float startScale = Time.timeScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                Time.timeScale = Mathf.Lerp(startScale, targetScale, t);
                yield return null;
            }

            Time.timeScale = targetScale;
            EventManager.Publish(EventNames.TIME_SCALE_CHANGED, 
                new TimeEventData { SlowdownFactor = targetScale, Duration = duration, Smooth = true });
        }

        /// <summary>
        /// 暂停/恢复时间
        /// </summary>
        protected virtual void TogglePause()
        {
            if (isPaused)
            {
                Time.timeScale = currentTimeScale;
                isPaused = false;
                EventManager.Publish(EventNames.GAME_RESUMED, null);
            }
            else
            {
                currentTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                isPaused = true;
                EventManager.Publish(EventNames.GAME_PAUSED, null);
            }
        }
    }
} 