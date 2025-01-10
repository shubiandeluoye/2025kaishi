using UnityEngine;
using Core.Combat.Bullet;
using Core.Base.Manager;
using Core.Combat.Team;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Core.Interactive.MiddleShapes
{
    public class CircleShape : BaseManager
    {
        #region Properties
        [Header("Collection Settings")]
        [SerializeField] protected int maxBulletCount = 20;
        [SerializeField] protected Vector2 skillTimeRange = new Vector2(5f, 8f);
        [SerializeField] protected float timeOutDuration = 30f;

        [Header("Skill Settings")]
        [SerializeField] protected float[] skillAngles = { 0f, 45f, 90f, 135f, 180f };
        [SerializeField] protected int bulletsPerSkill = 4;
        [SerializeField] protected float skillSpread = 15f;

        [Header("TimeOut Burst Settings")]
        [SerializeField] protected int burstBulletCount = 36;  // 360度发射的子弹数量（每10度一个）
        [SerializeField] protected float burstSpread = 10f;    // 每个方向的随机偏移角度
        [SerializeField] protected bool randomizeHeight = false; // 是否随机高度（3D/2D切换）
        
        protected float timeOutTimer;
        protected bool isTimedOut;
        protected List<CollectedBullet> collectedBullets = new List<CollectedBullet>();
        protected float skillTimer;
        protected float currentSkillTime;
        protected bool isSkillActive;
        #endregion

        #region Initialization
        protected virtual void Start()
        {
            ResetSkillTimer();
        }

        protected virtual void ResetSkillTimer()
        {
            currentSkillTime = Random.Range(skillTimeRange.x, skillTimeRange.y);
            skillTimer = 0f;
            isSkillActive = false;
        }
        #endregion

        #region Update
        protected virtual void Update()
        {
            if (!isTimedOut && !isSkillActive)
            {
                // 更新两个计时器
                skillTimer += Time.deltaTime;
                timeOutTimer += Time.deltaTime;

                // 检查是否到达超时时间
                if (timeOutTimer >= timeOutDuration)
                {
                    TimeOutBurst();
                }
                // 检查是否触发技能
                else if (skillTimer >= currentSkillTime)
                {
                    ActivateSkill();
                }
            }
        }
        #endregion

        #region TimeOut Burst
        protected virtual void TimeOutBurst()
        {
            if (collectedBullets.Count == 0) return;

            isTimedOut = true;

            // 计算每个子弹之间的角度
            float angleStep = 360f / burstBulletCount;

            for (int i = 0; i < burstBulletCount; i++)
            {
                // 基础角度
                float baseAngle = i * angleStep;
                
                // 添加随机偏移
                float randomOffset = Random.Range(-burstSpread, burstSpread);
                float finalAngle = baseAngle + randomOffset;

                // 创建2D平面上的方向向量
                Vector3 direction = Quaternion.Euler(0, finalAngle, 0) * Vector3.forward;

                // 随机选择一个收集的子弹作为模板
                if (collectedBullets.Count > 0)
                {
                    int randomIndex = Random.Range(0, collectedBullets.Count);
                    SpawnAndShootBullet(collectedBullets[randomIndex], direction);
                }
            }

            // 播放爆发特效
            PlayBurstEffects();

            // 清理并重置
            ClearCollectedBullets();
            ResetTimers();
        }

        protected virtual void PlayBurstEffects()
        {
            if (burstEffectPrefab != null)
            {
                Instantiate(burstEffectPrefab, transform.position, Quaternion.identity);
            }

            if (burstSound != null)
            {
                AudioSource.PlayClipAtPoint(burstSound, transform.position);
            }
        }

        protected virtual void ResetTimers()
        {
            timeOutTimer = 0f;
            skillTimer = 0f;
            isTimedOut = false;
            isSkillActive = false;
            currentSkillTime = Random.Range(skillTimeRange.x, skillTimeRange.y);
        }
        #endregion

        #region Skill System
        protected virtual void ActivateSkill()
        {
            if (collectedBullets.Count == 0) return;

            // 确定发射方向（基于子弹收集较少的一方）
            bool shootRight = DetermineShootDirection();
            
            // 随机打乱技能角度
            List<float> shuffledAngles = new List<float>(skillAngles);
            for (int i = shuffledAngles.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                float temp = shuffledAngles[i];
                shuffledAngles[i] = shuffledAngles[j];
                shuffledAngles[j] = temp;
            }

            // 执行技能
            StartCoroutine(ExecuteSkillSequence(shuffledAngles, shootRight));
        }

        protected virtual System.Collections.IEnumerator ExecuteSkillSequence(List<float> angles, bool shootRight)
        {
            isSkillActive = true;

            foreach (float baseAngle in angles)
            {
                // 调整角度（如果向左发射，镜像角度）
                float adjustedAngle = shootRight ? baseAngle : (180f - baseAngle);
                
                // 发射这组子弹
                FireSkillBullets(adjustedAngle);
                
                // 等待一小段时间再发射下一组
                yield return new WaitForSeconds(0.5f);
            }

            // 技能结束，重置
            ClearCollectedBullets();
            ResetSkillTimer();
        }

        protected virtual void FireSkillBullets(float baseAngle)
        {
            // 计算这组子弹的散射角度
            float angleStep = skillSpread / (bulletsPerSkill - 1);
            float startAngle = baseAngle - (skillSpread * 0.5f);

            for (int i = 0; i < bulletsPerSkill; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * Vector3.right;

                // 从收集的子弹中随机选择一个作为模板
                if (collectedBullets.Count > 0)
                {
                    int randomIndex = Random.Range(0, collectedBullets.Count);
                    SpawnAndShootBullet(collectedBullets[randomIndex], direction);
                }
            }

            PlaySkillEffects(baseAngle);
        }

        protected virtual bool DetermineShootDirection()
        {
            // 统计左右两侧的子弹数量
            int leftCount = 0, rightCount = 0;
            foreach (var bullet in collectedBullets)
            {
                if (bullet.BulletPrefab.transform.position.x < transform.position.x)
                    leftCount++;
                else
                    rightCount++;
            }

            // 返回true表示向右射击（左边子弹多），false表示向左射击
            return leftCount > rightCount;
        }
        #endregion

        #region Effects
        protected virtual void PlaySkillEffects(float angle)
        {
            // 播放技能特效和音效
            if (skillEffectPrefab != null)
            {
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Instantiate(skillEffectPrefab, transform.position, rotation);
            }

            if (skillSound != null)
            {
                AudioSource.PlayClipAtPoint(skillSound, transform.position);
            }
        }
        #endregion

        protected struct CollectedBullet
        {
            public BaseBullet BulletPrefab;
            public TeamType Team;
            public float Damage;
        }

        protected virtual int DetermineTargetTeam()
        {
            // 统计各队伍的子弹数量
            Dictionary<TeamType, int> teamBulletCount = new Dictionary<TeamType, int>();
            foreach (var bullet in collectedBullets)
            {
                if (!teamBulletCount.ContainsKey(bullet.Team))
                    teamBulletCount[bullet.Team] = 0;
                teamBulletCount[bullet.Team]++;
            }

            // 找出数量最多的队伍
            TeamType dominantTeam = TeamType.System;
            int maxCount = 0;
            foreach (var pair in teamBulletCount)
            {
                if (pair.Value > maxCount)
                {
                    maxCount = pair.Value;
                    dominantTeam = pair.Key;
                }
            }

            // 返回主导队伍的对手队伍
            return TeamSystem.GetOppositeTeam(dominantTeam);
        }
    }
}
