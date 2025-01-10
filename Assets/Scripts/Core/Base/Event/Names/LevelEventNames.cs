namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 等级系统事件名称
        /// <summary>
        /// 等级提升事件
        /// </summary>
        public const string LEVEL_UP = "LEVEL_UP";

        /// <summary>
        /// 经验值获取事件
        /// </summary>
        public const string EXP_GAINED = "EXP_GAINED";

        /// <summary>
        /// 等级奖励事件
        /// </summary>
        public const string LEVEL_REWARD = "LEVEL_REWARD";

        /// <summary>
        /// 等级系统初始化事件
        /// </summary>
        public const string LEVEL_SYSTEM_INIT = "LEVEL_SYSTEM_INIT";
        #endregion
    }
} 