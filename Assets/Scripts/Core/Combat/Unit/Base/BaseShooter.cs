using UnityEngine;
using Core.Base.Event;
using Core.Combat.Bullet.Base;

namespace Core.Combat.Unit.Base
{
    /// <summary>
    /// Base class for all shooter behaviors
    /// Provides core shooting functionality and bullet management
    /// </summary>
    public abstract class BaseShooter : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [Tooltip("Shots per second")]
        [SerializeField] protected float fireRate = 1f;
        
        [Tooltip("Initial bullet velocity")]
        [SerializeField] protected float bulletSpeed = 10f;
        
        [Tooltip("Damage dealt by each bullet")]
        [SerializeField] protected float bulletDamage = 10f;
        
        [Tooltip("Bullet prefab to instantiate")]
        [SerializeField] protected GameObject bulletPrefab;

        /// <summary>
        /// Time when the next shot can be fired
        /// </summary>
        protected float nextFireTime;

        /// <summary>
        /// Initialize shooting parameters
        /// </summary>
        protected virtual void Awake()
        {
            nextFireTime = 0f;
        }

        /// <summary>
        /// Check if the shooter can fire based on fire rate
        /// </summary>
        /// <returns>True if enough time has passed since last shot</returns>
        public virtual bool CanShoot()
        {
            return Time.time >= nextFireTime;
        }

        /// <summary>
        /// Get the bullet prefab with its BaseBullet component
        /// </summary>
        /// <returns>BaseBullet component of the bullet prefab</returns>
        public virtual BaseBullet GetBulletPrefab()
        {
            return bulletPrefab?.GetComponent<BaseBullet>();
        }

        /// <summary>
        /// Fire a bullet in the specified direction
        /// </summary>
        /// <param name="direction">Direction vector in world space</param>
        public virtual void Shoot(Vector3 direction)
        {
            if (!CanShoot()) return;

            // Update next fire time
            nextFireTime = Time.time + (1f / fireRate);

            // Create bullet
            lastFiredBullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(direction));
            BaseBullet bulletComponent = lastFiredBullet.GetComponent<BaseBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.Initialize(direction * bulletSpeed, bulletDamage);
            }

            // Notify event system
            EventManager.Publish(new ShootEvent(gameObject, lastFiredBullet));
        }

        protected GameObject lastFiredBullet;

        public GameObject GetLastFiredBullet()
        {
            return lastFiredBullet;
        }
    }

    /// <summary>
    /// Event data for shooting actions
    /// </summary>
    public class ShootEvent
    {
        /// <summary>
        /// The GameObject that fired the shot
        /// </summary>
        public GameObject Shooter { get; private set; }

        /// <summary>
        /// The bullet GameObject that was fired
        /// </summary>
        public GameObject Bullet { get; private set; }

        /// <summary>
        /// Create a new shoot event
        /// </summary>
        /// <param name="shooter">The GameObject that fired the shot</param>
        /// <param name="bullet">The bullet GameObject that was fired</param>
        public ShootEvent(GameObject shooter, GameObject bullet)
        {
            Shooter = shooter;
            Bullet = bullet;
        }
    }
}
