using UnityEngine;
using Core.Combat.Unit.Base;

namespace Core.Skills.Base
{
    /// <summary>
    /// Base implementation for skills
    /// </summary>
    public abstract class BaseSkill : MonoBehaviour, ISkill
    {
        [SerializeField] protected string skillName;
        [SerializeField] protected float cooldownTime;
        
        protected float currentCooldown;
        protected BaseUnit owner;

        public string SkillName => skillName;
        public float CooldownTime => cooldownTime;
        public bool IsReady => currentCooldown <= 0;

        protected virtual void Awake()
        {
            owner = GetComponent<BaseUnit>();
            currentCooldown = 0;
        }

        public abstract void Execute(Vector3 direction);

        public virtual void UpdateCooldown(float deltaTime)
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= deltaTime;
            }
        }
    }
}
