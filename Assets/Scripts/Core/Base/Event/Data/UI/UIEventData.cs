using UnityEngine;

namespace Core.Base.Event.Data.UI
{
    /// <summary>
    /// UI显示事件数据
    /// </summary>
    public class UIShowEventData : EventDataBase
    {
        /// <summary>
        /// UI元素名称
        /// </summary>
        public string ElementName { get; private set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object Data { get; private set; }

        public UIShowEventData(string elementName, object data = null)
        {
            ElementName = elementName;
            Data = data;
        }
    }

    /// <summary>
    /// UI隐藏事件数据
    /// </summary>
    public class UIHideEventData : EventDataBase
    {
        /// <summary>
        /// UI元素名称
        /// </summary>
        public string ElementName { get; private set; }

        public UIHideEventData(string elementName)
        {
            ElementName = elementName;
        }
    }

    /// <summary>
    /// UI区域更新事件数据
    /// </summary>
    public class UIAreaUpdateEventData : EventDataBase
    {
        /// <summary>
        /// 是否为顶部区域
        /// </summary>
        public bool IsTopArea { get; private set; }

        /// <summary>
        /// 区域大小
        /// </summary>
        public Vector2 Size { get; private set; }

        /// <summary>
        /// 区域位置
        /// </summary>
        public Vector2 Position { get; private set; }

        public UIAreaUpdateEventData(bool isTopArea, Vector2 size, Vector2 position)
        {
            IsTopArea = isTopArea;
            Size = size;
            Position = position;
        }
    }
} 