using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Movement.Base
{
    /// <summary>
    /// Base class for all movement behaviors
    /// Provides core movement functionality and interfaces with physics system
    /// </summary>
    public abstract class BaseMovement : MonoBehaviour
    {
        #region Movement Properties
        [Serializable]
        public class MovementSettings
        {
            public float maxSpeed = 10f;
            public float acceleration = 50f;
            public float deceleration = 50f;
            public float rotationSpeed = 360f;
            public bool useRootMotion = false;
            public bool orientToMovement = true;
        }

        [SerializeField]
        protected MovementSettings settings = new MovementSettings();

        protected Vector3 currentVelocity;
        protected Vector3 targetVelocity;
        protected Vector3 moveDirection;
        protected bool isMoving;
        
        protected Rigidbody rb;
        protected Animator animator;
        #endregion

        #region Ground Check
        [Serializable]
        public class GroundSettings
        {
            public float groundCheckDistance = 0.2f;
            public LayerMask groundLayer = -1;
            public float slopeLimit = 45f;
        }

        [SerializeField]
        protected GroundSettings groundSettings = new GroundSettings();

        protected bool isGrounded;
        protected RaycastHit groundHit;
        #endregion

        #region Events
        public event Action<Vector3> OnMoveStart;
        public event Action OnMoveEnd;
        public event Action<bool> OnGroundedStateChanged;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        protected virtual void Update()
        {
            UpdateGroundCheck();
            UpdateMovement();
            UpdateRotation();
            UpdateAnimation();
        }

        protected virtual void FixedUpdate()
        {
            if (rb != null)
            {
                ApplyMovement();
            }
        }
        #endregion

        #region Movement Control
        public virtual void SetMoveDirection(Vector3 direction)
        {
            moveDirection = direction.normalized;
            targetVelocity = moveDirection * settings.maxSpeed;
            
            bool wasMoving = isMoving;
            isMoving = moveDirection.sqrMagnitude > 0.01f;
            
            if (isMoving && !wasMoving)
            {
                OnMoveStart?.Invoke(moveDirection);
            }
            else if (!isMoving && wasMoving)
            {
                OnMoveEnd?.Invoke();
            }
        }

        protected virtual void UpdateMovement()
        {
            if (settings.useRootMotion && animator != null)
            {
                // Movement will be handled by animation root motion
                return;
            }

            float accelerationRate = isMoving ? settings.acceleration : settings.deceleration;
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                accelerationRate * Time.deltaTime
            );
        }

        protected virtual void ApplyMovement()
        {
            if (settings.useRootMotion) return;

            Vector3 movement = currentVelocity * Time.fixedDeltaTime;
            
            // Apply slope adjustment if needed
            if (isGrounded && groundHit.normal != Vector3.up)
            {
                movement = AdjustDirectionToSlope(movement);
            }
            
            rb.MovePosition(rb.position + movement);
        }

        protected virtual Vector3 AdjustDirectionToSlope(Vector3 direction)
        {
            float angle = Vector3.Angle(groundHit.normal, Vector3.up);
            if (angle > groundSettings.slopeLimit)
            {
                return Vector3.zero;
            }

            return Vector3.ProjectOnPlane(direction, groundHit.normal);
        }
        #endregion

        #region Rotation Control
        protected virtual void UpdateRotation()
        {
            if (!settings.orientToMovement || !isMoving) return;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                settings.rotationSpeed * Time.deltaTime
            );
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public virtual void SetRotation(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        #endregion

        #region Ground Check
        protected virtual void UpdateGroundCheck()
        {
            bool wasGrounded = isGrounded;
            
            isGrounded = Physics.Raycast(
                transform.position + Vector3.up * 0.1f,
                Vector3.down,
                out groundHit,
                groundSettings.groundCheckDistance + 0.1f,
                groundSettings.groundLayer
            );

            if (wasGrounded != isGrounded)
            {
                OnGroundedStateChanged?.Invoke(isGrounded);
            }
        }
        #endregion

        #region Animation
        protected virtual void UpdateAnimation()
        {
            if (animator == null) return;

            animator.SetFloat("Speed", currentVelocity.magnitude / settings.maxSpeed);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb != null ? rb.velocity.y : 0);
        }
        #endregion

        #region Utility Methods
        public virtual void Stop()
        {
            SetMoveDirection(Vector3.zero);
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }

        public virtual void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            if (rb != null)
            {
                rb.AddForce(force, mode);
            }
        }

        public virtual void SetVelocity(Vector3 velocity)
        {
            if (rb != null)
            {
                rb.velocity = velocity;
            }
        }
        #endregion

        #region Getters
        public bool IsGrounded() => isGrounded;
        public bool IsMoving() => isMoving;
        public Vector3 GetVelocity() => currentVelocity;
        public Vector3 GetMoveDirection() => moveDirection;
        public float GetCurrentSpeed() => currentVelocity.magnitude;
        public float GetMaxSpeed() => settings.maxSpeed;
        #endregion
    }
}
