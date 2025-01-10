using UnityEngine;
using Core.Skills.Shooting;
using Core.Combat.Unit.Base;
using Core.Base.Event;
using Core.Skills.Events;

namespace Core.Skills.Tests
{
    /// <summary>
    /// 技能系统测试组件
    /// </summary>
    public class SkillSystemTests : MonoBehaviour
    {
        private SkillManager skillManager;
        private DirectionalShootingSkill directionalSkill;
        private SpreadShootingSkill spreadSkill;
        private BurstShootingSkill burstSkill;
        private BouncingShootingSkill bounceSkill;

        private void Start()
        {
            if (!VerifyComponents())
            {
                return;
            }

            InitializeSkills();
            ConfigureSkills();
            SubscribeToEvents();

            EventManager.Publish(EventNames.SKILL_SYSTEM_INITIALIZED, 
                new SkillSystemInitializedEvent(gameObject));
        }

        private bool VerifyComponents()
        {
            var shooter = GetComponent<BaseShooter>();
            if (shooter == null)
            {
                EventManager.Publish(EventNames.SKILL_SYSTEM_ERROR, 
                    new SystemErrorEvent("SkillSystemTests requires a BaseShooter component!"));
                return false;
            }
            return true;
        }

        private void InitializeSkills()
        {
            skillManager = gameObject.AddComponent<SkillManager>();
            directionalSkill = gameObject.AddComponent<DirectionalShootingSkill>();
            spreadSkill = gameObject.AddComponent<SpreadShootingSkill>();
            burstSkill = gameObject.AddComponent<BurstShootingSkill>();
            bounceSkill = gameObject.AddComponent<BouncingShootingSkill>();

            EventManager.Publish(EventNames.SKILLS_INITIALIZED, 
                new SkillsInitializedEvent(new BaseSkill[] 
                { 
                    directionalSkill, spreadSkill, burstSkill, bounceSkill 
                }));
        }

        private void ConfigureSkills()
        {
            var skillConfigs = new SkillConfig[]
            {
                new SkillConfig { Name = "Basic Shot", Type = SkillType.Directional },
                new SkillConfig { Name = "Spread Shot", Type = SkillType.Spread },
                new SkillConfig { Name = "Burst Shot", Type = SkillType.Burst },
                new SkillConfig { Name = "Bounce Shot", Type = SkillType.Bounce }
            };

            EventManager.Publish(EventNames.SKILL_CONFIG_UPDATED, 
                new SkillConfigUpdateEvent(skillConfigs));
        }

        private void SubscribeToEvents()
        {
            EventManager.Subscribe<SkillExecuteEvent>(EventManager.EventNames.SKILL_EXECUTE, OnSkillExecute);
            EventManager.Subscribe<SkillLevelChangeEvent>(EventManager.EventNames.SKILL_LEVEL_CHANGE, OnSkillLevelChange);
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Unsubscribe<SkillExecuteEvent>(EventManager.EventNames.SKILL_EXECUTE, OnSkillExecute);
            EventManager.Unsubscribe<SkillLevelChangeEvent>(EventManager.EventNames.SKILL_LEVEL_CHANGE, OnSkillLevelChange);
        }

        private void OnSkillExecute(SkillExecuteEvent evt)
        {
            Vector3 direction = evt.Direction;
            switch (evt.SkillIndex)
            {
                case 0:
                    TestDirectionalShot(direction);
                    break;
                case 1:
                    TestSpreadShot(direction);
                    break;
                case 2:
                    TestBurstShot(direction);
                    break;
                case 3:
                    TestBounceShot(direction);
                    break;
            }
        }

        private void OnSkillLevelChange(SkillLevelChangeEvent evt)
        {
            Vector3 direction = transform.forward;
            switch (evt.Level)
            {
                case 1:
                    TestDirectionalShot(direction);
                    break;
                case 2:
                    TestBurstShot(direction);
                    break;
                case 3:
                    TestBounceShot(direction);
                    break;
            }
        }

        private void TestDirectionalShot(Vector3 direction)
        {
            Debug.Log("Testing Directional Shot");
            EventManager.Publish(EventManager.EventNames.SKILL_TEST_EXECUTED, 
                new SkillTestEvent(SkillType.Directional, direction));
        }

        private void TestSpreadShot(Vector3 direction)
        {
            Debug.Log("Testing Spread Shot");
            EventManager.Publish(EventManager.EventNames.SKILL_TEST_EXECUTED, 
                new SkillTestEvent(SkillType.Spread, direction));
        }

        private void TestBurstShot(Vector3 direction)
        {
            Debug.Log("Testing Burst Shot");
            EventManager.Publish(EventManager.EventNames.SKILL_TEST_EXECUTED, 
                new SkillTestEvent(SkillType.Burst, direction));
        }

        private void TestBounceShot(Vector3 direction)
        {
            Debug.Log("Testing Bounce Shot");
            EventManager.Publish(EventManager.EventNames.SKILL_TEST_EXECUTED, 
                new SkillTestEvent(SkillType.Bounce, direction));
        }

        public bool VerifySkillSystem()
        {
            bool isValid = true;

            if (skillManager == null || GetComponent<BaseShooter>() == null)
            {
                EventManager.Publish(EventManager.EventNames.SKILL_SYSTEM_ERROR, 
                    "Required components missing!");
                isValid = false;
            }

            if (directionalSkill == null || spreadSkill == null || 
                burstSkill == null || bounceSkill == null)
            {
                EventManager.Publish(EventManager.EventNames.SKILL_SYSTEM_ERROR, 
                    "One or more skills are missing!");
                isValid = false;
            }

            return isValid;
        }
    }

    public enum SkillType
    {
        Directional,
        Spread,
        Burst,
        Bounce
    }

    public class SkillConfig
    {
        public string Name { get; set; }
        public SkillType Type { get; set; }
    }

    public class SkillTestEvent
    {
        public SkillType Type { get; private set; }
        public Vector3 Direction { get; private set; }

        public SkillTestEvent(SkillType type, Vector3 direction)
        {
            Type = type;
            Direction = direction;
        }
    }
}
