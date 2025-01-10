namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 场景系统事件名称
        /// <summary>
        /// 场景加载开始事件
        /// </summary>
        public const string SCENE_LOAD_START = "SCENE_LOAD_START";

        /// <summary>
        /// 场景加载进度事件
        /// </summary>
        public const string SCENE_LOAD_PROGRESS = "SCENE_LOAD_PROGRESS";

        /// <summary>
        /// 场景加载完成事件
        /// </summary>
        public const string SCENE_LOAD_COMPLETE = "SCENE_LOAD_COMPLETE";

        /// <summary>
        /// 场景卸载事件
        /// </summary>
        public const string SCENE_UNLOAD = "SCENE_UNLOAD";

        /// <summary>
        /// 场景切换事件
        /// </summary>
        public const string SCENE_TRANSITION = "SCENE_TRANSITION";

        /// <summary>
        /// 场景初始化事件
        /// </summary>
        public const string SCENE_INITIALIZE = "SCENE_INITIALIZE";
        #endregion
    }
} 