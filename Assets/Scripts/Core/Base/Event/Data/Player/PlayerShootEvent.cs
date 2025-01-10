using UnityEngine;

namespace Core.Base.Event.Data.Player
{
    /// <summary>
    /// 玩家射击事件数据
    /// </summary>
    public class PlayerShootEvent : EventDataBase
    {
        /// <summary>
        /// 射击类型
        /// </summary>
        public ShootType Type { get; private set; }

        /// <summary>
        /// 射击角度
        /// </summary>
        public float Angle { get; private set; }

        public PlayerShootEvent(ShootType type, float angle = 0)
        {
            Type = type;
            Angle = angle;
        }
    }

    /// <summary>
    /// 射击类型枚举
    /// </summary>
    public enum ShootType
    {
        Straight,       // 直射
        LeftAngle,      // 左倾角度
        RightAngle,     // 右倾角度
        Level3          // 三级射击
    }
} 