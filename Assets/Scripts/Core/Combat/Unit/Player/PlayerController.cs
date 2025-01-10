using UnityEngine;
using Core.Combat.Unit.Base;
using Core.Skills;
using Core.Base.Event;
using Core.Combat.ShootPoint;

namespace Core.Combat.Unit.Player
{
    /// <summary>
    /// 跨平台玩家控制器
    /// 支持PC和移动端输入
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Components
        private BaseShooter shooter;
        private SkillManager skillManager;
        [SerializeField] private ShootPointSystem shootPointSystem;
        #endregion

        #region Settings
        [Header("Input Settings")]
        [SerializeField] private float inputSensitivity = 2f;
        [SerializeField] private bool invertY = false;
        [SerializeField] private float joystickDeadZone = 0.1f;
        #endregion

        #region Runtime Variables
        private Vector2 lastTouchPosition;
        private bool isTouching;
        private Vector2 moveInput;
        private int currentSkillIndex = 0;
        #endregion

        #region Initialization
        private void Awake()
        {
            shooter = GetComponent<BaseShooter>();
            skillManager = GetComponent<SkillManager>();

            if (shooter == null)
                Debug.LogError("PlayerController requires a BaseShooter component!");

            if (skillManager == null)
                Debug.LogError("PlayerController requires a SkillManager component!");

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region Event Handling
        private void SubscribeToEvents()
        {
            EventManager.Subscribe<Vector2>(EventNames.PLAYER_MOVE, OnPlayerMove);
            EventManager.Subscribe<ShootType>(EventNames.PLAYER_SHOOT, OnPlayerShoot);
            EventManager.Subscribe<Vector2, TouchEventType>(EventNames.TOUCH_INPUT, OnTouchInput);
            EventManager.Subscribe<int, int>(EventNames.SKILL_LEVEL_UP, OnSkillLevelChange);
            EventManager.Subscribe<float>(EventNames.ANGLE_TOGGLE, OnAngleToggle);
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.Unsubscribe<Vector2>(EventNames.PLAYER_MOVE, OnPlayerMove);
            EventManager.Unsubscribe<ShootType>(EventNames.PLAYER_SHOOT, OnPlayerShoot);
            EventManager.Unsubscribe<Vector2, TouchEventType>(EventNames.TOUCH_INPUT, OnTouchInput);
            EventManager.Unsubscribe<int, int>(EventNames.SKILL_LEVEL_UP, OnSkillLevelChange);
            EventManager.Unsubscribe<float>(EventNames.ANGLE_TOGGLE, OnAngleToggle);
        }

        private void OnPlayerMove(Vector2 moveDirection)
        {
            if (moveDirection.magnitude > joystickDeadZone)
            {
                moveInput = moveDirection;
                Vector3 moveDirection3D = new Vector3(moveInput.x, moveInput.y, 0f);
                transform.position += moveDirection3D * inputSensitivity * Time.deltaTime;
            }
        }

        private void OnPlayerShoot(ShootType type)
        {
            Vector3 direction = transform.right;
            switch (type)
            {
                case ShootType.Straight:
                    if (skillManager.IsSkillReady(0))
                        skillManager.ExecuteSkill(0, direction);
                    break;
                case ShootType.LeftAngle:
                    if (skillManager.IsSkillReady(1))
                        skillManager.ExecuteSkill(1, direction);
                    break;
                case ShootType.RightAngle:
                    if (skillManager.IsSkillReady(2))
                        skillManager.ExecuteSkill(2, direction);
                    break;
                case ShootType.Level3:
                    if (skillManager.IsSkillReady(3))
                        skillManager.ExecuteSkill(3, direction);
                    break;
            }
        }

        private void OnTouchInput(Vector2 position, TouchEventType type)
        {
            switch (type)
            {
                case TouchEventType.Move:
                    HandleMovement(position);
                    break;
                case TouchEventType.Attack:
                    HandleAttack(position);
                    break;
                case TouchEventType.SkillAim:
                    HandleSkillAim(position);
                    break;
                case TouchEventType.SkillTrigger:
                    HandleSkillTrigger(position);
                    break;
            }
        }

        private void OnSkillLevelChange(int skillIndex, int level)
        {
            if (skillManager.IsSkillReady(skillIndex))
            {
                skillManager.ExecuteSkill(skillIndex, transform.right);
            }
        }

        private void OnAngleToggle(float angle)
        {
            // 处理角度切换
            // 可以在这里添加角度切换的视觉反馈
        }
        #endregion

        #region Public Methods
        public void SetInputSensitivity(float sensitivity)
        {
            inputSensitivity = Mathf.Max(0.1f, sensitivity);
        }

        public void SetInvertY(bool invert)
        {
            invertY = invert;
        }

        public void SetCurrentSkill(int skillIndex)
        {
            currentSkillIndex = skillIndex;
        }
        #endregion

        #region Private Methods
        private void HandleMovement(Vector2 position)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
            Vector3 direction = (worldPos - transform.position).normalized;
            
            EventManager.Publish(EventNames.SHOOT_POINT_UPDATE, direction);
        }

        private void HandleAttack(Vector2 position)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
            Vector3 direction = (worldPos - transform.position).normalized;
            
            EventManager.Publish(EventNames.SHOOT_POINT_UPDATE, direction);
            EventManager.Publish(EventNames.SKILL_CAST_START, new SkillCastData(0, direction));
        }

        private void HandleSkillAim(Vector2 position)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
            Vector3 direction = (worldPos - transform.position).normalized;
            
            EventManager.Publish(EventNames.SHOOT_POINT_UPDATE, direction);
        }

        private void HandleSkillTrigger(Vector2 position)
        {
            if (currentSkillIndex > 0)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
                Vector3 direction = (worldPos - transform.position).normalized;
                
                EventManager.Publish(EventNames.SKILL_CAST_START, 
                    new SkillCastData(currentSkillIndex, direction));
            }
        }
        #endregion
    }

    public struct SkillCastData
    {
        public int SkillIndex;
        public Vector3 Direction;

        public SkillCastData(int skillIndex, Vector3 direction)
        {
            SkillIndex = skillIndex;
            Direction = direction;
        }
    }
}

