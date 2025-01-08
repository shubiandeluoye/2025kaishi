using UnityEngine;
using System.Collections.Generic;
using Core.Skills.Base;

namespace Core.Skills.Shooting
{
    /// <summary>
    /// Manages skills for a game entity
    /// </summary>
    public class SkillManager : MonoBehaviour
    {
        private List<ISkill> skills = new List<ISkill>();

        private void Awake()
        {
            // Automatically collect all skill components
            skills.AddRange(GetComponents<ISkill>());
        }

        private void Update()
        {
            // Update cooldowns for all skills
            foreach (var skill in skills)
            {
                skill.UpdateCooldown(Time.deltaTime);
            }
        }

        public void ExecuteSkill(int skillIndex, Vector3 direction)
        {
            if (skillIndex >= 0 && skillIndex < skills.Count)
            {
                skills[skillIndex].Execute(direction);
            }
        }

        public bool IsSkillReady(int skillIndex)
        {
            return skillIndex >= 0 && skillIndex < skills.Count && skills[skillIndex].IsReady;
        }
    }
}
