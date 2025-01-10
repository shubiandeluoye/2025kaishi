using UnityEngine;
using Core.Base.Manager;
using Core.Base.Event;
using Core.Combat.Bullet;

namespace Core.Interactive.Base
{
    public abstract class BaseShape : BaseManager
    {
        [Header("Effect Settings")]
        [SerializeField] protected GameObject collisionEffectPrefab;
        [SerializeField] protected AudioClip collisionSound;

        protected virtual void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            var bulletLevelSystem = bullet.GetComponent<BaseLevelSystem>();
            if (bulletLevelSystem == null) return;

            if (IsValidBulletLevel(bulletLevelSystem.Level))
            {
                HandleBulletCollision(bullet, bulletLevelSystem.Level);
            }
        }

        protected abstract bool IsValidBulletLevel(int level);

        protected virtual void HandleBulletCollision(BaseBullet bullet, int bulletLevel)
        {
            EventManager.Publish(EventNames.SHAPE_COLLISION, 
                new ShapeCollisionEvent(bullet.gameObject, bulletLevel, bullet.transform.position, GetType().Name));

            PlayEffects(bullet.transform.position);
            bullet.DestroyBullet();
        }

        protected virtual void PlayEffects(Vector3 position)
        {
            if (collisionEffectPrefab != null)
            {
                Instantiate(collisionEffectPrefab, position, Quaternion.identity);
            }

            if (collisionSound != null)
            {
                AudioSource.PlayClipAtPoint(collisionSound, position);
            }
        }
    }
} 