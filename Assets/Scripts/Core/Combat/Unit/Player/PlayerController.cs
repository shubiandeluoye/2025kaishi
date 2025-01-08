using UnityEngine;
using Core.Combat.Unit.Base;
using Core.Movement.Types;
using Core.Skills.Shooting;

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

        private void HandleMovement()
        {
            if (movement == null) return;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (direction.magnitude >= 0.1f)
            {
                movement.Move(direction);
            }
        }

        private void HandleSkills()
        {
            if (skillManager == null) return;

            Vector3 aimDirection = transform.forward;

            if (Input.GetMouseButtonDown(0) && skillManager.IsSkillReady(0))
            {
                skillManager.ExecuteSkill(0, aimDirection);
            }
            else if (Input.GetMouseButtonDown(1) && skillManager.IsSkillReady(1))
            {
                skillManager.ExecuteSkill(1, aimDirection);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && skillManager.IsSkillReady(2))
            {
                skillManager.ExecuteSkill(2, aimDirection);
            }
            else if (Input.GetKeyDown(KeyCode.E) && skillManager.IsSkillReady(3))
            {
                skillManager.ExecuteSkill(3, aimDirection);
            }
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
