using UnityEngine;
using System;
using Core.Base.Pool;

namespace Core.Combat.Unit.Base
{
    public class BaseBullet : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        protected float lifetime = 5f;
        
        [SerializeField]
        protected bool useGravity = false;
        
        [SerializeField]
        protected LayerMask collisionMask;
        
        protected float damage;
        protected float speed;
        protected BaseUnit owner;
        protected Vector3 direction;
        protected float currentLifetime;
        protected bool isInitialized;
        
        protected Rigidbody rb;
        #endregion

        #region Events
        public event Action<BaseBullet, Collision> OnBulletCollision;
        public event Action<BaseBullet> OnBulletDestroy;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = useGravity;
            }
        }

        protected virtual void OnEnable()
        {
            currentLifetime = lifetime;
            isInitialized = false;
        }

        protected virtual void Update()
        {
            if (!isInitialized) return;

            UpdateLifetime();
            if (!useGravity)
            {
                UpdateMovement();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!isInitialized || !useGravity) return;
            
            UpdatePhysicsMovement();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }
        #endregion

        #region Initialization
        public virtual void Initialize(BaseUnit owner, float damage, float speed, Vector3? direction = null)
        {
            this.owner = owner;
            this.damage = damage;
            this.speed = speed;
            this.direction = direction ?? transform.forward;
            
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            isInitialized = true;
        }
        #endregion

        #region Movement
        protected virtual void UpdateMovement()
        {
            transform.position += direction * (speed * Time.deltaTime);
        }

        protected virtual void UpdatePhysicsMovement()
        {
            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
        }
        #endregion

        #region Collision Handling
        protected virtual void HandleCollision(Collision collision)
        {
            BaseUnit hitUnit = collision.gameObject.GetComponent<BaseUnit>();
            if (hitUnit != null && hitUnit != owner)
            {
                ApplyDamage(hitUnit);
            }

            OnBulletCollision?.Invoke(this, collision);
            
            ApplyImpactEffects(collision);
            
            ReturnToPool();
        }

        protected virtual void ApplyDamage(BaseUnit target)
        {
            target.TakeDamage(damage, owner);
        }

        protected virtual void ApplyImpactEffects(Collision collision)
        {
            GameObject impactEffect = PoolManager.Instance.Spawn("BulletImpact", 
                collision.contacts[0].point, 
                Quaternion.LookRotation(collision.contacts[0].normal));
                
            if (impactEffect != null)
            {
                PoolManager.Instance.Despawn("BulletImpact", impactEffect);
            }
        }
        #endregion

        #region Lifetime Management
        protected virtual void UpdateLifetime()
        {
            if (currentLifetime > 0)
            {
                currentLifetime -= Time.deltaTime;
                if (currentLifetime <= 0)
                {
                    ReturnToPool();
                }
            }
        }

        protected virtual void ReturnToPool()
        {
            OnBulletDestroy?.Invoke(this);
            PoolManager.Instance.Despawn("Bullet", gameObject);
        }
        #endregion


        #region Trajectory Modification
        public virtual void ModifyTrajectory(Vector3 newDirection, float? newSpeed = null)
        {
            direction = newDirection.normalized;
            if (newSpeed.HasValue)
            {
                speed = newSpeed.Value;
            }

            if (rb != null && useGravity)
            {
                rb.velocity = direction * speed;
            }
        }

        public virtual void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            if (rb != null)
            {
                rb.AddForce(force, mode);
                direction = rb.velocity.normalized;
                speed = rb.velocity.magnitude;
            }
        }
        #endregion

        #region Getters
        public float GetDamage() => damage;
        public float GetSpeed() => speed;
        public BaseUnit GetOwner() => owner;
        public Vector3 GetDirection() => direction;
        public float GetRemainingLifetime() => currentLifetime;
        #endregion
    }
}
