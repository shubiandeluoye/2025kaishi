using UnityEngine;
using System;

namespace Core.Combat.Bullet.Base
{
    public abstract class BaseBullet : MonoBehaviour
    {
        #region Properties
        [Serializable]
        public class BulletSettings
        {
            public float speed = 20f;
            public float damage = 10f;
            public float lifeTime = 5f;
            public bool useGravity = false;
            public LayerMask collisionMask = -1;
        }

        [SerializeField]
        protected BulletSettings settings = new BulletSettings();

        protected Vector3 direction;
        protected bool isInitialized;
        protected Rigidbody rb;
        #endregion

        #region Events
        public event Action<Collision> OnBulletCollision;
        public event Action<BaseBullet> OnBulletDestroy;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = settings.useGravity;
            }
        }

        protected virtual void Start()
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"Bullet {gameObject.name} was not initialized before Start");
                Initialize(transform.forward);
            }

            Destroy(gameObject, settings.lifeTime);
        }

        protected virtual void FixedUpdate()
        {
            if (rb != null)
            {
                UpdateMovement();
            }
        }
        #endregion

        #region Initialization
        public virtual void Initialize(Vector3 direction, float? speed = null, float? damage = null)
        {
            this.direction = direction.normalized;
            if (speed.HasValue) settings.speed = speed.Value;
            if (damage.HasValue) settings.damage = damage.Value;
            
            isInitialized = true;
            
            if (rb != null)
            {
                rb.velocity = this.direction * settings.speed;
            }
        }
        #endregion

        #region Movement
        protected virtual void UpdateMovement()
        {
            if (!settings.useGravity)
            {
                // 保持恒定速度
                rb.velocity = direction * settings.speed;
            }
        }
        #endregion

        #region Collision
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (settings.collisionMask == -1 || (settings.collisionMask & (1 << collision.gameObject.layer)) != 0)
            {
                OnBulletCollision?.Invoke(collision);
                HandleCollision(collision);
            }
        }

        protected virtual void HandleCollision(Collision collision)
        {
            // 默认行为是销毁子弹
            DestroyBullet();
        }
        #endregion

        #region Utility
        public virtual void DestroyBullet()
        {
            OnBulletDestroy?.Invoke(this);
            Destroy(gameObject);
        }

        public virtual float GetDamage()
        {
            return settings.damage;
        }

        public virtual void SetDamage(float damage)
        {
            settings.damage = damage;
        }
        #endregion
    }
} 