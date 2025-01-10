namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 战斗系统事件名称
        /// <summary>
        /// 造成伤害事件
        /// </summary>
        public const string DAMAGE_DEALT = "DAMAGE_DEALT";

        /// <summary>
        /// 生命值变化事件
        /// </summary>
        public const string HEALTH_CHANGED = "HEALTH_CHANGED";

        /// <summary>
        /// 单位死亡事件
        /// </summary>
        public const string UNIT_DIED = "UNIT_DIED";

        /// <summary>
        /// 战斗开始事件
        /// </summary>
        public const string COMBAT_START = "COMBAT_START";

        /// <summary>
        /// 战斗结束事件
        /// </summary>
        public const string COMBAT_END = "COMBAT_END";

        /// <summary>
        /// 玩家生命值变化事件
        /// </summary>
        public const string PLAYER_HEALTH_CHANGED = "PLAYER_HEALTH_CHANGED";

        /// <summary>
        /// 玩家分数变化事件
        /// </summary>
        public const string PLAYER_SCORE_CHANGED = "PLAYER_SCORE_CHANGED";

        /// <summary>
        /// 玩家被击败事件
        /// </summary>
        public const string PLAYER_DEFEATED = "PLAYER_DEFEATED";

        /// <summary>
        /// 射击点更新事件
        /// </summary>
        public const string SHOOT_POINT_UPDATE = "SHOOT_POINT_UPDATE";

        /// <summary>
        /// 技能执行事件
        /// </summary>
        public const string SKILL_EXECUTE = "SKILL_EXECUTE";

        /// <summary>
        /// 子弹命中事件
        /// </summary>
        public const string BULLET_HIT = "BULLET_HIT";

        /// <summary>
        /// 子弹销毁事件
        /// </summary>
        public const string BULLET_DESTROYED = "BULLET_DESTROYED";

        /// <summary>
        /// 子弹轨迹修改事件
        /// </summary>
        public const string BULLET_TRAJECTORY_MODIFIED = "BULLET_TRAJECTORY_MODIFIED";
        #endregion

        #region 队伍系统事件名称
        /// <summary>
        /// 队伍关系检查事件
        /// </summary>
        public const string TEAM_RELATION_CHECK = "TEAM_RELATION_CHECK";

        /// <summary>
        /// 队伍关系结果事件
        /// </summary>
        public const string TEAM_RELATION_RESULT = "TEAM_RELATION_RESULT";

        /// <summary>
        /// 队伍关系改变事件
        /// </summary>
        public const string TEAM_RELATION_CHANGED = "TEAM_RELATION_CHANGED";

        /// <summary>
        /// 检测到敌对关系事件
        /// </summary>
        public const string TEAM_HOSTILE_DETECTED = "TEAM_HOSTILE_DETECTED";

        /// <summary>
        /// 检测到友好关系事件
        /// </summary>
        public const string TEAM_FRIENDLY_DETECTED = "TEAM_FRIENDLY_DETECTED";

        /// <summary>
        /// 找到对手队伍事件
        /// </summary>
        public const string TEAM_OPPOSITE_FOUND = "TEAM_OPPOSITE_FOUND";
        #endregion
    }
} 