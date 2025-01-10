using UnityEngine;

namespace Core.Skills.Base
{
    public interface ISkill
    {
        string SkillName { get; }
        bool IsReady { get; }
        float CooldownDuration { get; }
        float CurrentCooldown { get; }
        void Execute(Vector3 direction);
        void UpdateCooldown(float deltaTime);
    }
}
