using UnityEngine;
using Core.Base.Event;
using Core.Combat.Unit.Base;
using Core.Skills.Base;

namespace Core.Skills.Shooting
{
    /// <summary>
    /// 弹跳射击技能
    /// 子弹可以在物体之间弹跳
    /// </summary>
    public class BouncingShootingSkill : BaseSkill
    {
        [Header("Bouncing Settings")]
        [Tooltip("最大弹跳次数")]
        [SerializeField] private int maxBounces = 3;
        
        [Tooltip("每次弹跳后的伤害衰减")]
        [SerializeField] private float damageDecay = 0.8f;
        
        [Tooltip("弹跳检测范围")]
        [SerializeField] private float bounceRadius = 5f;
        
        [Tooltip("可弹跳的目标层级")]
        [SerializeField] private LayerMask bounceTargetLayers;

        private BaseShooter shooter;

        protected override void Awake()
        {
            base.Awake();
            shooter = GetComponent<BaseShooter>();
            if (shooter == null)
            {
                Debug.LogError("BouncingShootingSkill requires a BaseShooter component!");
            }
        }

        public override void Execute(Vector3 direction)
        {
            if (!IsReady || shooter == null) return;

            // 发射初始子弹
            shooter.Shoot(direction);

            // 获取子弹数据
            var bulletData = shooter.GetLastFiredBullet();
            if (bulletData == null) return;

            // 设置弹跳参数
            var bounceData = new BounceData
            {
                CurrentBounces = 0,
                MaxBounces = maxBounces,
                CurrentDamage = bulletData.Damage,
                DamageDecay = damageDecay
            };

            // 发布弹跳事件
            EventManager.Publish(EventManager.EventNames.BULLET_FIRED, new EventManager.BulletEventData
            {
                Position = bulletData.Position,
                Direction = direction,
                Damage = bounceData.CurrentDamage,
                Level = bulletData.Level,
                PathPoints = CalculateBouncePath(bulletData.Position, direction, bounceData),
                Duration = bulletData.Duration
            });

            // 开始冷却
            StartCooldown();
        }

        private Vector3[] CalculateBouncePath(Vector3 startPos, Vector3 direction, BounceData bounceData)
        {
            var path = new System.Collections.Generic.List<Vector3>();
            path.Add(startPos);

            Vector3 currentPos = startPos;
            Vector3 currentDir = direction;

            while (bounceData.CurrentBounces < bounceData.MaxBounces)
            {
                // 检测可能的弹跳目标
                var hits = Physics.SphereCastAll(currentPos, bounceRadius, currentDir, bounceRadius * 2, bounceTargetLayers);
                if (hits.Length == 0) break;

                // 找到最近的有效目标
                float nearestDist = float.MaxValue;
                Vector3? nextPos = null;
                Vector3? nextDir = null;

                foreach (var hit in hits)
                {
                    if (hit.distance < nearestDist)
                    {
                        nearestDist = hit.distance;
                        nextPos = hit.point;
                        nextDir = Vector3.Reflect(currentDir, hit.normal);
                    }
                }

                if (!nextPos.HasValue) break;

                // 添加弹跳点
                path.Add(nextPos.Value);
                
                // 更新状态
                currentPos = nextPos.Value;
                currentDir = nextDir.Value;
                bounceData.CurrentBounces++;
                bounceData.CurrentDamage *= bounceData.DamageDecay;
            }

            return path.ToArray();
        }

        private class BounceData
        {
            public int CurrentBounces;
            public int MaxBounces;
            public float CurrentDamage;
            public float DamageDecay;
        }
    }
}
