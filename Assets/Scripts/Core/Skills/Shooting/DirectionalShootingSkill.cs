using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

namespace Core.Skills.Shooting
{
    public class DirectionalShootingSkill : BaseSkill
    {
        [SerializeField] private BaseBullet bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private float spreadAngle = 0f;

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || !owner) return;

            var shooter = owner as BaseShooter;
            if (!shooter) return;

            // Create the bullet
            var bullet = shooter.SpawnBullet(bulletPrefab, shooter.ShootPoint.position);
            if (bullet)
            {
                // Apply spread if any
                if (spreadAngle != 0)
                {
                    float randomSpread = Random.Range(-spreadAngle, spreadAngle);
                    direction = Quaternion.Euler(0, 0, randomSpread) * direction;
                }

                // Set bullet properties
                bullet.Initialize(direction * bulletSpeed, shooter.Damage);
            }

            currentCooldown = cooldownTime;
        }
    }
}
