namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 触发区域系统事件名称
        /// <summary>
        /// 进入触发区域事件
        /// </summary>
        public const string TRIGGER_AREA_ENTER = "TRIGGER_AREA_ENTER";

        /// <summary>
        /// 离开触发区域事件
        /// </summary>
        public const string TRIGGER_AREA_EXIT = "TRIGGER_AREA_EXIT";

        /// <summary>
        /// 触发区域状态变化事件
        /// </summary>
        public const string TRIGGER_AREA_STATE = "TRIGGER_AREA_STATE";

        /// <summary>
        /// 触发区域激活事件
        /// </summary>
        public const string TRIGGER_AREA_ACTIVATE = "TRIGGER_AREA_ACTIVATE";
        #endregion
    }
} 