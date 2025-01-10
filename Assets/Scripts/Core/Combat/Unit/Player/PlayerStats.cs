using UnityEngine;
using Core.Base.Event;
using Core.Combat.Unit.Base;

namespace Core.Combat.Unit.Player
{
    public class PlayerStats : BaseUnit
    {
        [Header("Score Settings")]
        [SerializeField] private int maxScore = 100;
        private int currentScore;

        [Header("Combat Settings")]
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            currentScore = maxScore;
            currentHealth = maxHealth;
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            EventManager.Subscribe<DamageEvent>(EventNames.DAMAGE_TAKEN, OnDamageTaken);
            EventManager.Subscribe<BulletHitEvent>(EventNames.BULLET_HIT, OnBulletHit);
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();
            EventManager.Unsubscribe<DamageEvent>(EventNames.DAMAGE_TAKEN, OnDamageTaken);
            EventManager.Unsubscribe<BulletHitEvent>(EventNames.BULLET_HIT, OnBulletHit);
        }

        private void OnDamageTaken(DamageEvent evt)
        {
            if (evt.Target != gameObject) return;
            TakeDamage(evt.Damage);
        }

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Max(0f, currentHealth - damage);
            
            EventManager.Publish(EventNames.PLAYER_HEALTH_CHANGED, 
                new PlayerHealthEvent(currentHealth, maxHealth));
            
            if (currentHealth <= 0f)
            {
                EventManager.Publish(EventNames.PLAYER_DEFEATED, 
                    new PlayerDefeatedEvent(transform.position));
            }
        }

        private void OnBulletHit(BulletHitEvent evt)
        {
            if (evt.Target != gameObject) return;
            
            currentScore = Mathf.Max(0, currentScore - 1);
            
            EventManager.Publish(EventNames.PLAYER_SCORE_CHANGED, 
                new PlayerScoreEvent(currentScore, maxScore));
            
            if (currentScore <= 0)
            {
                EventManager.Publish(EventNames.PLAYER_DEFEATED, 
                    new PlayerDefeatedEvent(transform.position));
            }
        }

        public int CurrentScore => currentScore;
        public float CurrentHealth => currentHealth;
    }

    // 事件类定义
    public class PlayerHealthEvent
    {
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        public PlayerHealthEvent(float current, float max)
        {
            CurrentHealth = current;
            MaxHealth = max;
        }
    }

    public class PlayerScoreEvent
    {
        public int CurrentScore { get; private set; }
        public int MaxScore { get; private set; }

        public PlayerScoreEvent(int current, int max)
        {
            CurrentScore = current;
            MaxScore = max;
        }
    }

    public class PlayerDefeatedEvent
    {
        public Vector3 Position { get; private set; }

        public PlayerDefeatedEvent(Vector3 position)
        {
            Position = position;
        }
    }

    public class DamageEvent
    {
        public GameObject Target { get; private set; }
        public float Damage { get; private set; }

        public DamageEvent(GameObject target, float damage)
        {
            Target = target;
            Damage = damage;
        }
    }

    public class BulletHitEvent
    {
        public GameObject Target { get; private set; }
        public Vector3 HitPoint { get; private set; }

        public BulletHitEvent(GameObject target, Vector3 hitPoint)
        {
            Target = target;
            HitPoint = hitPoint;
        }
    }
}
