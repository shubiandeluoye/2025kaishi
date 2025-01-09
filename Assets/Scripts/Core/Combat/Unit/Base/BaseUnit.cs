using UnityEngine;
using System;
using System.Collections.Generic;
using Core.Base.Event;

namespace Core.Combat.Unit.Base
{
    /// <summary>
    /// Base class for all game units (players, enemies, etc.)
    /// Implements core unit functionality including stats, status effects, and collision
    /// </summary>
    public abstract class BaseUnit : MonoBehaviour
    {
        #region Stats
        [Serializable]
        public class UnitStats
        {
            public float maxHealth = 100f;
            public float currentHealth;
            public float moveSpeed = 5f;
            public float defense = 10f;
            public float attack = 10f;
            
            public void Initialize()
            {
                currentHealth = maxHealth;
            }
        }

        [SerializeField]
        protected UnitStats stats = new UnitStats();
        
        public float CurrentHealth => stats.currentHealth;
        public float MaxHealth => stats.maxHealth;
        public float MoveSpeed => stats.moveSpeed;
        public float Defense => stats.defense;
        public float Attack => stats.attack;
        #endregion

        #region Status Effects
        protected Dictionary<string, StatusEffect> activeStatusEffects = new Dictionary<string, StatusEffect>();
        
        [Serializable]
        public class StatusEffect
        {
            public string id;
            public string name;
            public float duration;
            public float remainingTime;
            public Dictionary<string, float> statModifiers = new Dictionary<string, float>();
            
            public bool IsExpired => remainingTime <= 0;
            
            public void Update(float deltaTime)
            {
                if (remainingTime > 0)
                {
                    remainingTime -= deltaTime;
                }
            }
        }
        #endregion

        #region Events
        public event Action<float> OnHealthChanged;
        public event Action<StatusEffect> OnStatusEffectAdded;
        public event Action<StatusEffect> OnStatusEffectRemoved;
        public event Action OnDeath;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            stats.Initialize();
        }

        protected virtual void Update()
        {
            UpdateStatusEffects();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }
        #endregion

        #region Health Management
        public virtual void TakeDamage(float amount, BaseUnit source = null)
        {
            float finalDamage = CalculateDamage(amount, source);
            stats.currentHealth = Mathf.Max(0, stats.currentHealth - finalDamage);
            
            OnHealthChanged?.Invoke(stats.currentHealth);
            
            if (stats.currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Heal(float amount)
        {
            stats.currentHealth = Mathf.Min(stats.maxHealth, stats.currentHealth + amount);
            OnHealthChanged?.Invoke(stats.currentHealth);
        }

        protected virtual float CalculateDamage(float amount, BaseUnit source)
        {
            // Basic damage calculation with defense
            float damage = amount * (100 / (100 + stats.defense));
            return damage;
        }

        protected virtual void Die()
        {
            OnDeath?.Invoke();
            // Override in derived classes for specific death behavior
        }
        #endregion

        #region Status Effect Management
        public virtual void AddStatusEffect(StatusEffect effect)
        {
            if (activeStatusEffects.ContainsKey(effect.id))
            {
                RemoveStatusEffect(effect.id);
            }
            
            activeStatusEffects[effect.id] = effect;
            ApplyStatusEffectModifiers(effect);
            OnStatusEffectAdded?.Invoke(effect);
        }

        public virtual void RemoveStatusEffect(string effectId)
        {
            if (activeStatusEffects.TryGetValue(effectId, out StatusEffect effect))
            {
                RemoveStatusEffectModifiers(effect);
                activeStatusEffects.Remove(effectId);
                OnStatusEffectRemoved?.Invoke(effect);
            }
        }

        protected virtual void UpdateStatusEffects()
        {
            List<string> expiredEffects = new List<string>();
            
            foreach (var effect in activeStatusEffects.Values)
            {
                effect.Update(Time.deltaTime);
                if (effect.IsExpired)
                {
                    expiredEffects.Add(effect.id);
                }
            }
            
            foreach (var effectId in expiredEffects)
            {
                RemoveStatusEffect(effectId);
            }
        }

        protected virtual void ApplyStatusEffectModifiers(StatusEffect effect)
        {
            // Override in derived classes to apply specific stat modifications
        }

        protected virtual void RemoveStatusEffectModifiers(StatusEffect effect)
        {
            // Override in derived classes to remove specific stat modifications
        }
        #endregion

        #region Collision Handling
        protected virtual void HandleCollision(Collision collision)
        {
            // Override in derived classes for specific collision behavior
        }
        #endregion

        #region Stats Modification
        public virtual void ModifyStat(string statName, float modifier, bool isMultiplicative = false)
        {
            // Override in derived classes to handle specific stat modifications
        }
        #endregion
    }
}
