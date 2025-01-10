using UnityEngine;

namespace Core.Base.Event.Data.Time
{
    /// <summary>
    /// 时间缩放事件数据
    /// </summary>
    public class TimeScaleEventData : EventDataBase
    {
        /// <summary>
        /// 时间缩放值
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        /// 持续时间（0表示永久）
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 缩放类型
        /// </summary>
        public TimeScaleType Type { get; private set; }

        /// <summary>
        /// 缩放优先级
        /// </summary>
        public int Priority { get; private set; }

        public TimeScaleEventData(float scale, float duration = 0, 
            TimeScaleType type = TimeScaleType.Normal, int priority = 0)
        {
            Scale = scale;
            Duration = duration;
            Type = type;
            Priority = priority;
        }
    }

    /// <summary>
    /// 时间状态事件数据
    /// </summary>
    public class TimeStateEventData : EventDataBase
    {
        /// <summary>
        /// 新状态
        /// </summary>
        public TimeState NewState { get; private set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public TimeState OldState { get; private set; }

        /// <summary>
        /// 状态改变原因
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// 状态持续时间
        /// </summary>
        public float Duration { get; private set; }

        public TimeStateEventData(TimeState newState, TimeState oldState, 
            string reason = "", float duration = 0)
        {
            NewState = newState;
            OldState = oldState;
            Reason = reason;
            Duration = duration;
        }
    }

    /// <summary>
    /// 时间缩放类型枚举
    /// </summary>
    public enum TimeScaleType
    {
        /// <summary>
        /// 普通缩放
        /// </summary>
        Normal,

        /// <summary>
        /// 技能时间缩放
        /// </summary>
        Skill,

        /// <summary>
        /// 特效时间缩放
        /// </summary>
        Effect,

        /// <summary>
        /// 系统时间缩放
        /// </summary>
        System
    }

    /// <summary>
    /// 时间状态枚举
    /// </summary>
    public enum TimeState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 暂停
        /// </summary>
        Paused,

        /// <summary>
        /// 加速
        /// </summary>
        Accelerated,

        /// <summary>
        /// 减速
        /// </summary>
        Slowed
    }
} 