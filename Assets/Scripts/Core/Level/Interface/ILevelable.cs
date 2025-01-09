using UnityEngine;
using System;
using Core.Level.Data;
using Core.Level.Events;

namespace Core.Level.Interface
{
    public interface ILevelable
    {
        int Level { get; }
        int MaxLevel { get; }
        float Experience { get; }
        float ExperienceToNextLevel { get; }
        int PrestigeLevel { get; }

        bool CanLevelUp();
        bool TryLevelUp();
        void AddExperience(float amount);
        bool CanPrestige();
        bool TryPrestige();
        
        event Action<int> OnLevelUp;
        event Action<int> OnPrestige;
        event Action<float> OnExperienceGained;
    }
} 