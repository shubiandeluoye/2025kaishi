namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 移动系统事件名称
        /// <summary>
        /// 移动开始事件
        /// </summary>
        public const string MOVEMENT_START = "MOVEMENT_START";

        /// <summary>
        /// 移动结束事件
        /// </summary>
        public const string MOVEMENT_END = "MOVEMENT_END";

        /// <summary>
        /// 移动状态变化事件
        /// </summary>
        public const string MOVEMENT_STATE_CHANGE = "MOVEMENT_STATE_CHANGE";

        /// <summary>
        /// 移动速度变化事件
        /// </summary>
        public const string MOVEMENT_SPEED_CHANGE = "MOVEMENT_SPEED_CHANGE";

        /// <summary>
        /// 移动方向变化事件
        /// </summary>
        public const string MOVEMENT_DIRECTION_CHANGE = "MOVEMENT_DIRECTION_CHANGE";
        #endregion
    }
} 