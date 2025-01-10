using UnityEngine;

namespace Core.Base.Event.Data.Combat
{
    /// <summary>
    /// 子弹事件数据
    /// </summary>
    public class BulletEventData : EventDataBase
    {
        /// <summary>
        /// 子弹对象
        /// </summary>
        public GameObject Bullet { get; private set; }

        /// <summary>
        /// 发射者
        /// </summary>
        public GameObject Shooter { get; private set; }

        /// <summary>
        /// 发射位置
        /// </summary>
        public Vector3 FirePosition { get; private set; }

        /// <summary>
        /// 发射方向
        /// </summary>
        public Vector3 FireDirection { get; private set; }

        /// <summary>
        /// 子弹数据
        /// </summary>
        public BulletData Data { get; private set; }

        public BulletEventData(GameObject bullet, GameObject shooter, 
            Vector3 firePosition, Vector3 fireDirection, BulletData data)
        {
            Bullet = bullet;
            Shooter = shooter;
            FirePosition = firePosition;
            FireDirection = fireDirection;
            Data = data;
        }
    }

    /// <summary>
    /// 子弹等级事件数据
    /// </summary>
    public class BulletLevelEventData : EventDataBase
    {
        /// <summary>
        /// 当前等级
        /// </summary>
        public int CurrentLevel { get; private set; }

        /// <summary>
        /// 升级类型
        /// </summary>
        public BulletUpgradeType UpgradeType { get; private set; }

        /// <summary>
        /// 升级来源
        /// </summary>
        public GameObject Source { get; private set; }

        public BulletLevelEventData(int currentLevel, BulletUpgradeType upgradeType, GameObject source)
        {
            CurrentLevel = currentLevel;
            UpgradeType = upgradeType;
            Source = source;
        }

    }

    /// <summary>
    /// 子弹数据
    /// </summary>
    public struct BulletData
    {
        public float Damage;
        public float Speed;
        public float Range;
        public int Penetration;
    }

    /// <summary>
    /// 子弹升级类型枚举
    /// </summary>
    public enum BulletUpgradeType
    {
        /// <summary>
        /// 伤害提升
        /// </summary>
        Damage,

        /// <summary>
        /// 速度提升
        /// </summary>
        Speed,

        /// <summary>
        /// 穿透提升
        /// </summary>
        Penetration,

        /// <summary>
        /// 范围提升
        /// </summary>
        Area
    }
} 