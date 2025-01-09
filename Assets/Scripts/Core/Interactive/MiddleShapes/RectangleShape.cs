using UnityEngine;
using Core.Combat.Bullet;
using Core.Level.Base;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 矩形交互区域
    /// 处理1-3级子弹的碰撞逻辑
    /// </summary>
    public class RectangleShape : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        [Tooltip("碰撞特效预制体")]
        protected GameObject collisionEffectPrefab;

        [SerializeField]
        [Tooltip("不同等级子弹的碰撞音效")]
        protected AudioClip[] levelCollisionSounds;
        #endregion

        #region Collision Handling
        /// <summary>
        /// 处理子弹进入触发区域的逻辑
        /// 响应1-3级子弹
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            var bulletLevelSystem = bullet.GetComponent<BaseLevelSystem>();
            if (bulletLevelSystem == null) return;

            int bulletLevel = bulletLevelSystem.Level;
            if (bulletLevel >= 1 && bulletLevel <= 3)
            {
                HandleBulletCollision(bullet, bulletLevel);
            }
        }

        /// <summary>
        /// 处理子弹碰撞的具体逻辑
        /// </summary>
        protected virtual void HandleBulletCollision(BaseBullet bullet, int bulletLevel)
        {
            // 播放对应等级的碰撞效果
            PlayCollisionEffects(bullet.transform.position, bulletLevel);
            
            // 销毁子弹
            bullet.DestroyBullet();
        }
        #endregion

        #region Effects
        /// <summary>
        /// 播放碰撞特效和音效
        /// </summary>
        protected virtual void PlayCollisionEffects(Vector3 position, int bulletLevel)
        {
            if (collisionEffectPrefab != null)
            {
                Instantiate(collisionEffectPrefab, position, Quaternion.identity);
            }

            if (levelCollisionSounds != null && bulletLevel <= levelCollisionSounds.Length)
            {
                var sound = levelCollisionSounds[bulletLevel - 1];
                if (sound != null)
                {
                    AudioSource.PlayClipAtPoint(sound, position);
                }
            }
        }
        #endregion
    }
}
