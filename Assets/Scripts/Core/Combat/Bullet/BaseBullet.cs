using UnityEngine;
using System;

namespace Core.Combat.Bullet
{
    /// <summary>
    /// 子弹系统的基础类
    /// 提供基本的移动、碰撞和生命周期管理
    /// </summary>
    public abstract class BaseBullet : MonoBehaviour
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
        
        // 添加公共访问器
        public BulletSettings Settings => settings;

        protected Vector3 direction;
        protected bool isInitialized;
        protected Rigidbody rb;
        #endregion

        #region Events
        // 子弹碰撞和销毁事件
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
        /// <summary>
        /// 初始化子弹
        /// </summary>
        /// <param name="direction">子弹移动方向</param>
        /// <param name="speed">可选：覆盖默认速度</param>
        /// <param name="damage">可选：覆盖默认伤害</param>
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
        /// <summary>
        /// 更新子弹移动
        /// 如果不使用重力，保持恒定速度
        /// </summary>
        protected virtual void UpdateMovement()
        {
            if (!settings.useGravity)
            {
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

        /// <summary>
        /// 处理碰撞逻辑
        /// 子类可以重写此方法来实现特定的碰撞行为
        /// </summary>
        protected virtual void HandleCollision(Collision collision)
        {
            // 默认行为是销毁子弹
            DestroyBullet();
        }
        #endregion

        #region Utility
        /// <summary>
        /// 销毁子弹
        /// </summary>
        public virtual void DestroyBullet()
        {
            OnBulletDestroy?.Invoke(this);
            Destroy(gameObject);
        }

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
        #endregion
    }
} 