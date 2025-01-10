using UnityEngine;
using Core.Base.Event;

namespace Core.Skills.Base
{
    public abstract class BaseSkill : MonoBehaviour, ISkill
    {
        [Header("Skill Settings")]
        [SerializeField] protected string skillName = "Unnamed Skill";
        [SerializeField] protected float cooldownDuration = 1f;
        protected float currentCooldown;

        public string SkillName => skillName;
        public bool IsReady => currentCooldown <= 0;
        public float CooldownDuration => cooldownDuration;
        public float CurrentCooldown => currentCooldown;

        protected virtual void Awake()
        {
            currentCooldown = 0;
        }

        public virtual void Execute(Vector3 direction)
        {
            if (!IsReady)
            {
                EventManager.Publish(EventNames.SKILL_FAILED, 
                    new SkillEventData(skillName, gameObject, direction, currentCooldown));
                return;
            }

            OnSkillExecute(direction);
            StartCooldown();

            EventManager.Publish(EventNames.SKILL_EXECUTED, 
                new SkillEventData(skillName, gameObject, direction, cooldownDuration));
        }

        protected abstract void OnSkillExecute(Vector3 direction);

        public virtual void UpdateCooldown(float deltaTime)
        {
            if (currentCooldown > 0)
            {
                float previousCooldown = currentCooldown;
                currentCooldown = Mathf.Max(0, currentCooldown - deltaTime);

                // 发布冷却进度事件
                EventManager.Publish(EventNames.SKILL_COOLDOWN_START, 
                    new SkillCooldownEventData(skillName, gameObject, currentCooldown, cooldownDuration));

                // 如果冷却刚结束
                if (previousCooldown > 0 && currentCooldown <= 0)
                {
                    EventManager.Publish(EventNames.SKILL_COOLDOWN_END, 
                        new SkillCooldownEventData(skillName, gameObject, 0, cooldownDuration));
                    
                    EventManager.Publish(EventNames.SKILL_READY, 
                        new SkillEventData(skillName, gameObject, Vector3.zero, 0));
                }
            }
        }

        protected virtual void StartCooldown()
        {
            currentCooldown = cooldownDuration;
            EventManager.Publish(EventNames.SKILL_COOLDOWN_START, 
                new SkillCooldownEventData(skillName, gameObject, currentCooldown, cooldownDuration));
        }
    }
}
