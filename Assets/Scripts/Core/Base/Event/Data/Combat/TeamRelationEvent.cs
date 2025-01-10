using Core.Combat.Team;

namespace Core.Base.Event.Data.Combat
{
    /// <summary>
    /// 队伍关系事件数据
    /// </summary>
    public class TeamRelationEvent : EventDataBase
    {
        /// <summary>
        /// 队伍A
        /// </summary>
        public TeamType TeamA { get; private set; }

        /// <summary>
        /// 队伍B
        /// </summary>
        public TeamType TeamB { get; private set; }

        public TeamRelationEvent(TeamType teamA, TeamType teamB)
        {
            TeamA = teamA;
            TeamB = teamB;
        }
    }

    /// <summary>
    /// 队伍关系结果事件数据
    /// </summary>
    public class TeamRelationResultEvent : EventDataBase
    {
        /// <summary>
        /// 队伍A
        /// </summary>
        public TeamType TeamA { get; private set; }

        /// <summary>
        /// 队伍B
        /// </summary>
        public TeamType TeamB { get; private set; }

        /// <summary>
        /// 队伍关系
        /// </summary>
        public TeamRelation Relation { get; private set; }

        public TeamRelationResultEvent(TeamType teamA, TeamType teamB, TeamRelation relation)
        {
            TeamA = teamA;
            TeamB = teamB;
            Relation = relation;
        }
    }

    /// <summary>
    /// 对手队伍事件数据
    /// </summary>
    public class TeamOppositeEvent : EventDataBase
    {
        /// <summary>
        /// 原始队伍
        /// </summary>
        public TeamType OriginalTeam { get; private set; }

        /// <summary>
        /// 对手队伍
        /// </summary>
        public TeamType OppositeTeam { get; private set; }

        public TeamOppositeEvent(TeamType originalTeam, TeamType oppositeTeam)
        {
            OriginalTeam = originalTeam;
            OppositeTeam = oppositeTeam;
        }
    }
} 