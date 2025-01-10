using UnityEngine;
using System;
using Core.Base.Pool;
using Core.Base.Event;
using Core.Combat.Unit.Base;
using Core.Combat.Team;

namespace Core.Combat.Bullet
{
    /// <summary>
    /// 子弹系统的基础类
    /// 提供基本的移动、碰撞和生命周期管理
    /// </summary>
    public abstract class BaseBullet : MonoBehaviour, ITeamMember
    {
        #region Properties
        /// <summary>
        /// 子弹基本设置
        /// 可在Unity编辑器中配置
        /// </summary>
        [Serializable]
        public class BulletSettings
        {
            [Tooltip("子弹移动速度")]
            public float speed = 20f;
            
            [Tooltip("子弹基础伤害")]
            public float damage = 10f;
            
            [Tooltip("子弹生存时间")]
            public float lifeTime = 5f;
            
            [Tooltip("是否受重力影响")]
            public bool useGravity = false;
            
            [Tooltip("碰撞检测层级")]
            public LayerMask collisionMask = -1;
        }

        [SerializeField]
        protected BulletSettings settings = new BulletSettings();
        
        [Header("Team Settings")]
        [SerializeField]
        [Tooltip("子弹所属队伍")]
        protected TeamType team = TeamType.System;

        // 添加公共访问器
        public BulletSettings Settings => settings;
        public TeamType Team => team;
        public float Damage => settings.damage;

        protected Vector3 direction;
        protected bool isInitialized;
        protected Rigidbody rb;
        protected float currentLifetime;
        protected BaseUnit owner;
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

        protected virtual void OnEnable()
        {
            currentLifetime = settings.lifeTime;
            isInitialized = false;
        }

        protected virtual void Start()
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"Bullet {gameObject.name} was not initialized before Start");
                Initialize(transform.forward);
            }
        }

        protected virtual void Update()
        {
            if (!isInitialized) return;

            UpdateLifetime();
            if (!settings.useGravity)
            {
                UpdateMovement();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!isInitialized || !settings.useGravity) return;
            
            if (rb != null)
            {
                UpdateMovement();
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// 初始化子弹
        /// </summary>
        public virtual void Initialize(Vector3 direction, float? speed = null, float? damage = null, BaseUnit owner = null, TeamType? team = null)
        {
            this.direction = direction.normalized;
            if (speed.HasValue) settings.speed = speed.Value;
            if (damage.HasValue) settings.damage = damage.Value;
            if (team.HasValue) this.team = team.Value;
            this.owner = owner;
            
            isInitialized = true;
            
            if (rb != null)
            {
                rb.velocity = this.direction * settings.speed;
            }
        }
        #endregion

        #region Movement
        /// <summary>
        /// 更新子弹移动
        /// </summary>
        protected virtual void UpdateMovement()
        {
            if (!settings.useGravity)
            {
                if (rb != null)
                {
                    rb.velocity = direction * settings.speed;
                }
                else
                {
                    transform.position += direction * (settings.speed * Time.deltaTime);
                }
            }
        }
        #endregion

        #region Collision
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (settings.collisionMask == -1 || (settings.collisionMask & (1 << collision.gameObject.layer)) != 0)
            {
                // 检查是否击中可伤害单位
                BaseUnit hitUnit = collision.gameObject.GetComponent<BaseUnit>();
                if (hitUnit != null)
                {
                    // 检查队伍关系
                    var hitTeamMember = hitUnit as ITeamMember;
                    if (hitTeamMember != null && !TeamSystem.IsFriend(team, hitTeamMember.Team))
                    {
                        ApplyDamage(hitUnit);
                        
                        // 发布子弹命中事件
                        EventManager.Publish(EventNames.BULLET_HIT, 
                            new BulletHitEvent(hitUnit.gameObject, settings.damage, collision.contacts[0].point));
                    }
                }

                OnBulletCollision?.Invoke(collision);
                HandleCollision(collision);
            }
        }

        /// <summary>
        /// 处理碰撞逻辑
        /// </summary>
        protected virtual void HandleCollision(Collision collision)
        {
            ApplyImpactEffects(collision);
            DestroyBullet();
        }

        /// <summary>
        /// 应用伤害
        /// </summary>
        protected virtual void ApplyDamage(BaseUnit target)
        {
            target.TakeDamage(settings.damage, owner);
        }

        /// <summary>
        /// 应用碰撞特效
        /// </summary>
        protected virtual void ApplyImpactEffects(Collision collision)
        {
            if (PoolManager.Instance != null)
            {
                GameObject impactEffect = PoolManager.Instance.Spawn("BulletImpact", 
                    collision.contacts[0].point, 
                    Quaternion.LookRotation(collision.contacts[0].normal));
                    
                if (impactEffect != null)
                {
                    PoolManager.Instance.Despawn("BulletImpact", impactEffect);
                }
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
                    DestroyBullet();
                }
            }
        }

        /// <summary>
        /// 销毁子弹
        /// </summary>
        public virtual void DestroyBullet()
        {
            OnBulletDestroy?.Invoke(this);
            
            // 发布子弹销毁事件
            EventManager.Publish(EventNames.BULLET_DESTROYED, 
                new BulletDestroyedEvent(this, transform.position));
                
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.Despawn("Bullet", gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Utility
        /// <summary>
        /// 获取子弹伤害值
        /// </summary>
        public virtual float GetDamage()
        {
            return settings.damage;
        }

        /// <summary>
        /// 设置子弹伤害值
        /// </summary>
        public virtual void SetDamage(float damage)
        {
            settings.damage = damage;
        }

        /// <summary>
        /// 修改子弹轨迹
        /// </summary>
        public virtual void ModifyTrajectory(Vector3 newDirection, float? newSpeed = null)
        {
            direction = newDirection.normalized;
            if (newSpeed.HasValue)
            {
                settings.speed = newSpeed.Value;
            }

            if (rb != null && settings.useGravity)
            {
                rb.velocity = direction * settings.speed;
            }

            // 发布轨迹修改事件
            EventManager.Publish(EventNames.BULLET_TRAJECTORY_MODIFIED, 
                new BulletTrajectoryEvent(gameObject, direction, 0f, true));
        }
        #endregion
    }
} 