using UnityEngine;
using Core.Combat.Unit.Base;
using Core.Skills;

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
        #endregion

        #region Input Settings
        [Header("Input Settings")]
        [Tooltip("鼠标/触摸灵敏度")]
        [SerializeField] private float inputSensitivity = 2f;
        
        [Tooltip("是否反转Y轴")]
        [SerializeField] private bool invertY = false;

        [Tooltip("移动端虚拟摇杆死区")]
        [SerializeField] private float joystickDeadZone = 0.1f;
        #endregion

        #region Runtime Variables
        private Vector2 lastTouchPosition;
        private bool isTouching;
        private Vector2 moveInput;
        #endregion

        #region Platform Detection
        private bool IsMobilePlatform
        {
            get
            {
                return Application.isMobilePlatform;
            }
        }
        #endregion

        #region Initialization
        private void Awake()
        {
            shooter = GetComponent<BaseShooter>();
            skillManager = GetComponent<SkillManager>();

            if (shooter == null)
            {
                Debug.LogError("PlayerController requires a BaseShooter component!");
            }

            if (skillManager == null)
            {
                Debug.LogError("PlayerController requires a SkillManager component!");
            }
        }
        #endregion

        #region Update Loop
        private void Update()
        {
            if (IsMobilePlatform)
            {
                HandleMobileInput();
            }
            else
            {
                HandlePCInput();
            }
        }
        #endregion

        #region PC Input Handling
        /// <summary>
        /// 处理PC端输入（鼠标和键盘）
        /// </summary>
        private void HandlePCInput()
        {
            HandleMouseInput();
            HandleKeyboardSkillInput();
        }

        private void HandleMouseInput()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
            Vector3 direction = (worldPosition - transform.position).normalized;

            // 更新朝向
            transform.right = direction;

            // 基础射击 - 左键
            if (Input.GetMouseButton(0) && skillManager.IsSkillReady(0))
            {
                skillManager.ExecuteSkill(0, direction);
            }
        }

        private void HandleKeyboardSkillInput()
        {
            Vector3 direction = transform.right;

            // 散射 - 右键
            if (Input.GetMouseButton(1) && skillManager.IsSkillReady(1))
            {
                skillManager.ExecuteSkill(1, direction);
            }

            // 连发 - Q键
            if (Input.GetKey(KeyCode.Q) && skillManager.IsSkillReady(2))
            {
                skillManager.ExecuteSkill(2, direction);
            }

            // 弹跳射击 - E键
            if (Input.GetKey(KeyCode.E) && skillManager.IsSkillReady(3))
            {
                skillManager.ExecuteSkill(3, direction);
            }
        }
        #endregion

        #region Mobile Input Handling
        /// <summary>
        /// 处理移动端输入（触摸）
        /// </summary>
        private void HandleMobileInput()
        {
            // 处理触摸输入
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        HandleTouchBegan(touch);
                        break;
                    case TouchPhase.Moved:
                        HandleTouchMoved(touch);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        HandleTouchEnded();
                        break;
                }
            }

            // 处理虚拟按钮输入（需要UI系统支持）
            HandleVirtualButtons();
        }

        private void HandleTouchBegan(Touch touch)
        {
            isTouching = true;
            lastTouchPosition = touch.position;
        }

        private void HandleTouchMoved(Touch touch)
        {
            if (!isTouching) return;

            // 计算触摸移动方向
            Vector2 touchDelta = touch.position - lastTouchPosition;
            Vector3 worldDirection = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f)) - transform.position;
            worldDirection.Normalize();

            // 更新朝向
            transform.right = worldDirection;

            // 如果移动距离超过阈值，视为有效输入
            if (touchDelta.magnitude > joystickDeadZone)
            {
                moveInput = touchDelta.normalized;
            }

            lastTouchPosition = touch.position;
        }

        private void HandleTouchEnded()
        {
            isTouching = false;
            moveInput = Vector2.zero;
        }

        private void HandleVirtualButtons()
        {
            // 这里需要配合UI系统实现虚拟按钮
            // 示例：通过UI按钮触发技能
            // 实际实现时需要添加UI按钮并绑定这些方法
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 触发技能（供UI按钮调用）
        /// </summary>
        public void TriggerSkill(int skillIndex)
        {
            if (skillManager.IsSkillReady(skillIndex))
            {
                skillManager.ExecuteSkill(skillIndex, transform.right);
            }
        }

        /// <summary>
        /// 设置输入灵敏度
        /// </summary>
        public void SetInputSensitivity(float sensitivity)
        {
            inputSensitivity = Mathf.Max(0.1f, sensitivity);
        }

        /// <summary>
        /// 设置Y轴是否反转
        /// </summary>
        public void SetInvertY(bool invert)
        {
            invertY = invert;
        }
        #endregion
    }
}

