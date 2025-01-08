using UnityEngine;
using Core.Movement.Base;

namespace Core.Movement.Types
{
    public class DirectionalMovement : BaseMovement
    {
        [SerializeField] private bool useEightDirections = true;
        [SerializeField] private float moveSpeed = 5f;

        private Vector3 moveDirection;
        private Transform cachedTransform;

        protected override void Awake()
        {
            base.Awake();
            cachedTransform = transform;
        }

        public override void SetDirection(Vector3 direction)
        {
            if (useEightDirections)
            {
                // Snap to 8 directions
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle = Mathf.Round(angle / 45) * 45;
                moveDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized;
            }
            else
            {
                moveDirection = direction.normalized;
            }
        }

        public override void Move()
        {
            if (moveDirection != Vector3.zero)
            {
                cachedTransform.position += moveDirection * (moveSpeed * Time.deltaTime);
            }
        }

        public override void Stop()
        {
            moveDirection = Vector3.zero;
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }
    }
}
