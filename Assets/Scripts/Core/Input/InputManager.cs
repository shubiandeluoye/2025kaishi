using UnityEngine;
using UnityEngine.InputSystem;
using Core.Base.Event;

namespace Core.Input
{
    public class InputManager : MonoBehaviour
    {
        private GameInputActions inputActions;
        private Vector2 moveInput;
        private bool isInitialized;

        #region Unity Lifecycle
        private void Awake()
        {
            InitializeInputActions();
        }

        private void OnEnable()
        {
            if (inputActions != null)
            {
                EnableInputActions();
            }
        }

        private void OnDisable()
        {
            if (inputActions != null)
            {
                DisableInputActions();
            }
        }

        private void Update()
        {
            if (!isInitialized) return;
            
            // Process movement input
            ProcessMovementInput();
        }
        #endregion

        #region Initialization
        private void InitializeInputActions()
        {
            inputActions = new GameInputActions();
            
            // Movement
            inputActions.Player.Move.performed += OnMovePerformed;
            inputActions.Player.Move.canceled += OnMoveCanceled;
            
            // Shooting
            inputActions.Player.StraightShoot.performed += OnStraightShootPerformed;
            inputActions.Player.LeftAngleShoot.performed += OnLeftAngleShootPerformed;
            inputActions.Player.RightAngleShoot.performed += OnRightAngleShootPerformed;
            inputActions.Player.ToggleAngle.performed += OnToggleAnglePerformed;
            
            // Bullet Level
            inputActions.Player.ToggleBulletLevel.performed += OnToggleBulletLevelPerformed;
            inputActions.Player.FireLevel3Bullet.performed += OnFireLevel3BulletPerformed;
            
            // Touch Input
            inputActions.Touch.PrimaryFingerPosition.performed += OnPrimaryFingerPositionPerformed;
            inputActions.Touch.SecondaryFingerPosition.performed += OnSecondaryFingerPositionPerformed;

            EnableInputActions();
            isInitialized = true;
        }

        private void EnableInputActions()
        {
            inputActions.Enable();
        }

        private void DisableInputActions()
        {
            inputActions.Disable();
        }
        #endregion

        #region Input Handlers
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            moveInput = Vector2.zero;
        }

        private void OnStraightShootPerformed(InputAction.CallbackContext context)
        {
            EventManager.Instance.TriggerEvent("OnStraightShoot");
        }

        private void OnLeftAngleShootPerformed(InputAction.CallbackContext context)
        {
            EventManager.Instance.TriggerEvent("OnLeftAngleShoot");
        }

        private void OnRightAngleShootPerformed(InputAction.CallbackContext context)
        {
            EventManager.Instance.TriggerEvent("OnRightAngleShoot");
        }

        private void OnToggleAnglePerformed(InputAction.CallbackContext context)
        {
            EventManager.Instance.TriggerEvent("OnToggleAngle");
        }

        private void OnToggleBulletLevelPerformed(InputAction.CallbackContext context)
        {
            EventManager.Instance.TriggerEvent("OnToggleBulletLevel");
        }

        private void OnFireLevel3BulletPerformed(InputAction.CallbackContext context)
        {
            EventManager.Instance.TriggerEvent("OnFireLevel3Bullet");
        }

        private void OnPrimaryFingerPositionPerformed(InputAction.CallbackContext context)
        {
            Vector2 position = context.ReadValue<Vector2>();
            EventManager.Instance.TriggerEvent("OnPrimaryFingerPosition", position);
        }

        private void OnSecondaryFingerPositionPerformed(InputAction.CallbackContext context)
        {
            Vector2 position = context.ReadValue<Vector2>();
            EventManager.Instance.TriggerEvent("OnSecondaryFingerPosition", position);
        }
        #endregion

        #region Input Processing
        private void ProcessMovementInput()
        {
            if (moveInput != Vector2.zero)
            {
                EventManager.Instance.TriggerEvent("OnMove", moveInput);
            }
        }
        #endregion

        #region Public Methods
        public Vector2 GetMoveInput() => moveInput;
        public bool IsInitialized() => isInitialized;
        #endregion
    }
}
