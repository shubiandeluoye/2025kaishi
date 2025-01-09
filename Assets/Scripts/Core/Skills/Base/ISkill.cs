using UnityEngine;

namespace Core.Skills.Base
{
    public interface ISkill
    {
        bool IsReady { get; }
        void Execute(Vector3 direction);
        void UpdateCooldown(float deltaTime);
    }
}
