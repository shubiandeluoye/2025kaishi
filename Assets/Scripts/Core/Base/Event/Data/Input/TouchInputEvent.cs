using UnityEngine;

namespace Core.Base.Event.Data.Input
{
    /// <summary>
    /// 触摸输入事件数据
    /// </summary>
    public class TouchInputEvent : EventDataBase
    {
        /// <summary>
        /// 触摸类型
        /// </summary>
        public TouchEventType Type { get; private set; }

        /// <summary>
        /// 触摸位置
        /// </summary>
        public Vector2 Position { get; private set; }

        public TouchInputEvent(TouchEventType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }

    /// <summary>
    /// 触摸事件类型枚举
    /// </summary>
    public enum TouchEventType
    {
        Move,           // 移动
        Attack,         // 攻击
        SkillAim,       // 技能瞄准
        SkillTrigger    // 技能触发
    }
} 