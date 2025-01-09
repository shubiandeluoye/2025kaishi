using UnityEngine;
using Core.Base.Event;

namespace Core.Base.Manager
{
    /// <summary>
    /// 特效管理器基类
    /// 处理游戏中所有视觉特效的基本功能
    /// </summary>
    public abstract class BaseEffectManager : BaseManager
    {
        [Header("Effect Settings")]
        [SerializeField] protected bool useObjectPool = true;
        [SerializeField] protected int defaultPoolSize = 20;

        protected override void OnManagerAwake()
        {
            base.OnManagerAwake();
            InitializeEffectPools();
        }

        protected virtual void InitializeEffectPools()
        {
            // 子类实现具体的对象池初始化
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        protected virtual GameObject PlayEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, float duration = 2f)
        {
            if (effectPrefab == null) return null;

            GameObject effect = useObjectPool 
                ? GetEffectFromPool(effectPrefab)
                : Instantiate(effectPrefab);

            effect.transform.position = position;
            effect.transform.rotation = rotation;

            if (duration > 0)
            {
                StartCoroutine(DestroyEffectAfterDelay(effect, duration));
            }

            return effect;
        }

        protected virtual GameObject GetEffectFromPool(GameObject prefab)
        {
            // 子类实现对象池逻辑
            return null;
        }

        protected System.Collections.IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (useObjectPool)
                ReturnEffectToPool(effect);
            else
                Destroy(effect);
        }

        protected virtual void ReturnEffectToPool(GameObject effect)
        {
            // 子类实现返回对象池逻辑
        }
    }
} 