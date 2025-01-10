using UnityEngine;

namespace Core.Base.Event.Data.UI
{
    /// <summary>
    /// HUD交互事件数据
    /// </summary>
    public class HUDInteractionEventData : EventDataBase
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public string ElementName { get; private set; }

        /// <summary>
        /// 交互位置
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// 位置变化量
        /// </summary>
        public Vector2 Delta { get; private set; }

        /// <summary>
        /// 是否处于激活状态
        /// </summary>
        public bool IsActive { get; private set; }

        public HUDInteractionEventData(string elementName, Vector2 position, Vector2 delta = default, bool isActive = true)
        {
            ElementName = elementName;
            Position = position;
            Delta = delta;
            IsActive = isActive;
        }
    }

    /// <summary>
    /// HUD更新事件数据
    /// </summary>
    public class HUDUpdateEventData : EventDataBase
    {
        /// <summary>
        /// 更新类型
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 更新值
        /// </summary>
        public object Value { get; private set; }

        public HUDUpdateEventData(string type, object value)
        {
            Type = type;
            Value = value;
        }
    }
} 