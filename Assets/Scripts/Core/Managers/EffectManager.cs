using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.Managers
{
    public class EffectManager : BaseEffectManager
    {
        [Header("Bullet Effects")]
        [SerializeField] private GameObject bulletTrailPrefab;
        [SerializeField] private GameObject bulletImpactPrefab;
        [SerializeField] private GameObject bulletExplosionPrefab;

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<EventManager.BulletEventData>(EventManager.EventNames.BULLET_FIRED, OnBulletFired);
            EventManager.Subscribe<EventManager.BulletEventData>(EventManager.EventNames.BULLET_HIT, OnBulletHit);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<EventManager.BulletEventData>(EventManager.EventNames.BULLET_FIRED, OnBulletFired);
            EventManager.Unsubscribe<EventManager.BulletEventData>(EventManager.EventNames.BULLET_HIT, OnBulletHit);
        }

        private void OnBulletFired(EventManager.BulletEventData data)
        {
            if (bulletTrailPrefab != null)
            {
                PlayEffect(bulletTrailPrefab, data.Position, Quaternion.LookRotation(data.Direction));
            }
        }

        private void OnBulletHit(EventManager.BulletEventData data)
        {
            if (data.Level >= 3 && bulletImpactPrefab != null)
            {
                PlayEffect(bulletImpactPrefab, data.Position, Quaternion.identity);
            }
            
            if (data.Level >= 4 && bulletExplosionPrefab != null)
            {
                PlayEffect(bulletExplosionPrefab, data.Position, Quaternion.identity);
            }
        }
    }
} 