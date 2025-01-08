using UnityEngine;
using Core.Movement.Base;

namespace Core.Movement.Types
{
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

        protected override void Awake()
        {
            base.Awake();
            if (orientationReference == null)
            {
                orientationReference = Camera.main?.transform;
            }
        }

        public void SetInputDirection(Vector2 input)
        {
            rawInputDirection = new Vector3(input.x, 0, input.y);
            ProcessMovementInput();
        }

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
        }

        protected virtual Vector3 GetWorldSpaceDirection()
        {
            if (useLocalSpace || orientationReference == null)
            {
                return rawInputDirection;
            }

            Vector3 forward = orientationReference.forward;
            Vector3 right = orientationReference.right;
            
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return (forward * rawInputDirection.z + right * rawInputDirection.x).normalized;
        }

        protected virtual Vector3 ConstrainToEightDirections(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.01f) return Vector3.zero;

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            angle = Mathf.Round(angle / 45f) * 45f;
            
            return new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0,
                Mathf.Cos(angle * Mathf.Deg2Rad)
            );
        }

        protected virtual void SetMoveDirection(Vector3 direction)
        {
            moveDirection = direction;
            isMoving = moveDirection.sqrMagnitude > 0.01f;
        }

        public void SetMovementType(MovementType type)
        {
            if (movementType != type)
            {
                movementType = type;
                ProcessMovementInput();
            }
        }

        public void SetOrientationReference(Transform reference)
        {
            orientationReference = reference;
        }

        public void SetUseLocalSpace(bool useLocal)
        {
            if (useLocalSpace != useLocal)
            {
                useLocalSpace = useLocal;
                ProcessMovementInput();
            }
        }

        public Vector3 GetLastValidDirection()
        {
            return lastValidDirection;
        }

        public MovementType GetMovementType()
        {
            return movementType;
        }

        public bool IsUsingLocalSpace()
        {
            return useLocalSpace;
        }
    }
}
