namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 游戏核心系统事件名称
        /// <summary>
        /// 游戏开始事件
        /// </summary>
        public const string GAME_START = "GAME_START";

        /// <summary>
        /// 游戏结束事件
        /// </summary>
        public const string GAME_OVER = "GAME_OVER";

        /// <summary>
        /// 游戏暂停事件
        /// </summary>
        public const string GAME_PAUSE = "GAME_PAUSE";

        /// <summary>
        /// 游戏继续事件
        /// </summary>
        public const string GAME_RESUME = "GAME_RESUME";

        /// <summary>
        /// 分数更新事件
        /// </summary>
        public const string SCORE_UPDATE = "SCORE_UPDATE";

        /// <summary>
        /// 玩家生命值为零事件
        /// </summary>
        public const string PLAYER_HEALTH_ZERO = "PLAYER_HEALTH_ZERO";

        /// <summary>
        /// 玩家出界事件
        /// </summary>
        public const string PLAYER_OUT_OF_BOUNDS = "PLAYER_OUT_OF_BOUNDS";

        /// <summary>
        /// 玩家胜利事件
        /// </summary>
        public const string PLAYER_VICTORY = "PLAYER_VICTORY";
        #endregion
    }
} 