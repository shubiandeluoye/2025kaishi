using UnityEngine;
using System.Collections;
using Core.Skills.Base;
using Core.Combat.Unit.Base;

namespace Core.Skills.Shooting
{
    public class BurstShootingSkill : BaseSkill
    {
        [SerializeField] private BaseBullet bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float timeBetweenShots = 0.1f;

        private bool isBursting = false;

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || !owner || isBursting) return;

            StartCoroutine(FireBurst(direction));
            currentCooldown = cooldownTime;
        }

        private IEnumerator FireBurst(Vector3 direction)
        {
            isBursting = true;
            var shooter = owner as BaseShooter;
            if (shooter)
            {
                for (int i = 0; i < burstCount; i++)
                {
                    var bullet = shooter.SpawnBullet(bulletPrefab, shooter.ShootPoint.position);
                    if (bullet)
                    {
                        bullet.Initialize(direction * bulletSpeed, shooter.Damage);
                    }

                    if (i < burstCount - 1)
                    {
                        yield return new WaitForSeconds(timeBetweenShots);
                    }
                }
            }
            isBursting = false;
        }
    }
}
