using UnityEngine;
using Core.Base.Event;

namespace Core.Combat.Unit.Base
{
    /// <summary>
    /// 射击单位的基类
    /// 提供基础的射击功能和事件发布
    /// </summary>
    public class BaseShooter : MonoBehaviour
    {
        #region 字段和属性
        [Header("Shooting Parameters")]
        [Tooltip("每秒发射子弹的次数")]
        [SerializeField] protected float fireRate = 0.5f;
        
        [Tooltip("子弹飞行速度")]
        [SerializeField] protected float bulletSpeed = 20f;
        
        [Tooltip("子弹伤害值")]
        [SerializeField] protected float bulletDamage = 10f;
        
        [Tooltip("子弹等级（影响特效和伤害）")]
        [SerializeField] protected int bulletLevel = 1;

        // 射击冷却计时器
        protected float shootCooldown;
        
        // 是否可以射击
        protected bool canShoot = true;

        protected EventManager.BulletEventData lastFiredBullet;
        #endregion

        #region Unity生命周期
        protected virtual void Update()
        {
            // 更新射击冷却
            if (shootCooldown > 0)
            {
                shootCooldown -= Time.deltaTime;
            }
        }
        #endregion

        #region 射击系统
        /// <summary>
        /// 检查是否可以射击
        /// </summary>
        protected virtual bool CanFire()
        {
            return canShoot && shootCooldown <= 0;
        }

        /// <summary>
        /// 执行射击 - 内部方法
        /// </summary>
        protected virtual void Fire(Vector3 direction)
        {
            if (!CanFire()) return;

            lastFiredBullet = new EventManager.BulletEventData
            {
                Position = transform.position,
                Direction = direction.normalized,
                Damage = bulletDamage,
                Level = bulletLevel,
                PathPoints = CalculateBulletPath(direction),
                Duration = CalculateBulletDuration()
            };

            EventManager.Publish(EventManager.EventNames.BULLET_FIRED, lastFiredBullet);
            shootCooldown = 1f / fireRate;
        }

        /// <summary>
        /// 公共射击方法 - 供技能系统使用
        /// </summary>
        public virtual void Shoot(Vector3? direction = null)
        {
            Fire(direction ?? transform.forward);
        }

        /// <summary>
        /// 获取最后发射的子弹数据
        /// </summary>
        public virtual EventManager.BulletEventData GetLastFiredBullet()
        {
            return lastFiredBullet;
        }

        /// <summary>
        /// 计算子弹路径点
        /// </summary>
        protected virtual Vector3[] CalculateBulletPath(Vector3 direction)
        {
            return new Vector3[] 
            { 
                transform.position,
                transform.position + direction * bulletSpeed
            };
        }

        /// <summary>
        /// 计算子弹飞行时间
        /// </summary>
        protected virtual float CalculateBulletDuration()
        {
            return bulletSpeed > 0 ? 1f / bulletSpeed : 0.5f;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置射击参数
        /// </summary>
        public virtual void SetShootingParameters(float rate, float speed, float damage, int level)
        {
            fireRate = rate;
            bulletSpeed = speed;
            bulletDamage = damage;
            bulletLevel = level;
        }

        /// <summary>
        /// 启用/禁用射击功能
        /// </summary>
        public virtual void SetCanShoot(bool enabled)
        {
            canShoot = enabled;
        }

        /// <summary>
        /// 获取当前射击冷却时间
        /// </summary>
        public float GetShootCooldown()
        {
            return shootCooldown;
        }

        /// <summary>
        /// 重置射击冷却
        /// </summary>
        public void ResetShootCooldown()
        {
            shootCooldown = 0f;
        }
        #endregion
    }
}
