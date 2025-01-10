using UnityEngine;
using System;
using Core.Base.Event;
using Core.Level.Data;
using Core.Level.Events;
using Core.Level.Interface;

namespace Core.Level.Base
{
    public class BaseLevelSystem : MonoBehaviour, ILevelable
    {
        [Serializable]
        public class LevelSettings
        {
            public int startLevel = 1;
            public int maxLevel = 10;
            public bool allowPrestige = false;
            public int prestigeLevel = 0;
            public LevelData.LevelRequirement[] requirements;
        }

        [SerializeField] protected LevelSettings settings = new LevelSettings();
        
        protected int currentLevel;
        protected float currentExperience;
        protected int currentPrestige;
        protected LevelData levelData;

        public event Action<int> OnLevelUp;
        public event Action<int> OnPrestige;
        public event Action<float> OnExperienceGained;

        #region Properties
        public virtual int Level 
        { 
            get => currentLevel;
            protected set
            {
                int oldLevel = currentLevel;
                currentLevel = Mathf.Clamp(value, 1, settings.maxLevel);
                if (currentLevel != oldLevel)
                {
                    OnLevelUp?.Invoke(currentLevel);
                    EventManager.Publish(EventNames.LEVEL_UP, 
                        new LevelUpEvent(this, currentLevel));
                }
            }
        }

        public virtual int MaxLevel => settings.maxLevel;
        public virtual int PrestigeLevel => currentPrestige;
        public virtual float Experience => currentExperience;
        public virtual float ExperienceToNextLevel => GetExperienceRequirement(Level);
        #endregion

        #region Initialization
        protected virtual void Awake()
        {
            InitializeLevelSystem();
        }

        protected virtual void InitializeLevelSystem()
        {
            currentLevel = settings.startLevel;
            levelData = new LevelData(settings);
        }
        #endregion

        #region Level Management
        public virtual bool CanLevelUp()
        {
            return currentLevel < MaxLevel && HasMetRequirements();
        }

        public virtual bool TryLevelUp()
        {
            if (CanLevelUp())
            {
                Level++;
                ResetExperience();
                return true;
            }
            return false;
        }

        public virtual void AddExperience(float amount)
        {
            currentExperience += amount;
            OnExperienceGained?.Invoke(amount);
            EventManager.Publish(EventNames.EXPERIENCE_GAINED, 
                new ExperienceGainEvent(this, amount));

            while (currentExperience >= ExperienceToNextLevel && CanLevelUp())
            {
                currentExperience -= ExperienceToNextLevel;
                TryLevelUp();
            }
        }
        #endregion

        #region Prestige System
        public virtual bool CanPrestige()
        {
            return settings.allowPrestige && Level >= MaxLevel;
        }

        public virtual bool TryPrestige()
        {
            if (CanPrestige())
            {
                currentPrestige++;
                ResetLevel();
                OnPrestige?.Invoke(currentPrestige);
                EventManager.Publish(EventNames.PRESTIGE_LEVEL_UP, 
                    new PrestigeEvent(this, currentPrestige));
                return true;
            }
            return false;
        }
        #endregion

        #region Requirements
        protected virtual bool HasMetRequirements()
        {
            if (settings.requirements == null) return true;
            return levelData.CheckRequirements(Level);
        }

        protected virtual float GetExperienceRequirement(int level)
        {
            return levelData.GetExperienceRequirement(level);
        }
        #endregion

        #region Utility
        protected virtual void ResetLevel()
        {
            Level = settings.startLevel;
            ResetExperience();
        }

        protected virtual void ResetExperience()
        {
            currentExperience = 0;
        }
        #endregion

        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnregisterEvents();
        }

        protected virtual void RegisterEvents()
        {
            // 如果需要监听其他事件，在这里注册
        }

        protected virtual void UnregisterEvents()
        {
            // 如果需要取消监听其他事件，在这里取消注册
        }
    }
} 