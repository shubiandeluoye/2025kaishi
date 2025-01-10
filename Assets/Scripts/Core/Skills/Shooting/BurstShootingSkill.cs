using UnityEngine;
using System.Collections;
using Core.Skills.Base;
using Core.Combat.Unit.Base;
using Core.Base.Event;

namespace Core.Skills.Shooting
{
    /// <summary>
    /// Implements burst firing as a skill
    /// </summary>
    public class BurstShootingSkill : BaseSkill
    {
        [SerializeField] private BaseShooter shooterComponent;
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstDelay = 0.1f;
        
        private Coroutine burstCoroutine;
        
        protected override void Awake()
        {
            base.Awake();
            shooterComponent = GetComponent<BaseShooter>();
        }

        protected override void OnSkillExecute(Vector3 direction)
        {
            if (shooterComponent == null) return;
            
            if (burstCoroutine != null)
            {
                StopCoroutine(burstCoroutine);
            }
            
            burstCoroutine = StartCoroutine(ExecuteBurst(direction));
        }

        private IEnumerator ExecuteBurst(Vector3 direction)
        {
            EventManager.Publish(EventNames.BURST_SHOT_START, 
                new BurstShotEventData(burstCount, burstDelay, direction));

            for (int i = 0; i < burstCount; i++)
            {
                shooterComponent.Shoot(direction);
                yield return new WaitForSeconds(burstDelay);
            }

            EventManager.Publish(EventNames.BURST_SHOT_END, 
                new BurstShotEventData(burstCount, burstDelay, direction));
        }

        private void OnDisable()
        {
            if (burstCoroutine != null)
            {
                StopCoroutine(burstCoroutine);
                burstCoroutine = null;
            }
        }
    }
}
