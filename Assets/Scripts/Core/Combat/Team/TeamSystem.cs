using UnityEngine;
using Core.Base.Manager;
using Core.Base.Event;
using Core.Base.Event.Data.Combat;

namespace Core.Combat.Team
{
    /// <summary>
    /// 队伍类型枚举
    /// </summary>
    public enum TeamType
    {
        Player,     // 玩家队伍
        Ally,       // 盟友队伍
        Enemy,      // 敌人队伍
        System      // 系统/中立队伍
    }

    /// <summary>
    /// 队伍关系枚举
    /// </summary>
    public enum TeamRelation
    {
        Friend,     // 友好关系（自己和盟友）
        Hostile,    // 敌对关系（敌人）
        Neutral     // 中立关系（系统）
    }

    /// <summary>
    /// 队伍系统
    /// 处理队伍关系和判断
    /// </summary>
    public class TeamSystem : BaseManager
    {
        protected override void RegisterEvents()
        {
            EventManager.Subscribe<TeamRelationEvent>(EventNames.TEAM_RELATION_CHECK, OnTeamRelationCheck);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<TeamRelationEvent>(EventNames.TEAM_RELATION_CHECK, OnTeamRelationCheck);
        }

        private void OnTeamRelationCheck(TeamRelationEvent evt)
        {
            TeamRelation relation = GetTeamRelation(evt.TeamA, evt.TeamB);
            EventManager.Publish(EventNames.TEAM_RELATION_RESULT, 
                new TeamRelationResultEvent(evt.TeamA, evt.TeamB, relation));
        }

        /// <summary>
        /// 判断两个队伍的关系
        /// </summary>
        public static TeamRelation GetTeamRelation(TeamType teamA, TeamType teamB)
        {
            // 如果是同一个队伍，一定是友好关系
            if (teamA == teamB)
            {
                EventManager.Publish(EventNames.TEAM_RELATION_CHANGED, 
                    new TeamRelationEvent(teamA, teamB));
                return TeamRelation.Friend;
            }

            // 如果任何一方是系统队伍，则为中立关系
            if (teamA == TeamType.System || teamB == TeamType.System)
                return TeamRelation.Neutral;

            // 玩家和盟友之间是友好关系
            if ((teamA == TeamType.Player && teamB == TeamType.Ally) ||
                (teamA == TeamType.Ally && teamB == TeamType.Player))
                return TeamRelation.Friend;

            // 其他情况都是敌对关系
            return TeamRelation.Hostile;
        }

        /// <summary>
        /// 判断是否是敌对关系
        /// </summary>
        public static bool IsHostile(TeamType teamA, TeamType teamB)
        {
            bool isHostile = GetTeamRelation(teamA, teamB) == TeamRelation.Hostile;
            if (isHostile)
            {
                EventManager.Publish(EventNames.TEAM_HOSTILE_DETECTED, 
                    new TeamRelationEvent(teamA, teamB));
            }
            return isHostile;
        }

        /// <summary>
        /// 判断是否是友好关系
        /// </summary>
        public static bool IsFriend(TeamType teamA, TeamType teamB)
        {
            bool isFriend = GetTeamRelation(teamA, teamB) == TeamRelation.Friend;
            if (isFriend)
            {
                EventManager.Publish(EventNames.TEAM_FRIENDLY_DETECTED, 
                    new TeamRelationEvent(teamA, teamB));
            }
            return isFriend;
        }

        /// <summary>
        /// 判断是否是中立关系
        /// </summary>
        public static bool IsNeutral(TeamType teamA, TeamType teamB)
        {
            return GetTeamRelation(teamA, teamB) == TeamRelation.Neutral;
        }

        /// <summary>
        /// 获取对手队伍
        /// </summary>
        public static TeamType GetOppositeTeam(TeamType team)
        {
            TeamType oppositeTeam = team switch
            {
                TeamType.Player or TeamType.Ally => TeamType.Enemy,
                TeamType.Enemy => TeamType.Player,
                _ => TeamType.System
            };

            EventManager.Publish(EventNames.TEAM_OPPOSITE_FOUND, 
                new TeamOppositeEvent(team, oppositeTeam));
            
            return oppositeTeam;
        }
    }

    /// <summary>
    /// 队伍组件接口
    /// 添加到需要有队伍属性的物体上
    /// </summary>
    public interface ITeamMember
    {
        TeamType Team { get; }
    }
} 