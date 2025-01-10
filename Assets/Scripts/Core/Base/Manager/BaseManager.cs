using UnityEngine;

namespace Core.Base.Manager
{
    /// <summary>
    /// 所有管理器的基类
    /// 提供基本的生命周期管理
    /// </summary>
    public abstract class BaseManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            EnsureSingleInstance();
            OnManagerAwake();
        }

        protected virtual void OnManagerAwake() { }

        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnregisterEvents();
        }

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        protected virtual void OnDestroy() { }

        /// <summary>
        /// 确保在场景中只有一个管理器实例
        /// </summary>
        protected virtual void EnsureSingleInstance()
        {
            var existingInstance = FindObjectOfType(GetType());
            if (existingInstance != null && existingInstance != this)
            {
                Debug.LogWarning($"Multiple instances of {GetType().Name} detected. Destroying duplicate.");
                Destroy(gameObject);
            }
        }
    }
} 