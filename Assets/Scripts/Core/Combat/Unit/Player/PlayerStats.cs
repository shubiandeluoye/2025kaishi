using UnityEngine;
using Core.Combat.Unit.Base;
using Core.Skills.Base;
using Core.Skills.Shooting;
using UnityEngine.Events;

namespace Core.Combat.Unit.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Score Settings")]
        [SerializeField] private int maxScore = 100;
        private int currentScore;

        [Header("Combat Settings")]
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        [Header("Events")]
        public UnityEvent<int> onScoreChanged;
        public UnityEvent<float> onHealthChanged;
        public UnityEvent onPlayerDefeated;

        private void Awake()
        {
            currentScore = maxScore;
            currentHealth = maxHealth;
            
            if (onScoreChanged == null) onScoreChanged = new UnityEvent<int>();
            if (onHealthChanged == null) onHealthChanged = new UnityEvent<float>();
            if (onPlayerDefeated == null) onPlayerDefeated = new UnityEvent();
        }

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Max(0f, currentHealth - damage);
            onHealthChanged?.Invoke(currentHealth);
            
            if (currentHealth <= 0f)
            {
                onPlayerDefeated?.Invoke();
            }
        }

        public void OnBulletHit()
        {
            currentScore = Mathf.Max(0, currentScore - 1);
            onScoreChanged?.Invoke(currentScore);
            
            if (currentScore <= 0)
            {
                onPlayerDefeated?.Invoke();
            }
        }

        public int CurrentScore => currentScore;
        public float CurrentHealth => currentHealth;
    }
}
