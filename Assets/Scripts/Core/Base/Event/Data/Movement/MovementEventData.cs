using UnityEngine;

namespace Core.Base.Event.Data.Movement
{
    /// <summary>
    /// 移动事件数据基类
    /// </summary>
    public class MovementEventData : EventDataBase
    {
        /// <summary>
        /// 移动对象
        /// </summary>
        public GameObject Mover { get; private set; }

        /// <summary>
        /// 起始位置
        /// </summary>
        public Vector3 StartPosition { get; private set; }

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 TargetPosition { get; private set; }

        /// <summary>
        /// 移动速度
        /// </summary>
        public float Speed { get; private set; }

        public MovementEventData(GameObject mover, Vector3 startPosition, 
            Vector3 targetPosition, float speed)
        {
            Mover = mover;
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            Speed = speed;
        }
    }

    /// <summary>
    /// 移动状态事件数据
    /// </summary>
    public class MovementStateEventData : EventDataBase
    {
        /// <summary>
        /// 移动对象
        /// </summary>
        public GameObject Mover { get; private set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public MovementState NewState { get; private set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public MovementState OldState { get; private set; }

        /// <summary>
        /// 状态变化原因
        /// </summary>
        public string Reason { get; private set; }

        public MovementStateEventData(GameObject mover, MovementState newState, 
            MovementState oldState, string reason = "")
        {
            Mover = mover;
            NewState = newState;
            OldState = oldState;
            Reason = reason;
        }
    }

    /// <summary>
    /// 移动速度事件数据
    /// </summary>
    public class MovementSpeedEventData : EventDataBase
    {
        /// <summary>
        /// 移动对象
        /// </summary>
        public GameObject Mover { get; private set; }

        /// <summary>
        /// 新速度
        /// </summary>
        public float NewSpeed { get; private set; }

        /// <summary>
        /// 旧速度
        /// </summary>
        public float OldSpeed { get; private set; }

        /// <summary>
        /// 速度修改类型
        /// </summary>
        public SpeedModificationType ModificationType { get; private set; }

        public MovementSpeedEventData(GameObject mover, float newSpeed, float oldSpeed, 
            SpeedModificationType modificationType)
        {
            Mover = mover;
            NewSpeed = newSpeed;
            OldSpeed = oldSpeed;
            ModificationType = modificationType;
        }
    }

    /// <summary>
    /// 移动状态枚举
    /// </summary>
    public enum MovementState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle,

        /// <summary>
        /// 移动中
        /// </summary>
        Moving,

        /// <summary>
        /// 跳跃中
        /// </summary>
        Jumping,

        /// <summary>
        /// 冲刺中
        /// </summary>
        Dashing,

        /// <summary>
        /// 被击退
        /// </summary>
        Knockback
    }

    /// <summary>
    /// 速度修改类型枚举
    /// </summary>
    public enum SpeedModificationType
    {
        /// <summary>
        /// 加速
        /// </summary>
        Acceleration,

        /// <summary>
        /// 减速
        /// </summary>
        Deceleration,

        /// <summary>
        /// 固定速度
        /// </summary>
        Fixed,

        /// <summary>
        /// 倍率修改
        /// </summary>
        Multiplier
    }
} 