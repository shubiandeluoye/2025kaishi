using UnityEngine;
using Core.Base.Event;
using Core.Combat.Unit.Base;
using Core.Skills.Base;

namespace Core.Combat.Unit.Base
{
    public abstract class BaseShooter : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] protected float fireRate = 1f;
        [SerializeField] protected float bulletSpeed = 10f;
        [SerializeField] protected float bulletDamage = 10f;
        [SerializeField] protected GameObject bulletPrefab;

        protected float nextFireTime;

        protected virtual void Awake()
        {
            nextFireTime = 0f;
        }

        public virtual bool CanShoot()
        {
            return Time.time >= nextFireTime;
        }

        public virtual void Shoot(Vector3 direction)
        {
            if (!CanShoot()) return;

            // Update next fire time
            nextFireTime = Time.time + (1f / fireRate);

            // Create bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(direction));
            BaseBullet bulletComponent = bullet.GetComponent<BaseBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Initialize(direction * bulletSpeed, bulletDamage);
            }

            // Notify event system
            EventManager.Trigger(new ShootEvent(gameObject, bullet));
        }
    }

    public class ShootEvent
    {
        public GameObject Shooter { get; private set; }
        public GameObject Bullet { get; private set; }

        public ShootEvent(GameObject shooter, GameObject bullet)
        {
            Shooter = shooter;
            Bullet = bullet;
        }
    }
}
