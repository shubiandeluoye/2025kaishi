using UnityEngine;
using System;
using Core.Level.Data;
using Core.Level.Interface;

namespace Core.Level.Events
{
    public static class LevelEvents
    {
        // 全局等级事件
        public static event Action<ILevelable, int> OnAnyLevelUp;
        public static event Action<ILevelable, int> OnAnyPrestige;
        public static event Action<ILevelable, float> OnAnyExperienceGained;
        
        // 特定类型的等级事件
        public static event Action<ILevelable, LevelData.UnlockableContent[]> OnContentUnlocked;
        public static event Action<ILevelable, LevelData.LevelRequirement> OnRequirementsMet;
        public static event Action<ILevelable, string> OnLevelError;

        public static void TriggerLevelUp(ILevelable source, int newLevel)
        {
            try
            {
                OnAnyLevelUp?.Invoke(source, newLevel);
                Debug.Log($"Entity leveled up to {newLevel}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in level up event: {e.Message}");
                OnLevelError?.Invoke(source, e.Message);
            }
        }

        public static void TriggerPrestige(ILevelable source, int prestigeLevel)
        {
            try
            {
                OnAnyPrestige?.Invoke(source, prestigeLevel);
                Debug.Log($"Entity reached prestige level {prestigeLevel}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in prestige event: {e.Message}");
                OnLevelError?.Invoke(source, e.Message);
            }
        }

        public static void TriggerExperienceGained(ILevelable source, float amount)
        {
            try
            {
                OnAnyExperienceGained?.Invoke(source, amount);
                Debug.Log($"Entity gained {amount} experience");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in experience gain event: {e.Message}");
                OnLevelError?.Invoke(source, e.Message);
            }
        }

        public static void TriggerContentUnlocked(ILevelable source, LevelData.UnlockableContent[] content)
        {
            try
            {
                OnContentUnlocked?.Invoke(source, content);
                foreach (var item in content)
                {
                    Debug.Log($"Unlocked content: {item.contentType} - {item.contentId}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in content unlock event: {e.Message}");
                OnLevelError?.Invoke(source, e.Message);
            }
        }

        public static void TriggerRequirementsMet(ILevelable source, LevelData.LevelRequirement requirement)
        {
            try
            {
                OnRequirementsMet?.Invoke(source, requirement);
                Debug.Log($"Level requirements met for level {requirement.level}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in requirements met event: {e.Message}");
                OnLevelError?.Invoke(source, e.Message);
            }
        }
    }
} 