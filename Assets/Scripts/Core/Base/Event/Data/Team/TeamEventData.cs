using UnityEngine;
using System.Collections.Generic;

namespace Core.Base.Event.Data.Team
{
    /// <summary>
    /// 团队关系事件数据
    /// </summary>
    public class TeamRelationEventData : EventDataBase
    {
        /// <summary>
        /// 团队A的ID
        /// </summary>
        public int TeamAId { get; private set; }

        /// <summary>
        /// 团队B的ID
        /// </summary>
        public int TeamBId { get; private set; }

        /// <summary>
        /// 新的关系类型
        /// </summary>
        public TeamRelationType RelationType { get; private set; }

        /// <summary>
        /// 关系变化原因
        /// </summary>
        public string Reason { get; private set; }

        public TeamRelationEventData(int teamAId, int teamBId, TeamRelationType relationType, string reason = "")
        {
            TeamAId = teamAId;
            TeamBId = teamBId;
            RelationType = relationType;
            Reason = reason;
        }
    }

    /// <summary>
    /// 对立团队更新事件数据
    /// </summary>
    public class TeamOppositeEventData : EventDataBase
    {
        /// <summary>
        /// 团队ID
        /// </summary>
        public int TeamId { get; private set; }

        /// <summary>
        /// 对立团队ID列表
        /// </summary>
        public List<int> OppositeTeamIds { get; private set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public float UpdateTime { get; private set; }

        public TeamOppositeEventData(int teamId, List<int> oppositeTeamIds)
        {
            TeamId = teamId;
            OppositeTeamIds = oppositeTeamIds;
            UpdateTime = Time.time;
        }
    }

    /// <summary>
    /// 团队成员更新事件数据
    /// </summary>
    public class TeamMemberEventData : EventDataBase
    {
        /// <summary>
        /// 团队ID
        /// </summary>
        public int TeamId { get; private set; }

        /// <summary>
        /// 成员对象
        /// </summary>
        public GameObject Member { get; private set; }

        /// <summary>
        /// 是否为添加操作
        /// </summary>
        public bool IsAdd { get; private set; }

        public TeamMemberEventData(int teamId, GameObject member, bool isAdd)
        {
            TeamId = teamId;
            Member = member;
            IsAdd = isAdd;
        }
    }

    /// <summary>
    /// 团队关系类型枚举
    /// </summary>
    public enum TeamRelationType
    {
        /// <summary>
        /// 友好
        /// </summary>
        Friendly,

        /// <summary>
        /// 中立
        /// </summary>
        Neutral,

        /// <summary>
        /// 敌对
        /// </summary>
        Hostile,

        /// <summary>
        /// 联盟
        /// </summary>
        Allied
    }
} 