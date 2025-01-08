using UnityEngine;
using System;
using System.Collections.Generic;
using Core.Base.Pool;
using Core.Base.Event;

namespace Core.Combat.Unit.Base
{
    public class BaseShooter : BaseUnit
    {
        #region Firing Properties
        [Serializable]
        public class WeaponStats
        {
            public float fireRate = 0.5f;
            public float bulletSpeed = 20f;
            public float bulletDamage = 10f;
            public float bulletSpread = 5f;
            public int bulletsPerShot = 1;
            public float reloadTime = 2f;
            public int maxAmmo = 30;
            public int currentAmmo;
            
            public void Initialize()
            {
                currentAmmo = maxAmmo;
            }
        }

        [SerializeField]
        protected WeaponStats weaponStats = new WeaponStats();
        
        [SerializeField]
        protected Transform firePoint;
        
        protected float nextFireTime;
        protected bool isReloading;
        #endregion

        #region Recoil System
        [Serializable]
        public class RecoilSettings
        {
            public float recoilForce = 2f;
            public float recoilRecoverySpeed = 3f;
            public float maxRecoil = 10f;
            public AnimationCurve recoilPattern = AnimationCurve.Linear(0, 0, 1, 1);
        }

        [SerializeField]
        protected RecoilSettings recoilSettings = new RecoilSettings();
        
        protected float currentRecoil;
        protected Vector3 recoilOffset;
        #endregion

        #region Accuracy System
        [Serializable]
        public class AccuracySettings
        {
            public float baseAccuracy = 0.8f;
            public float movementAccuracyPenalty = 0.2f;
            public float jumpingAccuracyPenalty = 0.4f;
            public float recoveryRate = 0.1f;
            public float minAccuracy = 0.3f;
        }

        [SerializeField]
        protected AccuracySettings accuracySettings = new AccuracySettings();
        
        protected float currentAccuracy;
        #endregion

        #region Events
        public event Action<int> OnAmmoChanged;
        public event Action OnStartReload;
        public event Action OnEndReload;
        public event Action OnFire;
        #endregion

        #region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();
            weaponStats.Initialize();
            currentAccuracy = accuracySettings.baseAccuracy;
        }

        protected override void Update()
        {
            base.Update();
            UpdateRecoil();
            UpdateAccuracy();
            UpdateReload();
        }
        #endregion

        #region Firing System
        public virtual bool CanFire()
        {
            return !isReloading && 
                   Time.time >= nextFireTime && 
                   weaponStats.currentAmmo > 0;
        }

        public virtual void Fire()
        {
            if (!CanFire()) return;

            for (int i = 0; i < weaponStats.bulletsPerShot; i++)
            {
                FireBullet();
            }

            weaponStats.currentAmmo--;
            nextFireTime = Time.time + weaponStats.fireRate;
            
            ApplyRecoil();
            DecreaseAccuracy();
            
            OnAmmoChanged?.Invoke(weaponStats.currentAmmo);
            OnFire?.Invoke();

            if (weaponStats.currentAmmo <= 0)
            {
                StartReload();
            }
        }

        protected virtual void FireBullet()
        {
            if (firePoint == null) return;

            float spread = weaponStats.bulletSpread * (1f - currentAccuracy);
            Quaternion randomRotation = Quaternion.Euler(
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread),
                0
            );

            GameObject bulletObj = PoolManager.Instance.Spawn("Bullet", firePoint.position, firePoint.rotation * randomRotation);
            if (bulletObj == null) return;

            BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                bullet.Initialize(this, weaponStats.bulletDamage, weaponStats.bulletSpeed);
            }
        }
        #endregion

        #region Recoil System
        protected virtual void ApplyRecoil()
        {
            currentRecoil = Mathf.Min(currentRecoil + recoilSettings.recoilForce, recoilSettings.maxRecoil);
            
            float recoilAmount = recoilSettings.recoilPattern.Evaluate(currentRecoil / recoilSettings.maxRecoil);
            recoilOffset = Vector3.up * recoilAmount;
        }

        protected virtual void UpdateRecoil()
        {
            if (currentRecoil > 0)
            {
                currentRecoil = Mathf.Max(0, currentRecoil - recoilSettings.recoilRecoverySpeed * Time.deltaTime);
                float recoilAmount = recoilSettings.recoilPattern.Evaluate(currentRecoil / recoilSettings.maxRecoil);
                recoilOffset = Vector3.up * recoilAmount;
            }
        }
        #endregion

        #region Accuracy System
        protected virtual void UpdateAccuracy()
        {
            if (currentAccuracy < accuracySettings.baseAccuracy)
            {
                currentAccuracy = Mathf.Min(
                    accuracySettings.baseAccuracy,
                    currentAccuracy + accuracySettings.recoveryRate * Time.deltaTime
                );
            }
        }

        protected virtual void DecreaseAccuracy()
        {
            currentAccuracy = Mathf.Max(
                accuracySettings.minAccuracy,
                currentAccuracy - (1f - currentAccuracy) * 0.5f
            );
        }

        public virtual void ModifyAccuracy(float modifier)
        {
            currentAccuracy = Mathf.Clamp(
                currentAccuracy + modifier,
                accuracySettings.minAccuracy,
                accuracySettings.baseAccuracy
            );
        }
        #endregion

        #region Ammo Management
        public virtual void StartReload()
        {
            if (isReloading || weaponStats.currentAmmo >= weaponStats.maxAmmo) return;

            isReloading = true;
            OnStartReload?.Invoke();
            
            Invoke(nameof(CompleteReload), weaponStats.reloadTime);
        }

        protected virtual void CompleteReload()
        {
            isReloading = false;
            weaponStats.currentAmmo = weaponStats.maxAmmo;
            
            OnEndReload?.Invoke();
            OnAmmoChanged?.Invoke(weaponStats.currentAmmo);
        }

        protected virtual void UpdateReload()
        {
        }
        #endregion

        #region Getters
        public float GetCurrentAccuracy() => currentAccuracy;
        public Vector3 GetRecoilOffset() => recoilOffset;
        public bool IsReloading() => isReloading;
        public int GetCurrentAmmo() => weaponStats.currentAmmo;
        public int GetMaxAmmo() => weaponStats.maxAmmo;
        #endregion
    }
}
