using UnityEngine;

namespace Core.Base.Event.Data.Skill
{
    /// <summary>
    /// 技能系统初始化事件
    /// </summary>
    public class SkillSystemInitializedEvent : EventDataBase
    {
        public GameObject Owner { get; private set; }

        public SkillSystemInitializedEvent(GameObject owner)
        {
            Owner = owner;
        }
    }

    /// <summary>
    /// 系统错误事件
    /// </summary>
    public class SystemErrorEvent : EventDataBase
    {
        public string ErrorMessage { get; private set; }

        public SystemErrorEvent(string message)
        {
            ErrorMessage = message;
        }
    }

    /// <summary>
    /// 技能初始化事件
    /// </summary>
    public class SkillsInitializedEvent : EventDataBase
    {
        public BaseSkill[] Skills { get; private set; }

        public SkillsInitializedEvent(BaseSkill[] skills)
        {
            Skills = skills;
        }
    }

    /// <summary>
    /// 技能配置更新事件
    /// </summary>
    public class SkillConfigUpdateEvent : EventDataBase
    {
        public SkillConfig[] Configs { get; private set; }

        public SkillConfigUpdateEvent(SkillConfig[] configs)
        {
            Configs = configs;
        }
    }

    /// <summary>
    /// 技能等级变化事件
    /// </summary>
    public class SkillLevelChangeEvent : EventDataBase
    {
        public int SkillIndex { get; private set; }
        public int Level { get; private set; }

        public SkillLevelChangeEvent(int skillIndex, int level)
        {
            SkillIndex = skillIndex;
            Level = level;
        }
    }

    /// <summary>
    /// 技能测试事件
    /// </summary>
    public class SkillTestEvent : EventDataBase
    {
        public SkillType Type { get; private set; }
        public Vector3 Direction { get; private set; }

        public SkillTestEvent(SkillType type, Vector3 direction)
        {
            Type = type;
            Direction = direction;
        }
    }

    /// <summary>
    /// 技能执行事件
    /// </summary>
    public class SkillExecuteEvent : EventDataBase
    {
        public int SkillIndex { get; private set; }
        public Vector3 Direction { get; private set; }

        public SkillExecuteEvent(int skillIndex, Vector3 direction)
        {
            SkillIndex = skillIndex;
            Direction = direction;
        }
    }

    /// <summary>
    /// 技能释放事件数据
    /// </summary>
    public class SkillCastEventData : EventDataBase
    {
        public int SkillIndex { get; private set; }
        public Vector3 Direction { get; private set; }
        public bool IsStarting { get; private set; }

        public SkillCastEventData(int skillIndex, Vector3 direction, bool isStarting)
        {
            SkillIndex = skillIndex;
            Direction = direction;
            IsStarting = isStarting;
        }
    }

    /// <summary>
    /// 技能冷却事件数据
    /// </summary>
    public class SkillCooldownEventData : EventDataBase
    {
        public string SkillName { get; private set; }
        public GameObject Owner { get; private set; }
        public float CurrentCooldown { get; private set; }
        public float TotalCooldown { get; private set; }

        public SkillCooldownEventData(string skillName, GameObject owner, float currentCooldown, float totalCooldown)
        {
            SkillName = skillName;
            Owner = owner;
            CurrentCooldown = currentCooldown;
            TotalCooldown = totalCooldown;
        }
    }
} 