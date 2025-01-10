using UnityEngine;
using Core.Combat.Bullet;
using Core.Base.Manager;
using Core.Combat.Team;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 矩形交互区域
    /// - 被子弹击中后消失
    /// - 支持不同的消失方式
    /// - 可配置的碰撞规则
    /// </summary>
    public class RectangleShape : BaseManager
    {
        #region Properties
        [Header("Rectangle Settings")]
        [SerializeField] protected Vector2 size = new Vector2(2.5f, 4f);  // 矩形大小
        
        [Header("Collision Settings")]
        [SerializeField] protected CollisionBehavior collisionBehavior = CollisionBehavior.BothDestroy;
        [SerializeField] protected bool checkTeam = false;  // 是否检查队伍
        [SerializeField] protected TeamType[] targetTeams;  // 目标队伍（如果checkTeam为true）

        [Header("Effect Settings")]
        [SerializeField] protected GameObject destroyEffectPrefab;
        [SerializeField] protected AudioClip destroySound;

        // 碰撞行为枚举
        public enum CollisionBehavior
        {
            BothDestroy,     // 子弹和矩形都消失
            OnlyRectangle,   // 只有矩形消失
            OnlyBullet,      // 只有子弹消失
            Custom           // 自定义行为
        }
        #endregion

        #region Unity Lifecycle
        protected virtual void Start()
        {
            // 设置碰撞体大小
            if (TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
            {
                boxCollider.size = new Vector3(size.x, size.y, 1f);
            }
        }
        #endregion

        #region Collision Handling
        protected virtual void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            // 检查队伍（如果启用）
            if (checkTeam && !IsValidTeam(bullet.Team))
            {
                return;
            }

            // 处理碰撞
            HandleCollision(bullet);
        }

        protected virtual bool IsValidTeam(TeamType bulletTeam)
        {
            if (targetTeams == null || targetTeams.Length == 0)
                return true;

            foreach (var team in targetTeams)
            {
                if (team == bulletTeam)
                    return true;
            }
            return false;
        }

        protected virtual void HandleCollision(BaseBullet bullet)
        {
            switch (collisionBehavior)
            {
                case CollisionBehavior.BothDestroy:
                    DestroyBoth(bullet);
                    break;
                case CollisionBehavior.OnlyRectangle:
                    DestroyRectangle();
                    break;
                case CollisionBehavior.OnlyBullet:
                    DestroyBullet(bullet);
                    break;
                case CollisionBehavior.Custom:
                    HandleCustomCollision(bullet);
                    break;
            }
        }

        protected virtual void DestroyBoth(BaseBullet bullet)
        {
            PlayDestroyEffects();
            bullet.DestroyBullet();
            DestroyRectangle();
        }

        protected virtual void DestroyRectangle()
        {
            PlayDestroyEffects();
            Destroy(gameObject);
        }

        protected virtual void DestroyBullet(BaseBullet bullet)
        {
            bullet.DestroyBullet();
        }

        protected virtual void HandleCustomCollision(BaseBullet bullet)
        {
            // 可以在子类中重写实现自定义碰撞行为
        }
        #endregion

        #region Effects
        protected virtual void PlayDestroyEffects()
        {
            if (destroyEffectPrefab != null)
            {
                Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            }

            if (destroySound != null)
            {
                AudioSource.PlayClipAtPoint(destroySound, transform.position);
            }
        }
        #endregion

        #region Public Methods
        public virtual void SetSize(Vector2 newSize)
        {
            size = newSize;
            if (TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
            {
                boxCollider.size = new Vector3(size.x, size.y, 1f);
            }
        }

        public virtual Vector2 GetSize()
        {
            return size;
        }
        #endregion
    }
}
