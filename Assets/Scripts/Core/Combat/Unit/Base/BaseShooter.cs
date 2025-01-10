using UnityEngine;
using Core.Base.Manager;
using Core.Base.Event;

namespace Core.Combat.Unit.Base
{
    public class BaseShooter : BaseManager
    {
        [Header("Shooting Settings")]
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected float shootForce = 20f;
        [SerializeField] protected float shootCooldown = 0.5f;
        
        protected float lastShootTime;

        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            lastShootTime = -shootCooldown;
        }

        public virtual void Shoot(Vector3 direction)
        {
            if (Time.time - lastShootTime < shootCooldown) return;
            
            if (bulletPrefab != null && shootPoint != null)
            {
                EventManager.Publish(EventNames.BULLET_SPAWN, 
                    new BulletSpawnEvent(
                        bulletPrefab,
                        shootPoint.position,
                        direction,
                        shootForce,
                        10f, // 基础伤害
                        GetComponent<BaseUnit>(),
                        GetComponent<ITeamMember>()?.Team
                    ));
                
                lastShootTime = Time.time;
                
                EventManager.Publish(EventNames.BULLET_FIRED, 
                    new BulletFiredEvent(null, shootPoint.position, direction, shootForce));
            }
        }
    }
}
