namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region UI系统事件名称
        /// <summary>
        /// UI显示事件
        /// </summary>
        public const string UI_SHOW = "UI_SHOW";

        /// <summary>
        /// UI隐藏事件
        /// </summary>
        public const string UI_HIDE = "UI_HIDE";

        /// <summary>
        /// UI初始化事件
        /// </summary>
        public const string UI_INITIALIZE = "UI_INITIALIZE";

        /// <summary>
        /// UI区域更新事件
        /// </summary>
        public const string UI_AREA_UPDATE = "UI_AREA_UPDATE";

        #region UI动画相关
        /// <summary>
        /// UI动画开始事件
        /// </summary>
        public const string UI_ANIMATION_START = "UI_ANIMATION_START";

        /// <summary>
        /// UI动画完成事件
        /// </summary>
        public const string UI_ANIMATION_COMPLETE = "UI_ANIMATION_COMPLETE";

        /// <summary>
        /// UI过渡开始事件
        /// </summary>
        public const string UI_TRANSITION_START = "UI_TRANSITION_START";

        /// <summary>
        /// UI过渡完成事件
        /// </summary>
        public const string UI_TRANSITION_COMPLETE = "UI_TRANSITION_COMPLETE";
        #endregion

        #region HUD相关
        /// <summary>
        /// HUD交互事件
        /// </summary>
        public const string HUD_INTERACTION = "HUD_INTERACTION";

        /// <summary>
        /// HUD战斗信息更新事件
        /// </summary>
        public const string HUD_COMBAT = "HUD_COMBAT";

        /// <summary>
        /// HUD状态更新事件
        /// </summary>
        public const string HUD_UPDATE = "HUD_UPDATE";
        #endregion

        #region 菜单相关
        /// <summary>
        /// 菜单显示事件
        /// </summary>
        public const string MENU_SHOW = "MENU_SHOW";

        /// <summary>
        /// 菜单隐藏事件
        /// </summary>
        public const string MENU_HIDE = "MENU_HIDE";

        /// <summary>
        /// 游戏暂停事件
        /// </summary>
        public const string GAME_PAUSE = "GAME_PAUSE";

        /// <summary>
        /// 游戏恢复事件
        /// </summary>
        public const string GAME_RESUME = "GAME_RESUME";

        /// <summary>
        /// 菜单状态改变事件
        /// </summary>
        public const string MENU_STATE_CHANGED = "MENU_STATE_CHANGED";

        /// <summary>
        /// 菜单打开事件
        /// </summary>
        public const string MENU_OPENED = "MENU_OPENED";

        /// <summary>
        /// 菜单关闭事件
        /// </summary>
        public const string MENU_CLOSED = "MENU_CLOSED";
        #endregion
        #endregion
    }
} 