using UnityEngine;

namespace Core.Skills.Base
{
    /// <summary>
    /// Base interface for all skills in the game
    /// </summary>
    public interface ISkill
    {
        string SkillName { get; }
        float CooldownTime { get; }
        bool IsReady { get; }
        void Execute(Vector3 direction);
        void UpdateCooldown(float deltaTime);
    }
}
