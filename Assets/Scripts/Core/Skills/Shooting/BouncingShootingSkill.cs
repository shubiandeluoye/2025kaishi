using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;
using Core.Combat.Bullet.Base;

namespace Core.Skills.Shooting
{
    /// <summary>
    /// Implements a bouncing projectile shooting skill
    /// </summary>
    public class BouncingShootingSkill : BaseSkill
    {
        [SerializeField] private BaseShooter shooterComponent;
        [SerializeField] private int maxBounces = 3;
        [SerializeField] private float bounceForce = 0.8f;
        
        protected override void Awake()
        {
            base.Awake();
            shooterComponent = GetComponent<BaseShooter>();
        }

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || shooterComponent == null) return;

            // 先射击，获取实例化的子弹
            shooterComponent.Shoot(direction);
            
            // 在实例化的子弹上添加弹跳组件
            var bullet = shooterComponent.GetLastFiredBullet();
            if (bullet != null)
            {
                var bulletRb = bullet.GetComponent<Rigidbody>();
                if (bulletRb != null)
                {
                    bulletRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    var bounceHandler = bullet.AddComponent<BulletBounceHandler>();
                    bounceHandler.Initialize(maxBounces, bounceForce);
                }
            }

            currentCooldown = cooldownDuration;
        }
    }

    /// <summary>
    /// Helper component to handle bullet bouncing behavior
    /// </summary>
    public class BulletBounceHandler : MonoBehaviour
    {
        private int remainingBounces;
        private float bounceForce;

        public void Initialize(int maxBounces, float force)
        {
            remainingBounces = maxBounces;
            bounceForce = force;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (remainingBounces <= 0) return;

            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 reflection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
                rb.velocity = reflection * (rb.velocity.magnitude * bounceForce);
                remainingBounces--;
            }
        }
    }
}
