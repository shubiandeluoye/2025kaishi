using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;
using Core.Base.Event;

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

        protected override void OnSkillExecute(Vector3 direction)
        {
            if (shooterComponent == null) return;

            Vector3[] directions = new Vector3[projectileCount];
            
            if (projectileCount == 1)
            {
                directions[0] = direction;
                shooterComponent.Shoot(direction);
            }
            else
            {
                float angleStep = spreadAngle / (projectileCount - 1);
                float startAngle = -spreadAngle / 2;

                for (int i = 0; i < projectileCount; i++)
                {
                    float currentAngle = startAngle + (angleStep * i);
                    directions[i] = Quaternion.Euler(0, currentAngle, 0) * direction;
                    shooterComponent.Shoot(directions[i]);
                }
            }

            EventManager.Publish(EventNames.DIRECTIONAL_SHOT_FIRED, 
                new SpreadShotEventData(projectileCount, spreadAngle, false, directions));
        }
    }
}
