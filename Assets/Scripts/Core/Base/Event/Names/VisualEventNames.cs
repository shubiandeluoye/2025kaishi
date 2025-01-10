namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 视觉效果系统事件名称
        /// <summary>
        /// 效果播放事件
        /// </summary>
        public const string EFFECT_PLAY = "EFFECT_PLAY";

        /// <summary>
        /// 效果停止事件
        /// </summary>
        public const string EFFECT_STOP = "EFFECT_STOP";

        /// <summary>
        /// 效果完成事件
        /// </summary>
        public const string EFFECT_COMPLETE = "EFFECT_COMPLETE";

        /// <summary>
        /// 效果进度更新事件
        /// </summary>
        public const string EFFECT_PROGRESS = "EFFECT_PROGRESS";

        /// <summary>
        /// 效果对象池创建事件
        /// </summary>
        public const string EFFECT_POOL_CREATED = "EFFECT_POOL_CREATED";

        /// <summary>
        /// 效果系统错误事件
        /// </summary>
        public const string EFFECT_ERROR = "EFFECT_ERROR";
        #endregion
    }
} 