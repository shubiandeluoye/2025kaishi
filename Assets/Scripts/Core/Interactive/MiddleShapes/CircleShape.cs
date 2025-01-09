using UnityEngine;
using Core.Combat.Bullet;
using Core.Level.Base;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 圆形交互区域
    /// 处理1级和2级子弹的碰撞逻辑
    /// </summary>
    public class CircleShape : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        [Tooltip("碰撞时是否显示特效")]
        protected bool showCollisionEffect = true;

        [SerializeField]
        [Tooltip("碰撞特效预制体")]
        protected GameObject collisionEffectPrefab;
        #endregion

        #region Collision Handling
        /// <summary>
        /// 处理子弹进入触发区域的逻辑
        /// 只响应1级和2级子弹
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            var bulletLevelSystem = bullet.GetComponent<BaseLevelSystem>();
            if (bulletLevelSystem == null) return;

            int bulletLevel = bulletLevelSystem.Level;
            if (bulletLevel == 1 || bulletLevel == 2)
            {
                HandleBulletCollision(bullet, bulletLevel);
            }
        }

        /// <summary>
        /// 处理子弹碰撞的具体逻辑
        /// </summary>
        protected virtual void HandleBulletCollision(BaseBullet bullet, int bulletLevel)
        {
            // 显示碰撞特效
            if (showCollisionEffect && collisionEffectPrefab != null)
            {
                Instantiate(collisionEffectPrefab, bullet.transform.position, Quaternion.identity);
            }

            // 销毁子弹
            bullet.DestroyBullet();
        }
        #endregion
    }
}
