namespace Core.Base.Event
{
    public static partial class EventNames
    {
        #region 技能系统事件名称
        /// <summary>
        /// 技能系统初始化事件
        /// </summary>
        public const string SKILL_SYSTEM_INITIALIZED = "SKILL_SYSTEM_INITIALIZED";

        /// <summary>
        /// 技能系统错误事件
        /// </summary>
        public const string SKILL_SYSTEM_ERROR = "SKILL_SYSTEM_ERROR";

        /// <summary>
        /// 技能初始化事件
        /// </summary>
        public const string SKILLS_INITIALIZED = "SKILLS_INITIALIZED";

        /// <summary>
        /// 技能配置更新事件
        /// </summary>
        public const string SKILL_CONFIG_UPDATED = "SKILL_CONFIG_UPDATED";

        /// <summary>
        /// 技能等级变化事件
        /// </summary>
        public const string SKILL_LEVEL_CHANGE = "SKILL_LEVEL_CHANGE";

        /// <summary>
        /// 技能测试执行事件
        /// </summary>
        public const string SKILL_TEST_EXECUTED = "SKILL_TEST_EXECUTED";

        /// <summary>
        /// 技能执行事件
        /// </summary>
        public const string SKILL_EXECUTE = "SKILL_EXECUTE";

        /// <summary>
        /// 技能释放开始事件
        /// </summary>
        public const string SKILL_CAST_START = "SKILL_CAST_START";

        /// <summary>
        /// 技能释放结束事件
        /// </summary>
        public const string SKILL_CAST_END = "SKILL_CAST_END";

        /// <summary>
        /// 技能冷却开始事件
        /// </summary>
        public const string SKILL_COOLDOWN_START = "SKILL_COOLDOWN_START";

        /// <summary>
        /// 技能冷却结束事件
        /// </summary>
        public const string SKILL_COOLDOWN_END = "SKILL_COOLDOWN_END";
        #endregion
    }
} 