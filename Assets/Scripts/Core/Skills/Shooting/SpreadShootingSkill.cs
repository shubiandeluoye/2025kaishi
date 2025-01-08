using UnityEngine;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

namespace Core.Skills.Shooting
{
    public class SpreadShootingSkill : BaseSkill
    {
        [SerializeField] private BaseBullet bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private int bulletCount = 3;
        [SerializeField] private float spreadAngle = 15f;

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || !owner) return;

            var shooter = owner as BaseShooter;
            if (!shooter) return;

            float angleStep = spreadAngle / (bulletCount - 1);
            float startAngle = -spreadAngle / 2;

            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector3 rotatedDirection = Quaternion.Euler(0, 0, currentAngle) * direction;

                var bullet = shooter.SpawnBullet(bulletPrefab, shooter.ShootPoint.position);
                if (bullet)
                {
                    bullet.Initialize(rotatedDirection * bulletSpeed, shooter.Damage);
                }
            }

            currentCooldown = cooldownTime;
        }
    }
}
