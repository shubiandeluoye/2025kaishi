using UnityEngine;
using System.Collections.Generic;
using Core.Combat.Unit.Base;
using Core.Skills.Base;
using Core.Skills.Shooting;

namespace Core.Skills
{
    /// <summary>
    /// Manages skills for a game entity
    /// </summary>
    public class SkillManager : MonoBehaviour
    {
        private BaseShooter shooter;
        private List<BaseSkill> skills = new List<BaseSkill>();

        private void Awake()
        {
            shooter = GetComponent<BaseShooter>();
            if (shooter == null)
            {
                Debug.LogError("SkillManager requires a BaseShooter component!");
                return;
            }

            // 收集所有技能
            skills.AddRange(GetComponents<BaseSkill>());
        }

        /// <summary>
        /// 检查指定索引的技能是否可用
        /// </summary>
        /// <param name="skillIndex">技能索引</param>
        /// <returns>技能是否可用</returns>
        public bool IsSkillReady(int skillIndex)
        {
            if (skillIndex < 0 || skillIndex >= skills.Count)
            {
                Debug.LogWarning($"Invalid skill index: {skillIndex}");
                return false;
            }

            return skills[skillIndex].IsReady;
        }

        /// <summary>
        /// 执行指定索引的技能
        /// </summary>
        public void ExecuteSkill(int skillIndex, Vector3 direction)
        {
            if (skillIndex < 0 || skillIndex >= skills.Count)
            {
                Debug.LogWarning($"Invalid skill index: {skillIndex}");
                return;
            }

            var skill = skills[skillIndex];
            if (skill.IsReady)
            {
                skill.Execute(direction);
            }
        }

        /// <summary>
        /// 获取所有技能列表
        /// </summary>
        public IReadOnlyList<BaseSkill> GetSkills()
        {
            return skills;
        }

        /// <summary>
        /// 添加新技能
        /// </summary>
        public void AddSkill(BaseSkill skill)
        {
            if (!skills.Contains(skill))
            {
                skills.Add(skill);
            }
        }

        /// <summary>
        /// 移除技能
        /// </summary>
        public void RemoveSkill(BaseSkill skill)
        {
            skills.Remove(skill);
        }

        /// <summary>
        /// 更新所有技能的冷却时间
        /// </summary>
        private void Update()
        {
            foreach (var skill in skills)
            {
                skill.UpdateCooldown(Time.deltaTime);
            }
        }
    }
}
