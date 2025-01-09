using UnityEngine;
using Core.Combat.Bullet;
using Core.Level.Base;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 梯形交互区域
    /// 处理2级及以上子弹的碰撞逻辑
    /// </summary>
    public class TrapezoidShape : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        [Tooltip("碰撞特效预制体")]
        protected GameObject collisionEffectPrefab;

        [SerializeField]
        [Tooltip("高等级子弹碰撞音效")]
        protected AudioClip highLevelCollisionSound;
        #endregion

        #region Collision Handling
        /// <summary>
        /// 处理子弹进入触发区域的逻辑
        /// 只响应2级及以上子弹
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            var bulletLevelSystem = bullet.GetComponent<BaseLevelSystem>();
            if (bulletLevelSystem == null) return;

            if (bulletLevelSystem.Level >= 2)
            {
                HandleBulletCollision(bullet, bulletLevelSystem.Level);
            }
        }

        /// <summary>
        /// 处理子弹碰撞的具体逻辑
        /// </summary>
        protected virtual void HandleBulletCollision(BaseBullet bullet, int bulletLevel)
        {
            // 播放碰撞效果
            PlayCollisionEffects(bullet.transform.position);
            
            // 销毁子弹
            bullet.DestroyBullet();
        }
        #endregion

        #region Effects
        /// <summary>
        /// 播放碰撞特效和音效
        /// </summary>
        protected virtual void PlayCollisionEffects(Vector3 position)
        {
            if (collisionEffectPrefab != null)
            {
                Instantiate(collisionEffectPrefab, position, Quaternion.identity);
            }

            if (highLevelCollisionSound != null)
            {
                AudioSource.PlayClipAtPoint(highLevelCollisionSound, position);
            }
        }
        #endregion
    }
}
