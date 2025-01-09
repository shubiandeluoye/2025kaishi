using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

namespace Core.Skills.Shooting
{
    /// <summary>
    /// Implements directional shooting as a skill
    /// </summary>
    public class DirectionalShootingSkill : BaseSkill
    {
        [SerializeField] private BaseShooter shooterComponent;
        [SerializeField] private float spreadAngle = 0f;
        [SerializeField] private int projectileCount = 1;

        protected override void Awake()
        {
            base.Awake();
            shooterComponent = GetComponent<BaseShooter>();
        }

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || shooterComponent == null) return;

            if (projectileCount == 1)
            {
                shooterComponent.Shoot(direction);
            }
            else
            {
                float angleStep = spreadAngle / (projectileCount - 1);
                float startAngle = -spreadAngle / 2;

                for (int i = 0; i < projectileCount; i++)
                {
                    float currentAngle = startAngle + (angleStep * i);
                    Vector3 rotatedDirection = Quaternion.Euler(0, currentAngle, 0) * direction;
                    shooterComponent.Shoot(rotatedDirection);
                }
            }

            currentCooldown = cooldownDuration;
        }
    }
}
