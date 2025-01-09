using UnityEngine;
using Core.Base.Event;

namespace Core.Interactive.Base
{
    /// <summary>
    /// 可交互物体的基类
    /// 提供基础的交互功能和事件系统集成
    /// </summary>
    public class BaseInteractive : MonoBehaviour
    {
        #region 字段和属性
        [Header("Interaction Settings")]
        [Tooltip("是否可以交互")]
        [SerializeField] protected bool isInteractable = true;
        
        [Tooltip("交互范围")]
        [SerializeField] protected float interactionRange = 2f;
        
        [Tooltip("可以与之交互的层级")]
        [SerializeField] protected LayerMask interactorLayers;
        
        [Tooltip("交互提示文本")]
        [SerializeField] protected string interactionPrompt = "Press E to interact";
        
        // 状态标记
        protected bool isHighlighted;
        protected bool isSelected;
        protected GameObject currentInteractor;
        #endregion

        #region Unity生命周期
        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnregisterEvents();
            ResetInteractionState();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            // 在编辑器中绘制交互范围
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
        #endregion

        #region 事件注册
        /// <summary>
        /// 注册事件监听
        /// </summary>
        protected virtual void RegisterEvents()
        {
            EventManager.Subscribe<EventManager.InteractionEventData>(
                EventManager.EventNames.INTERACTION_START, 
                OnInteractionStart);
        }

        /// <summary>
        /// 取消事件注册
        /// </summary>
        protected virtual void UnregisterEvents()
        {
            EventManager.Unsubscribe<EventManager.InteractionEventData>(
                EventManager.EventNames.INTERACTION_START, 
                OnInteractionStart);
        }
        #endregion

        #region 交互系统
        /// <summary>
        /// 检查是否可以交互
        /// </summary>
        public virtual bool CanInteract(GameObject interactor)
        {
            if (!isInteractable) return false;
            
            // 检查距离
            float distance = Vector3.Distance(transform.position, interactor.transform.position);
            if (distance > interactionRange) return false;
            
            // 检查层级
            if ((interactorLayers.value & (1 << interactor.layer)) == 0) return false;
            
            return true;
        }

        /// <summary>
        /// 开始交互
        /// </summary>
        public virtual void StartInteraction(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;
            
            currentInteractor = interactor;
            
            // 发布交互开始事件
            var interactionData = new EventManager.InteractionEventData
            {
                Interactor = interactor,
                Target = gameObject,
                InteractionType = "Start"
            };
            
            EventManager.Publish(EventManager.EventNames.INTERACTION_START, interactionData);
        }

        /// <summary>
        /// 结束交互
        /// </summary>
        public virtual void EndInteraction(GameObject interactor)
        {
            if (currentInteractor != interactor) return;
            
            var interactionData = new EventManager.InteractionEventData
            {
                Interactor = interactor,
                Target = gameObject,
                InteractionType = "End"
            };
            
            EventManager.Publish(EventManager.EventNames.INTERACTION_END, interactionData);
            currentInteractor = null;
        }
        #endregion

        #region 状态管理
        /// <summary>
        /// 重置交互状态
        /// </summary>
        protected virtual void ResetInteractionState()
        {
            if (currentInteractor != null)
            {
                EndInteraction(currentInteractor);
            }
            
            isHighlighted = false;
            isSelected = false;
            currentInteractor = null;
        }
        #endregion

        #region 事件处理
        /// <summary>
        /// 处理交互开始事件
        /// </summary>
        protected virtual void OnInteractionStart(EventManager.InteractionEventData data)
        {
            if (data.Target == gameObject)
            {
                isHighlighted = true;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置是否可交互
        /// </summary>
        public virtual void SetInteractable(bool interactable)
        {
            if (isInteractable == interactable) return;
            
            isInteractable = interactable;
            if (!isInteractable)
            {
                ResetInteractionState();
            }
        }

        /// <summary>
        /// 设置交互范围
        /// </summary>
        public virtual void SetInteractionRange(float range)
        {
            interactionRange = range;
        }

        /// <summary>
        /// 设置可交互层级
        /// </summary>
        public virtual void SetInteractorLayers(LayerMask layers)
        {
            interactorLayers = layers;
        }

        /// <summary>
        /// 设置交互提示文本
        /// </summary>
        public virtual void SetInteractionPrompt(string prompt)
        {
            interactionPrompt = prompt;
        }
        #endregion

        #region Getters
        public bool IsInteractable() => isInteractable;
        public bool IsHighlighted() => isHighlighted;
        public bool IsSelected() => isSelected;
        public float GetInteractionRange() => interactionRange;
        public LayerMask GetInteractorLayers() => interactorLayers;
        public string GetInteractionPrompt() => interactionPrompt;
        public GameObject GetCurrentInteractor() => currentInteractor;
        #endregion
    }
}
