using UnityEngine;
using System;
using System.Collections.Generic;
using Core.Level.Base;

namespace Core.Level.Data
{
    [Serializable]
    public class LevelData
    {
        #region Level Requirements
        [Serializable]
        public class LevelRequirement
        {
            public int level;
            public float experienceRequired;
            public ResourceRequirement[] resourceRequirements;
            public ConditionRequirement[] conditions;
            public UnlockableContent[] unlocks;
        }

        [Serializable]
        public class ResourceRequirement
        {
            public string resourceType;
            public float amount;
            public bool consumeOnLevelUp = true;
        }

        [Serializable]
        public class ConditionRequirement
        {
            public string conditionType;
            public string[] parameters;
            public bool isPermanent;
        }

        [Serializable]
        public class UnlockableContent
        {
            public string contentType;
            public string contentId;
            public float unlockValue;
            public bool isTemporary;
        }
        #endregion

        #region Properties
        private Dictionary<int, LevelRequirement> levelRequirements;
        private BaseLevelSystem.LevelSettings settings;
        private float baseExperience = 100f;
        private float experienceMultiplier = 1.5f;
        #endregion

        #region Initialization
        public LevelData(BaseLevelSystem.LevelSettings settings)
        {
            this.settings = settings;
            InitializeLevelData();
        }

        private void InitializeLevelData()
        {
            levelRequirements = new Dictionary<int, LevelRequirement>();
            if (settings.requirements != null)
            {
                foreach (var req in settings.requirements)
                {
                    levelRequirements[req.level] = req;
                }
            }
        }
        #endregion

        #region Level Management
        public float GetExperienceRequirement(int level)
        {
            if (levelRequirements.TryGetValue(level, out var requirement))
            {
                return requirement.experienceRequired;
            }
            
            // 默认经验计算公式
            return baseExperience * Mathf.Pow(experienceMultiplier, level - 1);
        }

        public bool CheckRequirements(int level)
        {
            if (!levelRequirements.TryGetValue(level, out var requirement))
            {
                return true; // 如果没有特定要求，默认允许
            }

            return CheckResourceRequirements(requirement.resourceRequirements) 
                && CheckConditions(requirement.conditions);
        }
        #endregion

        #region Requirement Checking
        private bool CheckResourceRequirements(ResourceRequirement[] requirements)
        {
            if (requirements == null || requirements.Length == 0)
                return true;

            // TODO: 实现资源检查逻辑
            return true;
        }

        private bool CheckConditions(ConditionRequirement[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
                return true;

            // TODO: 实现条件检查逻辑
            return true;
        }
        #endregion

        #region Unlockable Content
        public UnlockableContent[] GetUnlockableContent(int level)
        {
            if (levelRequirements.TryGetValue(level, out var requirement))
            {
                return requirement.unlocks;
            }
            return null;
        }
        #endregion

        #region Data Access
        public LevelRequirement GetLevelRequirement(int level)
        {
            levelRequirements.TryGetValue(level, out var requirement);
            return requirement;
        }

        public Dictionary<int, LevelRequirement> GetAllRequirements()
        {
            return new Dictionary<int, LevelRequirement>(levelRequirements);
        }
        #endregion
    }
} 