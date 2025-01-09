using UnityEngine;
using Core.Combat.Bullet;
using Core.Level.Base;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 2级到3级的转换区域
    /// 将2级子弹升级为3级子弹
    /// </summary>
    public class Level2To3Shape : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        [Tooltip("升级特效预制体")]
        protected GameObject levelUpEffectPrefab;

        [SerializeField]
        [Tooltip("升级音效")]
        protected AudioClip levelUpSound;
        #endregion

        #region Collision Handling
        /// <summary>
        /// 处理子弹进入触发区域的逻辑
        /// 只响应2级子弹并尝试升级
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            var bulletLevelSystem = bullet.GetComponent<BaseLevelSystem>();
            if (bulletLevelSystem == null) return;

            if (bulletLevelSystem.Level == 2)
            {
                HandleLevelUpAttempt(bullet, bulletLevelSystem);
            }
        }

        /// <summary>
        /// 处理子弹升级尝试
        /// </summary>
        protected virtual void HandleLevelUpAttempt(BaseBullet bullet, BaseLevelSystem levelSystem)
        {
            if (levelSystem.TryLevelUp())
            {
                PlayLevelUpEffects(bullet.transform.position);
            }
        }
        #endregion

        #region Effects
        /// <summary>
        /// 播放升级特效和音效
        /// </summary>
        protected virtual void PlayLevelUpEffects(Vector3 position)
        {
            if (levelUpEffectPrefab != null)
            {
                Instantiate(levelUpEffectPrefab, position, Quaternion.identity);
            }

            if (levelUpSound != null)
            {
                AudioSource.PlayClipAtPoint(levelUpSound, position);
            }
        }
        #endregion
    }
}
