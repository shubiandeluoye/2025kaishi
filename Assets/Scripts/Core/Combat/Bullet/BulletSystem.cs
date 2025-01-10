using UnityEngine;
using Core.Base.Manager;
using Core.Base.Event;
using Core.Combat.Unit.Base;
using Core.Combat.Team;

namespace Core.Combat.Bullet
{
    public class BulletSystem : BaseManager
    {
        protected override void RegisterEvents()
        {
            EventManager.Subscribe<BulletSpawnEvent>(EventNames.BULLET_SPAWN, OnBulletSpawn);
            EventManager.Subscribe<BulletHitEvent>(EventNames.BULLET_HIT, OnBulletHit);
            EventManager.Subscribe<BulletDestroyedEvent>(EventNames.BULLET_DESTROYED, OnBulletDestroyed);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<BulletSpawnEvent>(EventNames.BULLET_SPAWN, OnBulletSpawn);
            EventManager.Unsubscribe<BulletHitEvent>(EventNames.BULLET_HIT, OnBulletHit);
            EventManager.Unsubscribe<BulletDestroyedEvent>(EventNames.BULLET_DESTROYED, OnBulletDestroyed);
        }

        private void OnBulletSpawn(BulletSpawnEvent evt)
        {
            if (evt.BulletPrefab != null)
            {
                var bullet = Instantiate(evt.BulletPrefab, evt.Position, Quaternion.LookRotation(evt.Direction));
                if (bullet.TryGetComponent<BaseBullet>(out var baseBullet))
                {
                    baseBullet.Initialize(evt.Direction, evt.Speed, evt.Damage, evt.Owner, evt.Team);
                }
            }
        }

        private void OnBulletHit(BulletHitEvent evt)
        {
            if (evt.Target != null)
            {
                EventManager.Publish(EventNames.DAMAGE_TAKEN, 
                    new DamageEvent(evt.Target, evt.Damage));
            }
        }

        private void OnBulletDestroyed(BulletDestroyedEvent evt)
        {
            if (evt.Bullet != null)
            {
                Destroy(evt.Bullet.gameObject);
            }
        }
    }

    // 事件数据类
    public class BulletSpawnEvent
    {
        public GameObject BulletPrefab { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }
        public float Damage { get; private set; }
        public BaseUnit Owner { get; private set; }
        public TeamType Team { get; private set; }

        public BulletSpawnEvent(GameObject prefab, Vector3 pos, Vector3 dir, float speed, float damage, BaseUnit owner = null, TeamType team = TeamType.System)
        {
            BulletPrefab = prefab;
            Position = pos;
            Direction = dir;
            Speed = speed;
            Damage = damage;
            Owner = owner;
            Team = team;
        }
    }

    public class BulletHitEvent
    {
        public GameObject Target { get; private set; }
        public float Damage { get; private set; }
        public Vector3 HitPoint { get; private set; }

        public BulletHitEvent(GameObject target, float damage, Vector3 hitPoint)
        {
            Target = target;
            Damage = damage;
            HitPoint = hitPoint;
        }
    }

    public class BulletDestroyedEvent
    {
        public BaseBullet Bullet { get; private set; }
        public Vector3 Position { get; private set; }

        public BulletDestroyedEvent(BaseBullet bullet, Vector3 position)
        {
            Bullet = bullet;
            Position = position;
        }
    }
} 