namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 输入系统事件名称
        /// <summary>
        /// 触摸输入事件
        /// </summary>
        public const string TOUCH_INPUT = "TOUCH_INPUT";

        /// <summary>
        /// 键盘输入事件
        /// </summary>
        public const string KEYBOARD_INPUT = "KEYBOARD_INPUT";

        /// <summary>
        /// 鼠标输入事件
        /// </summary>
        public const string MOUSE_INPUT = "MOUSE_INPUT";

        /// <summary>
        /// 角度切换事件
        /// </summary>
        public const string ANGLE_TOGGLE = "ANGLE_TOGGLE";

        /// <summary>
        /// 输入模式切换事件
        /// </summary>
        public const string INPUT_MODE_CHANGE = "INPUT_MODE_CHANGE";
        #endregion
    }
} 