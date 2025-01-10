using UnityEngine;

namespace Core.Base.Event.Data.UI
{
    /// <summary>
    /// UI区域更新事件数据
    /// </summary>
    public class UIAreaUpdateEvent : EventDataBase
    {
        /// <summary>
        /// 是否是顶部区域
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

        public UIAreaUpdateEvent(bool isTopArea, Vector2 size, Vector2 position)
        {
            IsTopArea = isTopArea;
            Size = size;
            Position = position;
        }
    }
} 