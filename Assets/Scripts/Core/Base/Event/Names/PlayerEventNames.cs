namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 玩家系统事件名称
        /// <summary>
        /// 玩家移动事件
        /// </summary>
        public const string PLAYER_MOVE = "PLAYER_MOVE";

        /// <summary>
        /// 玩家射击事件
        /// </summary>
        public const string PLAYER_SHOOT = "PLAYER_SHOOT";

        /// <summary>
        /// 玩家生命值变化事件
        /// </summary>
        public const string PLAYER_HEALTH_CHANGE = "PLAYER_HEALTH_CHANGE";

        /// <summary>
        /// 玩家死亡事件
        /// </summary>
        public const string PLAYER_DEATH = "PLAYER_DEATH";

        /// <summary>
        /// 玩家复活事件
        /// </summary>
        public const string PLAYER_RESPAWN = "PLAYER_RESPAWN";

        /// <summary>
        /// 玩家状态变化事件
        /// </summary>
        public const string PLAYER_STATE_CHANGE = "PLAYER_STATE_CHANGE";

        /// <summary>
        /// 玩家装备变化事件
        /// </summary>
        public const string PLAYER_EQUIPMENT_CHANGE = "PLAYER_EQUIPMENT_CHANGE";
        #endregion
    }
} 