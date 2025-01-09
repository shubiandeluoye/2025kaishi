using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

namespace Core.Skills.Shooting
{
    /// <summary>
    /// Implements a spread shooting pattern as a skill
    /// </summary>
    public class SpreadShootingSkill : BaseSkill
    {
        [SerializeField] private BaseShooter shooterComponent;
        [SerializeField] private int projectileCount = 3;
        [SerializeField] private float spreadAngle = 30f;
        [SerializeField] private bool randomizeSpread = false;
        
        protected override void Awake()
        {
            base.Awake();
            shooterComponent = GetComponent<BaseShooter>();
        }

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || shooterComponent == null) return;

            float angleStep = spreadAngle / (projectileCount - 1);
            float startAngle = -spreadAngle / 2;

            for (int i = 0; i < projectileCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                if (randomizeSpread)
                {
                    currentAngle += Random.Range(-angleStep/4, angleStep/4);
                }
                
                Vector3 rotatedDirection = Quaternion.Euler(0, currentAngle, 0) * direction;
                shooterComponent.Shoot(rotatedDirection);
            }

            currentCooldown = cooldownDuration;
        }
    }
}
