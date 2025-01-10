using UnityEngine;
using Core.Combat.Bullet;
using Core.Base.Manager;
using Core.Combat.Team;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 三角形交互区域
    /// - 三边都可以反弹子弹
    /// - 子弹撞击会导致旋转
    /// - 玩家子弹增加旋转速度，敌方子弹减少旋转速度
    /// - 30秒后消失
    /// </summary>
    public class TriangleShape : BaseManager
    {
        #region Properties
        [Header("Triangle Settings")]
        [SerializeField] protected float lifetime = 30f;           // 生存时间
        [SerializeField] protected float maxRotationSpeed = 360f;  // 最大旋转速度（度/秒）
        [SerializeField] protected float minRotationSpeed = -360f; // 最小旋转速度
        [SerializeField] protected float rotationSpeedPerHit = 30f;// 每次撞击增加的旋转速度
        [SerializeField] protected float naturalDeceleration = 5f; // 自然减速
        
        [Header("Bounce Settings")]
        [SerializeField] protected float bounceForce = 1.2f;      // 反弹力度倍数
        [SerializeField] protected GameObject bounceEffectPrefab;  // 反弹特效预制体
        [SerializeField] protected AudioClip bounceSound;         // 反弹音效

        [Header("Effect Settings")]
        [SerializeField] protected GameObject deactivateEffectPrefab; // 消失特效预制体

        protected float currentRotationSpeed;
        protected float currentLifetime;
        protected bool isActive = true;
        #endregion

        #region Unity Lifecycle
        protected virtual void Start()
        {
            currentLifetime = lifetime;
            currentRotationSpeed = 0f;
        }

        protected virtual void Update()
        {
            if (!isActive) return;

            // 更新生存时间
            currentLifetime -= Time.deltaTime;
            if (currentLifetime <= 0)
            {
                DeactivateTriangle();
                return;
            }

            // 应用自然减速
            if (Mathf.Abs(currentRotationSpeed) > 0.1f)
            {
                currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, 0f, 
                    naturalDeceleration * Time.deltaTime);
            }

            // 应用旋转
            transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);
        }
        #endregion

        #region Collision Handling
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (!isActive) return;

            var bullet = collision.gameObject.GetComponent<BaseBullet>();
            if (bullet == null) return;

            // 获取碰撞点
            Vector3 hitPoint = collision.contacts[0].point;
            
            // 计算碰撞点相对于三角形中心的位置
            Vector3 localHitPoint = transform.InverseTransformPoint(hitPoint);
            
            // 根据碰撞点的x坐标判断是左侧还是右侧
            float rotationDirection = localHitPoint.x > 0 ? -1f : 1f; // 右侧逆时针(-1)，左侧顺时针(1)
            
            // 更新旋转速度
            UpdateRotationSpeed(rotationDirection);

            // 计算反弹
            Vector3 normal = collision.contacts[0].normal;
            Vector3 incomingVelocity = bullet.GetComponent<Rigidbody>().velocity;
            Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal) * bounceForce;
            
            // 应用反弹
            bullet.GetComponent<Rigidbody>().velocity = reflectedVelocity;

            // 播放效果
            PlayBounceEffects(hitPoint);
        }

        protected virtual void UpdateRotationSpeed(float direction)
        {
            // 根据撞击位置添加旋转速度
            currentRotationSpeed = Mathf.Clamp(
                currentRotationSpeed + (rotationSpeedPerHit * direction),
                minRotationSpeed,
                maxRotationSpeed
            );
        }
        #endregion

        #region Effects
        protected virtual void PlayBounceEffects(Vector3 position)
        {
            if (bounceEffectPrefab != null)
            {
                Instantiate(bounceEffectPrefab, position, Quaternion.identity);
            }

            if (bounceSound != null)
            {
                AudioSource.PlayClipAtPoint(bounceSound, position);
            }
        }

        protected virtual void DeactivateTriangle()
        {
            isActive = false;
            
            // 播放消失效果
            if (deactivateEffectPrefab != null)
            {
                Instantiate(deactivateEffectPrefab, transform.position, Quaternion.identity);
            }

            // 可以选择直接销毁或者返回对象池
            Destroy(gameObject);
        }
        #endregion

        #region Public Methods
        public virtual float GetCurrentRotationSpeed()
        {
            return currentRotationSpeed;
        }

        public virtual float GetRemainingLifetime()
        {
            return currentLifetime;
        }
        #endregion
    }
} 