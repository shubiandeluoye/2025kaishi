using UnityEngine;
using System.Collections.Generic;
using Core.Base.Manager;
using Core.Base.Event;
using Core.Combat.ShootPoint;
using Core.Skills.Base;

namespace Core.Skills
{
    public class SkillManager : BaseManager
    {
        [SerializeField] private List<BaseSkill> skillList = new List<BaseSkill>();
        private Dictionary<int, BaseSkill> skills = new Dictionary<int, BaseSkill>();

        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            InitializeSkills();
        }

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<SkillExecuteEvent>(EventNames.SKILL_EXECUTE, OnSkillExecute);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<SkillExecuteEvent>(EventNames.SKILL_EXECUTE, OnSkillExecute);
        }

        private void InitializeSkills()
        {
            skills.Clear();
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i] != null)
                {
                    skills[i] = skillList[i];
                    EventManager.Publish(EventNames.SKILL_INITIALIZED, 
                        new SkillInitializedEvent(i, skillList[i]));
                }
            }
        }

        private void OnSkillExecute(SkillExecuteEvent evt)
        {
            ExecuteSkill(evt.SkillIndex, evt.Direction);
        }

        public void ExecuteSkill(int skillIndex, Vector3 direction)
        {
            if (skills.TryGetValue(skillIndex, out BaseSkill skill))
            {
                if (skill.IsReady)
                {
                    EventManager.Publish(EventNames.SHOOT_POINT_UPDATE, 
                        new ShootPointEvent(ShootPointEventType.Direction, direction));
                    skill.Execute(direction);
                }
                else
                {
                    EventManager.Publish(EventNames.SKILL_NOT_READY, 
                        new SkillNotReadyEvent(skillIndex, skill.CurrentCooldown));
                }
            }
        }

        public bool IsSkillReady(int skillIndex)
        {
            return skills.TryGetValue(skillIndex, out BaseSkill skill) && skill.IsReady;
        }
    }

    public class SkillExecuteEvent
    {
        public int SkillIndex { get; private set; }
        public Vector3 Direction { get; private set; }

        public SkillExecuteEvent(int skillIndex, Vector3 direction)
        {
            SkillIndex = skillIndex;
            Direction = direction;
        }
    }
}
