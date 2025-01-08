using UnityEngine;

namespace Core.Movement.Base
{
    public abstract class BaseMovement : MonoBehaviour
    {
        [SerializeField]
        protected float speed = 5f;
        
        [SerializeField]
        protected float rotationSpeed = 360f;
        
        protected Vector3 moveDirection;
        protected bool isMoving;
        
        public float Speed
        {
            get => speed;
            set => speed = value;
        }
        
        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }
        
        protected virtual void Awake()
        {
            moveDirection = Vector3.zero;
            isMoving = false;
        }
        
        public virtual void Move(Vector3 direction)
        {
            moveDirection = direction.normalized;
            isMoving = moveDirection.sqrMagnitude > 0.01f;
            
            if (isMoving)
            {
                // Apply movement
                transform.position += moveDirection * (speed * Time.deltaTime);
                
                // Apply rotation
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
        
        public virtual void Stop()
        {
            moveDirection = Vector3.zero;
            isMoving = false;
        }
        
        public bool IsMoving() => isMoving;
        public Vector3 GetMoveDirection() => moveDirection;
    }
}
