using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.Managers
{
    public class TimeManager : BaseTimeManager
    {
        [Header("Bullet Time")]
        [SerializeField] private float bulletTimeScale = 0.3f;
        [SerializeField] private float bulletTimeDuration = 1f;
        
        [Header("Hit Stop")]
        [SerializeField] private float hitStopDuration = 0.1f;
        [SerializeField] private float hitStopTimeScale = 0f;

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<EventManager.TimeEventData>(EventManager.EventNames.BULLET_FIRED, OnBulletTime);
            EventManager.Subscribe<EventManager.TimeEventData>(EventManager.EventNames.BULLET_HIT, OnHitStop);
            EventManager.Subscribe<EventManager.TimeEventData>(EventManager.EventNames.GAME_PAUSED, OnGamePaused);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<EventManager.TimeEventData>(EventManager.EventNames.BULLET_FIRED, OnBulletTime);
            EventManager.Unsubscribe<EventManager.TimeEventData>(EventManager.EventNames.BULLET_HIT, OnHitStop);
            EventManager.Unsubscribe<EventManager.TimeEventData>(EventManager.EventNames.GAME_PAUSED, OnGamePaused);
        }

        private void OnBulletTime(EventManager.TimeEventData data)
        {
            SetTimeScale(bulletTimeScale, true, bulletTimeDuration);
        }

        private void OnHitStop(EventManager.TimeEventData data)
        {
            StartCoroutine(HitStopRoutine());
        }

        private void OnGamePaused(EventManager.TimeEventData data)
        {
            SetTimeScale(0f);
        }

        private System.Collections.IEnumerator HitStopRoutine()
        {
            SetTimeScale(hitStopTimeScale);
            yield return new WaitForSecondsRealtime(hitStopDuration);
            SetTimeScale(defaultTimeScale, true);
        }

        public void TriggerBulletTime()
        {
            var data = new EventManager.TimeEventData
            {
                SlowdownFactor = bulletTimeScale,
                Duration = bulletTimeDuration,
                Smooth = true
            };
            EventManager.Publish(EventManager.EventNames.BULLET_FIRED, data);
        }

        public void TriggerHitStop()
        {
            var data = new EventManager.TimeEventData
            {
                SlowdownFactor = hitStopTimeScale,
                Duration = hitStopDuration,
                Smooth = false
            };
            EventManager.Publish(EventManager.EventNames.BULLET_HIT, data);
        }
    }
} 