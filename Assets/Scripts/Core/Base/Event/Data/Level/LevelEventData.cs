using UnityEngine;

namespace Core.Base.Event.Data.Level
{
    /// <summary>
    /// 等级提升事件数据
    /// </summary>
    public class LevelUpEventData : EventDataBase
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        public GameObject Target { get; private set; }

        /// <summary>
        /// 新等级
        /// </summary>
        public int NewLevel { get; private set; }

        /// <summary>
        /// 旧等级
        /// </summary>
        public int OldLevel { get; private set; }

        /// <summary>
        /// 升级类型
        /// </summary>
        public LevelUpType Type { get; private set; }

        public LevelUpEventData(GameObject target, int newLevel, int oldLevel, LevelUpType type)
        {
            Target = target;
            NewLevel = newLevel;
            OldLevel = oldLevel;
            Type = type;
        }
    }

    /// <summary>
    /// 经验值事件数据
    /// </summary>
    public class ExpEventData : EventDataBase
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        public GameObject Target { get; private set; }

        /// <summary>
        /// 获得的经验值
        /// </summary>
        public float ExpGained { get; private set; }

        /// <summary>
        /// 当前总经验值
        /// </summary>
        public float CurrentExp { get; private set; }

        /// <summary>
        /// 下一级所需经验值
        /// </summary>
        public float NextLevelExp { get; private set; }

        /// <summary>
        /// 经验值来源
        /// </summary>
        public ExpSource Source { get; private set; }

        public ExpEventData(GameObject target, float expGained, float currentExp, 
            float nextLevelExp, ExpSource source)
        {
            Target = target;
            ExpGained = expGained;
            CurrentExp = currentExp;
            NextLevelExp = nextLevelExp;
            Source = source;
        }
    }

    /// <summary>
    /// 等级奖励事件数据
    /// </summary>
    public class LevelRewardEventData : EventDataBase
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        public GameObject Target { get; private set; }

        /// <summary>
        /// 当前等级
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        public RewardType RewardType { get; private set; }

        /// <summary>
        /// 奖励数值
        /// </summary>
        public float RewardValue { get; private set; }

        public LevelRewardEventData(GameObject target, int level, 
            RewardType rewardType, float rewardValue)
        {
            Target = target;
            Level = level;
            RewardType = rewardType;
            RewardValue = rewardValue;
        }
    }

    /// <summary>
    /// 升级类型枚举
    /// </summary>
    public enum LevelUpType
    {
        /// <summary>
        /// 普通升级
        /// </summary>
        Normal,

        /// <summary>
        /// 突破升级
        /// </summary>
        Breakthrough,

        /// <summary>
        /// 系统升级
        /// </summary>
        System
    }

    /// <summary>
    /// 经验值来源枚举
    /// </summary>
    public enum ExpSource
    {
        /// <summary>
        /// 击败敌人
        /// </summary>
        Combat,

        /// <summary>
        /// 完成任务
        /// </summary>
        Quest,

        /// <summary>
        /// 收集物品
        /// </summary>
        Collection,

        /// <summary>
        /// 系统奖励
        /// </summary>
        System
    }

    /// <summary>
    /// 奖励类型枚举
    /// </summary>
    public enum RewardType
    {
        /// <summary>
        /// 属性提升
        /// </summary>
        Attribute,

        /// <summary>
        /// 技能解锁
        /// </summary>
        Skill,

        /// <summary>
        /// 物品奖励
        /// </summary>
        Item,

        /// <summary>
        /// 特殊能力
        /// </summary>
        Ability
    }
} 