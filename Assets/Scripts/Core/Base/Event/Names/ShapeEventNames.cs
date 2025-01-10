namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 形状系统事件名称
        /// <summary>
        /// 形状碰撞事件
        /// </summary>
        public const string SHAPE_COLLISION = "SHAPE_COLLISION";

        /// <summary>
        /// 形状变形事件
        /// </summary>
        public const string SHAPE_DEFORM = "SHAPE_DEFORM";

        /// <summary>
        /// 形状旋转事件
        /// </summary>
        public const string SHAPE_ROTATE = "SHAPE_ROTATE";

        /// <summary>
        /// 形状缩放事件
        /// </summary>
        public const string SHAPE_SCALE = "SHAPE_SCALE";

        /// <summary>
        /// 形状状态变化事件
        /// </summary>
        public const string SHAPE_STATE_CHANGE = "SHAPE_STATE_CHANGE";
        #endregion
    }
} 