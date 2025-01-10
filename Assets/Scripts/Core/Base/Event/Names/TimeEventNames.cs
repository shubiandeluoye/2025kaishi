namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 时间系统事件名称
        /// <summary>
        /// 时间缩放事件
        /// </summary>
        public const string TIME_SCALE = "TIME_SCALE";

        /// <summary>
        /// 时间暂停事件
        /// </summary>
        public const string TIME_PAUSE = "TIME_PAUSE";

        /// <summary>
        /// 时间恢复事件
        /// </summary>
        public const string TIME_RESUME = "TIME_RESUME";

        /// <summary>
        /// 时间系统状态变化事件
        /// </summary>
        public const string TIME_STATE_CHANGE = "TIME_STATE_CHANGE";
        #endregion
    }
} 