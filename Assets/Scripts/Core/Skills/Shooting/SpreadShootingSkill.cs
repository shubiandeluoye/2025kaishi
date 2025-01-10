using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;
using Core.Base.Event;

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

        protected override void OnSkillExecute(Vector3 direction)
        {
            if (shooterComponent == null) return;

            Vector3[] directions = new Vector3[projectileCount];
            float angleStep = spreadAngle / (projectileCount - 1);
            float startAngle = -spreadAngle / 2;

            for (int i = 0; i < projectileCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                if (randomizeSpread)
                {
                    currentAngle += Random.Range(-angleStep/4, angleStep/4);
                }
                
                directions[i] = Quaternion.Euler(0, currentAngle, 0) * direction;
                shooterComponent.Shoot(directions[i]);
            }

            EventManager.Publish(EventNames.SPREAD_SHOT_FIRED, 
                new SpreadShotEventData(projectileCount, spreadAngle, randomizeSpread, directions));
        }
    }
}
