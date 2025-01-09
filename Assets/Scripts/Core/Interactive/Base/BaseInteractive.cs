using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactive.Base
{
    public abstract class BaseInteractive : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] protected bool isInteractable = true;
        [SerializeField] protected string interactionTag = "Player";
        [SerializeField] protected float interactionRadius = 1f;
        
        [Header("Events")]
        public UnityEvent onInteractionStart;
        public UnityEvent onInteractionEnd;
        
        protected bool isInteracting = false;
        protected Collider2D interactionCollider;

        protected virtual void Awake()
        {
            // Get or add collider
            interactionCollider = GetComponent<Collider2D>();
            if (!interactionCollider)
            {
                // Add a CircleCollider2D by default
                interactionCollider = gameObject.AddComponent<CircleCollider2D>();
                ((CircleCollider2D)interactionCollider).radius = interactionRadius;
                interactionCollider.isTrigger = true;
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!isInteractable || isInteracting) return;
            
            if (other.CompareTag(interactionTag))
            {
                StartInteraction(other.gameObject);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!isInteracting) return;
            
            if (other.CompareTag(interactionTag))
            {
                EndInteraction(other.gameObject);
            }
        }

        protected virtual void StartInteraction(GameObject interactor)
        {
            isInteracting = true;
            onInteractionStart?.Invoke();
        }

        protected virtual void EndInteraction(GameObject interactor)
        {
            isInteracting = false;
            onInteractionEnd?.Invoke();
        }

        public virtual void SetInteractable(bool value)
        {
            isInteractable = value;
            if (!isInteractable && isInteracting)
            {
                EndInteraction(null);
            }
        }

        public virtual bool IsInteracting()
        {
            return isInteracting;
        }
    }
}
