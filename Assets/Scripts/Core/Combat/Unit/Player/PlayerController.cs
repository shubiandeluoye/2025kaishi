using UnityEngine;
using Core.Combat.Unit.Base;
using Core.Movement.Types;
using Core.Skills.Shooting;
using Core.Base.Event;

namespace Core.Combat.Unit.Player
{
    public class PlayerController : BaseUnit
    {
        [Header("Components")]
        [SerializeField] private DirectionalMovement movement;
        [SerializeField] private BaseShooter shooter;
        [SerializeField] private SkillManager skillManager;
        
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 360f;
        
        protected override void Awake()
        {
            base.Awake();
            
            movement = GetComponent<DirectionalMovement>();
            shooter = GetComponent<BaseShooter>();
            skillManager = GetComponent<SkillManager>();

            if (movement != null)
            {
                movement.Speed = moveSpeed;
                movement.RotationSpeed = rotationSpeed;
            }
        }

        protected virtual void Update()
        {
            HandleMovement();
            HandleSkills();
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener("OnMove", OnMove);
            EventManager.Instance.AddListener("OnStraightShoot", OnStraightShoot);
            EventManager.Instance.AddListener("OnLeftAngleShoot", OnLeftAngleShoot);
            EventManager.Instance.AddListener("OnRightAngleShoot", OnRightAngleShoot);
            EventManager.Instance.AddListener("OnToggleAngle", OnToggleAngle);
            EventManager.Instance.AddListener("OnToggleBulletLevel", OnToggleBulletLevel);
            EventManager.Instance.AddListener("OnFireLevel3Bullet", OnFireLevel3Bullet);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener("OnMove", OnMove);
            EventManager.Instance.RemoveListener("OnStraightShoot", OnStraightShoot);
            EventManager.Instance.RemoveListener("OnLeftAngleShoot", OnLeftAngleShoot);
            EventManager.Instance.RemoveListener("OnRightAngleShoot", OnRightAngleShoot);
            EventManager.Instance.RemoveListener("OnToggleAngle", OnToggleAngle);
            EventManager.Instance.RemoveListener("OnToggleBulletLevel", OnToggleBulletLevel);
            EventManager.Instance.RemoveListener("OnFireLevel3Bullet", OnFireLevel3Bullet);
        }

        private void HandleMovement()
        {
            // Movement is now handled through events
        }

        private void HandleSkills()
        {
            // Skills are now handled through events
        }

        private void OnMove(object moveInput)
        {
            if (movement == null || !(moveInput is Vector2)) return;
            Vector2 input = (Vector2)moveInput;
            Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
            
            if (direction.magnitude >= 0.1f)
            {
                movement.Move(direction);
            }
        }

        private void OnStraightShoot()
        {
            if (skillManager == null || !skillManager.IsSkillReady(0)) return;
            skillManager.ExecuteSkill(0, transform.forward);
        }

        private void OnLeftAngleShoot()
        {
            if (skillManager == null || !skillManager.IsSkillReady(1)) return;
            skillManager.ExecuteSkill(1, transform.forward);
        }

        private void OnRightAngleShoot()
        {
            if (skillManager == null || !skillManager.IsSkillReady(2)) return;
            skillManager.ExecuteSkill(2, transform.forward);
        }

        private void OnToggleAngle()
        {
            // Implement angle toggle logic
        }

        private void OnToggleBulletLevel()
        {
            // Implement bullet level toggle logic
        }

        private void OnFireLevel3Bullet()
        {
            if (skillManager == null || !skillManager.IsSkillReady(3)) return;
            skillManager.ExecuteSkill(3, transform.forward);
        }

        public void OnMobileMove(Vector2 input)
        {
            if (movement == null) return;
            Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
            if (direction.magnitude >= 0.1f)
            {
                movement.Move(direction);
            }
        }

        public void OnMobileSkill(int skillIndex)
        {
            if (skillManager == null) return;
            if (skillManager.IsSkillReady(skillIndex))
            {
                skillManager.ExecuteSkill(skillIndex, transform.forward);
            }
        }
    }
}
