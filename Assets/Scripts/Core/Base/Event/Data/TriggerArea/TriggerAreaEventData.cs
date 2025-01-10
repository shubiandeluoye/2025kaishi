using UnityEngine;

namespace Core.Base.Event.Data.TriggerArea
{
    /// <summary>
    /// 触发区域基础事件数据
    /// </summary>
    public class TriggerAreaEventData : EventDataBase
    {
        /// <summary>
        /// 触发区域对象
        /// </summary>
        public GameObject TriggerArea { get; private set; }

        /// <summary>
        /// 触发者对象
        /// </summary>
        public GameObject Triggerer { get; private set; }

        /// <summary>
        /// 触发点
        /// </summary>
        public Vector3 TriggerPoint { get; private set; }

        /// <summary>
        /// 触发时间
        /// </summary>
        public float TriggerTime { get; private set; }

        public TriggerAreaEventData(GameObject triggerArea, GameObject triggerer, 
            Vector3 triggerPoint)
        {
            TriggerArea = triggerArea;
            Triggerer = triggerer;
            TriggerPoint = triggerPoint;
            TriggerTime = Time.time;
        }
    }

    /// <summary>
    /// 触发区域状态事件数据
    /// </summary>
    public class TriggerAreaStateEventData : EventDataBase
    {
        /// <summary>
        /// 触发区域对象
        /// </summary>
        public GameObject TriggerArea { get; private set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public TriggerAreaState NewState { get; private set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public TriggerAreaState OldState { get; private set; }

        /// <summary>
        /// 状态持续时间
        /// </summary>
        public float Duration { get; private set; }

        public TriggerAreaStateEventData(GameObject triggerArea, 
            TriggerAreaState newState, TriggerAreaState oldState, float duration = 0)
        {
            TriggerArea = triggerArea;
            NewState = newState;
            OldState = oldState;
            Duration = duration;
        }
    }

    /// <summary>
    /// 触发区域激活事件数据
    /// </summary>
    public class TriggerAreaActivateEventData : EventDataBase
    {
        /// <summary>
        /// 触发区域对象
        /// </summary>
        public GameObject TriggerArea { get; private set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// 激活者
        /// </summary>
        public GameObject Activator { get; private set; }

        public TriggerAreaActivateEventData(GameObject triggerArea, bool isActive, 
            GameObject activator = null)
        {
            TriggerArea = triggerArea;
            IsActive = isActive;
            Activator = activator;
        }
    }

    /// <summary>
    /// 触发区域状态枚举
    /// </summary>
    public enum TriggerAreaState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle,

        /// <summary>
        /// 激活
        /// </summary>
        Active,

        /// <summary>
        /// 冷却
        /// </summary>
        Cooldown,

        /// <summary>
        /// 禁用
        /// </summary>
        Disabled
    }
} 