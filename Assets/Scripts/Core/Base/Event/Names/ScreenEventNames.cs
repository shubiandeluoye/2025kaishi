namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 屏幕系统事件名称
        /// <summary>
        /// 屏幕震动事件
        /// </summary>
        public const string SCREEN_SHAKE = "SCREEN_SHAKE";

        /// <summary>
        /// 屏幕闪烁事件
        /// </summary>
        public const string SCREEN_FLASH = "SCREEN_FLASH";

        /// <summary>
        /// 屏幕渐变事件
        /// </summary>
        public const string SCREEN_FADE = "SCREEN_FADE";

        /// <summary>
        /// 屏幕特效事件
        /// </summary>
        public const string SCREEN_EFFECT = "SCREEN_EFFECT";
        #endregion

        #region 屏幕效果事件名称
        /// <summary>
        /// 高亮开始事件
        /// </summary>
        public const string HIGHLIGHT_START = "HIGHLIGHT_START";
        #endregion
    }
} 