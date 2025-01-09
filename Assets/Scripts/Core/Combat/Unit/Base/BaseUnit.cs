using UnityEngine;
using System;

namespace Core.Combat.Unit.Base
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [Header("Unit Stats")]
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth;

        public event Action<float> OnHealthChanged;
        public event Action OnDeath;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            if (currentHealth <= 0) return;

            currentHealth = Mathf.Max(0, currentHealth - damage);
            OnHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Heal(float amount)
        {
            if (currentHealth <= 0) return;

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth);
        }

        protected virtual void Die()
        {
            OnDeath?.Invoke();
        }
    }
}
