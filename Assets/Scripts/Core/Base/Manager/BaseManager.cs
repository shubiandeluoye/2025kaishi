using UnityEngine;

namespace Core.Base.Manager
{
    /// <summary>
    /// 所有管理器的基类
    /// 提供基本的单例模式和生命周期管理
    /// </summary>
    public abstract class BaseManager : MonoBehaviour
    {
        protected static BaseManager instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                OnManagerAwake();
            }
            else
            {
                Debug.LogWarning($"Duplicate manager of type {GetType().Name} detected. Destroying duplicate.");
                Destroy(gameObject);
            }
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

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        /// 检查管理器是否已经初始化
        /// </summary>
        protected bool IsInitialized()
        {
            if (instance == null)
            {
                Debug.LogError($"Manager of type {GetType().Name} is not initialized!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 确保在场景中只有一个管理器实例
        /// </summary>
        protected void EnsureSingleInstance()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning($"Multiple instances of {GetType().Name} detected. Destroying duplicate.");
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
    }
} 