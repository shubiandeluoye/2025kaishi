using UnityEngine;
using Core.Combat.Bullet;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.Interactive.MiddleShapes
{
    /// <summary>
    /// 碰撞事件数据类
    /// </summary>
    public class ShapeCollisionEvent
    {
        public BaseBullet Bullet { get; private set; }
        public string CollisionType { get; private set; }  // "SPLIT", "BOUNCE", "BOOST"
        public Vector3 Position { get; private set; }

        public ShapeCollisionEvent(BaseBullet bullet, string type, Vector3 position)
        {
            Bullet = bullet;
            CollisionType = type;
            Position = position;
        }
    }

    /// <summary>
    /// 梯形交互区域
    /// - 从上底射入：子弹分裂成2个，沿梯形腰线向内飞行
    /// - 从下底射入：碰到腰线反弹
    /// - 从上底飞出：子弹速度增加50%
    /// - 支持物理碰撞移动和旋转
    /// </summary>
    public class TrapezoidShape : BaseManager
    {
        #region Properties
        [Header("Trapezoid Settings")]
        [SerializeField] protected Transform upperBase;    // 上底
        [SerializeField] protected Transform lowerBase;    // 下底
        [SerializeField] protected Transform leftLeg;      // 左腰
        [SerializeField] protected Transform rightLeg;     // 右腰
        [SerializeField] protected float splitAngle = 15f; // 分裂角度
        [SerializeField] protected float speedBoost = 1.5f;// 速度提升倍数

        [Header("Physics Settings")]
        [SerializeField] protected bool usePhysics = true;         // 是否启用物理
        [SerializeField] protected float mass = 1f;                // 质量
        [SerializeField] protected float drag = 0.5f;             // 空气阻力
        [SerializeField] protected float angularDrag = 0.5f;      // 角阻力
        [SerializeField] protected bool freezeRotationX = true;   // 锁定X轴旋转
        [SerializeField] protected bool freezeRotationY = false;  // 锁定Y轴旋转
        [SerializeField] protected bool freezeRotationZ = true;   // 锁定Z轴旋转

        [Header("Movement Limits")]
        [SerializeField] protected bool limitMovement = true;     // 是否限制移动
        [SerializeField] protected Vector2 movementBounds = new Vector2(5f, 5f); // X,Y移动范围
        [SerializeField] protected Vector2 rotationBounds = new Vector2(-45f, 45f); // 旋转角度限制

        protected Rigidbody rb;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("Effect Settings")]
        [SerializeField] protected GameObject collisionEffectPrefab;
        [SerializeField] protected AudioClip collisionSound;
        #endregion

        #region Unity Lifecycle
        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            SetupPhysics();
        }

        protected virtual void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void FixedUpdate()
        {
            if (usePhysics && limitMovement)
            {
                EnforceMovementLimits();
            }
        }
        #endregion

        #region Physics Setup
        protected virtual void SetupPhysics()
        {
            if (!usePhysics) return;

            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            // 设置物理属性
            rb.mass = mass;
            rb.drag = drag;
            rb.angularDrag = angularDrag;
            rb.useGravity = false; // 通常不需要重力
            rb.constraints = GetConstraints();
        }

        protected virtual RigidbodyConstraints GetConstraints()
        {
            RigidbodyConstraints constraints = RigidbodyConstraints.None;

            if (freezeRotationX)
                constraints |= RigidbodyConstraints.FreezeRotationX;
            if (freezeRotationY)
                constraints |= RigidbodyConstraints.FreezeRotationY;
            if (freezeRotationZ)
                constraints |= RigidbodyConstraints.FreezeRotationZ;

            return constraints;
        }
        #endregion

        #region Movement Limits
        protected virtual void EnforceMovementLimits()
        {
            if (rb == null) return;

            // 限制位置
            Vector3 currentPos = transform.position;
            currentPos.x = Mathf.Clamp(currentPos.x, 
                initialPosition.x - movementBounds.x, 
                initialPosition.x + movementBounds.x);
            currentPos.y = Mathf.Clamp(currentPos.y, 
                initialPosition.y - movementBounds.y, 
                initialPosition.y + movementBounds.y);
            transform.position = currentPos;

            // 限制旋转
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float yRotation = currentRotation.y;
            if (yRotation > 180) yRotation -= 360;
            yRotation = Mathf.Clamp(yRotation, rotationBounds.x, rotationBounds.y);
            transform.rotation = Quaternion.Euler(currentRotation.x, yRotation, currentRotation.z);
        }
        #endregion

        #region Collision Handling
        protected virtual void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 bulletDirection = bullet.GetComponent<Rigidbody>().velocity.normalized;

            if (IsHittingUpperBase(hitPoint, bulletDirection))
            {
                HandleUpperBaseEntry(bullet, hitPoint);
            }
            else if (IsHittingLowerBase(hitPoint, bulletDirection))
            {
                HandleLowerBaseEntry(bullet, hitPoint);
            }
            else if (IsHittingLegs(hitPoint))
            {
                HandleLegCollision(bullet, hitPoint);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            var bullet = other.GetComponent<BaseBullet>();
            if (bullet == null) return;

            Vector3 exitPoint = other.ClosestPoint(transform.position);
            if (IsExitingUpperBase(exitPoint))
            {
                BoostBulletSpeed(bullet);
            }
        }

        protected virtual void HandleUpperBaseEntry(BaseBullet bullet, Vector3 hitPoint)
        {
            // 分裂成两个子弹
            Vector3 leftDirection = CalculateSplitDirection(hitPoint, true);
            Vector3 rightDirection = CalculateSplitDirection(hitPoint, false);

            SpawnSplitBullet(bullet, hitPoint, leftDirection);
            SpawnSplitBullet(bullet, hitPoint, rightDirection);

            // 发布事件
            EventManager.Publish(EventManager.EventNames.SHAPE_COLLISION, 
                new ShapeCollisionEvent(bullet, "SPLIT", hitPoint));

            bullet.DestroyBullet();
            PlayEffects(hitPoint);
        }

        protected virtual void HandleLowerBaseEntry(BaseBullet bullet, Vector3 hitPoint)
        {
            // 从下底进入时的处理逻辑
            // ... 实现下底进入的具体逻辑 ...
        }

        protected virtual void HandleLegCollision(BaseBullet bullet, Vector3 hitPoint)
        {
            // 计算反弹角度
            Vector3 normal = CalculateLegNormal(hitPoint);
            Vector3 velocity = bullet.GetComponent<Rigidbody>().velocity;
            Vector3 reflection = Vector3.Reflect(velocity, normal);

            bullet.GetComponent<Rigidbody>().velocity = reflection;

            // 发布事件
            EventManager.Publish(EventManager.EventNames.SHAPE_COLLISION, 
                new ShapeCollisionEvent(bullet, "BOUNCE", hitPoint));

            PlayEffects(hitPoint);
        }

        protected virtual void BoostBulletSpeed(BaseBullet bullet)
        {
            var rb = bullet.GetComponent<Rigidbody>();
            rb.velocity *= speedBoost;

            // 发布事件
            EventManager.Publish(EventManager.EventNames.SHAPE_COLLISION, 
                new ShapeCollisionEvent(bullet, "BOOST", bullet.transform.position));
        }
        #endregion

        #region Helper Methods
        protected virtual bool IsHittingUpperBase(Vector3 point, Vector3 direction) 
        {
            // 实现上底碰撞检测
            return false;
        }

        protected virtual bool IsHittingLowerBase(Vector3 point, Vector3 direction)
        {
            // 实现下底碰撞检测
            return false;
        }

        protected virtual bool IsHittingLegs(Vector3 point)
        {
            // 实现腰线碰撞检测
            return false;
        }

        protected virtual bool IsExitingUpperBase(Vector3 point)
        {
            // 实现上底出口检测
            return false;
        }

        protected virtual Vector3 CalculateSplitDirection(Vector3 hitPoint, bool isLeft)
        {
            // 计算分裂方向
            return Vector3.zero;
        }

        protected virtual Vector3 CalculateLegNormal(Vector3 hitPoint)
        {
            // 计算腰线法线
            return Vector3.zero;
        }

        protected virtual void SpawnSplitBullet(BaseBullet originalBullet, Vector3 position, Vector3 direction)
        {
            // 生成分裂子弹
        }

        protected virtual void PlayEffects(Vector3 position)
        {
            if (collisionEffectPrefab != null)
            {
                Instantiate(collisionEffectPrefab, position, Quaternion.identity);
            }

            if (collisionSound != null)
            {
                AudioSource.PlayClipAtPoint(collisionSound, position);
            }
        }

        // 重置位置和旋转
        public virtual void ResetTransform()
        {
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        #endregion
    }
}
