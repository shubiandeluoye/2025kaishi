using UnityEngine;
using Core.Movement.Base;

namespace Core.Movement.Types
{
    /// <summary>
    /// Implements directional movement with support for both 8-direction and free movement
    /// </summary>
    public class DirectionalMovement : BaseMovement
    {
        public enum MovementType
        {
            EightDirectional,
            FreeDirectional
        }

        [SerializeField]
        private MovementType movementType = MovementType.EightDirectional;

        [SerializeField]
        private bool useLocalSpace = false;

        [SerializeField]
        private Transform orientationReference;

        private Vector3 rawInputDirection;
        private Vector3 lastValidDirection;

        public float Speed
        {
            get => settings.maxSpeed;
            set => settings.maxSpeed = value;
        }

        public float RotationSpeed
        {
            get => settings.rotationSpeed;
            set => settings.rotationSpeed = value;
        }

        protected override void Awake()
        {
            base.Awake();
            if (orientationReference == null)
            {
                orientationReference = Camera.main?.transform;
            }
        }

        public void Move(Vector2 direction)
        {
            SetInputDirection(direction);
        }

        /// <summary>
        /// Set raw input direction (before processing)
        /// </summary>
        public void SetInputDirection(Vector2 input)
        {
            rawInputDirection = new Vector3(input.x, 0, input.y);
            ProcessMovementInput();
        }

        /// <summary>
        /// Process input based on movement type and constraints
        /// </summary>
        protected virtual void ProcessMovementInput()
        {
            if (rawInputDirection.sqrMagnitude < 0.01f)
            {
                SetMoveDirection(Vector3.zero);
                return;
            }

            Vector3 worldSpaceDirection = GetWorldSpaceDirection();
            Vector3 constrainedDirection = movementType == MovementType.EightDirectional 
                ? ConstrainToEightDirections(worldSpaceDirection) 
                : worldSpaceDirection;

            if (constrainedDirection != Vector3.zero)
            {
                lastValidDirection = constrainedDirection;
            }

            SetMoveDirection(constrainedDirection);

            var data = new EventManager.DirectionalMovementEventData(
                transform.position,
                constrainedDirection,
                currentVelocity.magnitude,
                isGrounded,
                useLocalSpace,
                movementType,
                rawInputDirection
            );
            EventManager.Publish(EventManager.EventNames.MOVEMENT_DIRECTION_CHANGED, data);
        }

        /// <summary>
        /// Convert input to world space direction considering orientation reference
        /// </summary>
        protected virtual Vector3 GetWorldSpaceDirection()
        {
            if (useLocalSpace || orientationReference == null)
            {
                return rawInputDirection;
            }

            // Convert input direction to be relative to camera orientation
            Vector3 forward = orientationReference.forward;
            Vector3 right = orientationReference.right;
            
            // Project vectors onto the horizontal plane
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return (forward * rawInputDirection.z + right * rawInputDirection.x).normalized;
        }

        /// <summary>
        /// Constrain movement to 8 cardinal/diagonal directions
        /// </summary>
        protected virtual Vector3 ConstrainToEightDirections(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.01f) return Vector3.zero;

            // Convert to angle
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
            // Round to nearest 45 degrees
            angle = Mathf.Round(angle / 45f) * 45f;
            
            // Convert back to vector
            return new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0,
                Mathf.Cos(angle * Mathf.Deg2Rad)
            );
        }

        /// <summary>
        /// Set movement type at runtime
        /// </summary>
        public void SetMovementType(MovementType type)
        {
            if (movementType != type)
            {
                movementType = type;
                ProcessMovementInput(); // Reprocess current input with new constraints
            }
        }

        /// <summary>
        /// Set orientation reference (usually the camera)
        /// </summary>
        public void SetOrientationReference(Transform reference)
        {
            orientationReference = reference;
        }

        /// <summary>
        /// Toggle between local and world space input processing
        /// </summary>
        public void SetUseLocalSpace(bool useLocal)
        {
            if (useLocalSpace != useLocal)
            {
                useLocalSpace = useLocal;
                ProcessMovementInput();
            }
        }

        /// <summary>
        /// Get the last valid movement direction
        /// </summary>
        public Vector3 GetLastValidDirection()
        {
            return lastValidDirection;
        }

        /// <summary>
        /// Get current movement type
        /// </summary>
        public MovementType GetMovementType()
        {
            return movementType;
        }

        /// <summary>
        /// Check if using local space for input processing
        /// </summary>
        public bool IsUsingLocalSpace()
        {
            return useLocalSpace;
        }
    }
}
