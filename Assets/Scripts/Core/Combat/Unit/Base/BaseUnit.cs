using UnityEngine;
using Core.Base.Manager;
using Core.Base.Event;

namespace Core.Combat.Unit.Base
{
    public abstract class BaseUnit : BaseManager
    {
        [Header("Unit Settings")]
        [SerializeField] protected float moveSpeed = 5f;
        [SerializeField] protected float rotateSpeed = 360f;
        
        protected bool isAlive = true;
        protected Vector3 moveDirection;
        protected Vector3 lookDirection;

        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            moveDirection = Vector3.zero;
            lookDirection = transform.forward;
        }

        protected virtual void Update()
        {
            if (!isAlive) return;
            
            HandleMovement();
            HandleRotation();
        }

        protected virtual void HandleMovement()
        {
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                EventManager.Publish(EventNames.UNIT_MOVED, 
                    new UnitMovedEvent(gameObject, transform.position));
            }
        }

        protected virtual void HandleRotation()
        {
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime
                );
                
                EventManager.Publish(EventNames.UNIT_ROTATED, 
                    new UnitRotatedEvent(gameObject, transform.rotation));
            }
        }

        public virtual void SetMoveDirection(Vector3 direction)
        {
            moveDirection = direction.normalized;
            EventManager.Publish(EventNames.UNIT_DIRECTION_CHANGED, 
                new UnitDirectionEvent(gameObject, direction, true));
        }

        public virtual void SetLookDirection(Vector3 direction)
        {
            lookDirection = direction.normalized;
            EventManager.Publish(EventNames.UNIT_DIRECTION_CHANGED, 
                new UnitDirectionEvent(gameObject, direction, false));
        }
    }

    public class UnitMovedEvent
    {
        public GameObject Unit { get; private set; }
        public Vector3 Position { get; private set; }

        public UnitMovedEvent(GameObject unit, Vector3 position)
        {
            Unit = unit;
            Position = position;
        }
    }

    public class UnitRotatedEvent
    {
        public GameObject Unit { get; private set; }
        public Quaternion Rotation { get; private set; }

        public UnitRotatedEvent(GameObject unit, Quaternion rotation)
        {
            Unit = unit;
            Rotation = rotation;
        }
    }
}
