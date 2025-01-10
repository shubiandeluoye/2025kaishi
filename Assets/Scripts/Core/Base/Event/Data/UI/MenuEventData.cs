namespace Core.Base.Event.Data.UI
{
    /// <summary>
    /// 菜单状态事件数据
    /// </summary>
    public class MenuStateEventData : EventDataBase
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; private set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; private set; }

        public MenuStateEventData(string menuName, bool isActive)
        {
            MenuName = menuName;
            IsActive = isActive;
        }
    }

    /// <summary>
    /// 菜单交互事件数据
    /// </summary>
    public class MenuInteractionEventData : EventDataBase
    {
        /// <summary>
        /// 交互类型
        /// </summary>
        public MenuInteractionType InteractionType { get; private set; }

        /// <summary>
        /// 菜单项ID
        /// </summary>
        public string ItemId { get; private set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object Data { get; private set; }

        public MenuInteractionEventData(MenuInteractionType type, string itemId, object data = null)
        {
            InteractionType = type;
            ItemId = itemId;
            Data = data;
        }
    }

    /// <summary>
    /// 菜单交互类型
    /// </summary>
    public enum MenuInteractionType
    {
        /// <summary>
        /// 点击
        /// </summary>
        Click,

        /// <summary>
        /// 选择
        /// </summary>
        Select,

        /// <summary>
        /// 悬停
        /// </summary>
        Hover,

        /// <summary>
        /// 拖拽开始
        /// </summary>
        DragStart,

        /// <summary>
        /// 拖拽结束
        /// </summary>
        DragEnd,

        /// <summary>
        /// 值改变
        /// </summary>
        ValueChanged
    }
} 