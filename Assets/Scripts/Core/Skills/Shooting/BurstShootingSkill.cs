using UnityEngine;
using System.Collections;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

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

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || shooterComponent == null) return;
            
            if (burstCoroutine != null)
            {
                StopCoroutine(burstCoroutine);
            }
            
            burstCoroutine = StartCoroutine(ExecuteBurst(direction));
            currentCooldown = cooldownTime;
        }

        private IEnumerator ExecuteBurst(Vector3 direction)
        {
            for (int i = 0; i < burstCount; i++)
            {
                shooterComponent.Shoot(direction);
                yield return new WaitForSeconds(burstDelay);
            }
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
