using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

namespace Core.Skills.Shooting
{
    public class BouncingShootingSkill : BaseSkill
    {
        [SerializeField] private BaseBullet bulletPrefab;
        [SerializeField] private float bulletSpeed = 8f;
        [SerializeField] private int maxBounces = 3;

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || !owner) return;

            var shooter = owner as BaseShooter;
            if (!shooter) return;

            var bullet = shooter.SpawnBullet(bulletPrefab, shooter.ShootPoint.position);
            if (bullet)
            {
                bullet.Initialize(direction * bulletSpeed, shooter.Damage);
                // Set bounce properties if the bullet supports it
                var bouncingBullet = bullet as IBouncingBullet;
                if (bouncingBullet != null)
                {
                    bouncingBullet.SetBounceCount(maxBounces);
                }
            }

            currentCooldown = cooldownTime;
        }
    }

    // Interface for bouncing bullets
    public interface IBouncingBullet
    {
        void SetBounceCount(int bounces);
    }
}
