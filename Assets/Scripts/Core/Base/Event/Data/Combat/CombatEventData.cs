using UnityEngine;

namespace Core.Base.Event.Data.Combat
{
    /// <summary>
    /// 生命值变化事件数据
    /// </summary>
    public class HealthChangeEventData : EventDataBase
    {
        /// <summary>
        /// 变化前的生命值
        /// </summary>
        public float PreviousHealth { get; private set; }

        /// <summary>
        /// 当前生命值
        /// </summary>
        public float CurrentHealth { get; private set; }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public float MaxHealth { get; private set; }

        /// <summary>
        /// 变化值
        /// </summary>
        public float Delta { get; private set; }

        /// <summary>
        /// 伤害来源
        /// </summary>
        public GameObject Source { get; private set; }

        public HealthChangeEventData(float previousHealth, float currentHealth, float maxHealth, 
            float delta, GameObject source = null)
        {
            PreviousHealth = previousHealth;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Delta = delta;
            Source = source;
        }
    }

    /// <summary>
    /// 射击点更新事件数据
    /// </summary>
    public class ShootPointEventData : EventDataBase
    {
        /// <summary>
        /// 射击点位置
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// 射击方向
        /// </summary>
        public Vector3 Direction { get; private set; }

        /// <summary>
        /// 射击角度
        /// </summary>
        public float Angle { get; private set; }

        public ShootPointEventData(Vector3 position, Vector3 direction, float angle)
        {
            Position = position;
            Direction = direction;
            Angle = angle;
        }
    }

    /// <summary>
    /// 战斗状态事件数据
    /// </summary>
    public class CombatStateEventData : EventDataBase
    {
        /// <summary>
        /// 战斗状态
        /// </summary>
        public CombatState State { get; private set; }

        /// <summary>
        /// 状态持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 状态来源
        /// </summary>
        public GameObject Source { get; private set; }

        public CombatStateEventData(CombatState state, float duration = 0, GameObject source = null)
        {
            State = state;
            Duration = duration;
            Source = source;
        }
    }

    /// <summary>
    /// 战斗状态枚举
    /// </summary>
    public enum CombatState
    {
        /// <summary>
        /// 普通状态
        /// </summary>
        Normal,

        /// <summary>
        /// 无敌状态
        /// </summary>
        Invincible,

        /// <summary>
        /// 眩晕状态
        /// </summary>
        Stunned,

        /// <summary>
        /// 击退状态
        /// </summary>
        Knockback,

        /// <summary>
        /// 死亡状态
        /// </summary>
        Dead
    }
} 