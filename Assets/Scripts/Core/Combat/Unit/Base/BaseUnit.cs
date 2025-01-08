using UnityEngine;
using System;

namespace Core.Combat.Unit.Base
{
    /// <summary>
    /// Base class for all units in the game, handling health, damage, and basic unit functionality
    /// </summary>
    public class BaseUnit : MonoBehaviour
    {
        #region Properties
        [Header("Unit Stats")]
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected bool isInvulnerable = false;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public bool IsAlive => currentHealth > 0;
        public bool IsInvulnerable => isInvulnerable;
        #endregion

        #region Events
        public event Action<float> OnHealthChanged;
        public event Action<BaseUnit> OnDeath;
        public event Action<float, BaseUnit> OnDamageTaken;
        public event Action<float> OnHeal;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }
        #endregion

        #region Health Management
        public virtual void TakeDamage(float damage, BaseUnit source = null)
        {
            if (isInvulnerable || !IsAlive || damage <= 0) return;

            currentHealth = Mathf.Max(0, currentHealth - damage);
            OnDamageTaken?.Invoke(damage, source);
            OnHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Heal(float amount)
        {
            if (!IsAlive || amount <= 0) return;

            float oldHealth = currentHealth;
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            float actualHeal = currentHealth - oldHealth;

            if (actualHeal > 0)
            {
                OnHeal?.Invoke(actualHeal);
                OnHealthChanged?.Invoke(currentHealth);
            }
        }

        public virtual void RestoreFullHealth()
        {
            if (!IsAlive) return;
            Heal(maxHealth - currentHealth);
        }

        protected virtual void Die()
        {
            OnDeath?.Invoke(this);
        }
        #endregion

        #region Status Management
        public virtual void SetInvulnerable(bool state)
        {
            isInvulnerable = state;
        }
        #endregion
    }
}
