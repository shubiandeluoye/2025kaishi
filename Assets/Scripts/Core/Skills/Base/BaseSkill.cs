using UnityEngine;

namespace Core.Skills.Base
{
    public abstract class BaseSkill : MonoBehaviour, ISkill
    {
        [Header("Skill Settings")]
        [SerializeField] protected float cooldownDuration = 1f;
        protected float currentCooldown;

        public bool IsReady => currentCooldown <= 0;

        protected virtual void Awake()
        {
            currentCooldown = 0;
        }

        public abstract void Execute(Vector3 direction);

        public virtual void UpdateCooldown(float deltaTime)
        {
            if (currentCooldown > 0)
            {
                currentCooldown = Mathf.Max(0, currentCooldown - deltaTime);
            }
        }

        protected virtual void StartCooldown()
        {
            currentCooldown = cooldownDuration;
        }
    }
}
