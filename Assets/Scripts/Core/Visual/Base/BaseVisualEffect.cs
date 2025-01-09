using UnityEngine;

namespace Core.Visual.Base
{
    public abstract class BaseVisualEffect : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected bool autoDestroy = true;
        
        protected float currentTime = 0f;
        protected bool isPlaying = false;

        protected virtual void Start()
        {
            // Initialize effect
        }

        protected virtual void Update()
        {
            if (!isPlaying) return;

            currentTime += Time.deltaTime;
            
            if (autoDestroy && currentTime >= duration)
            {
                OnEffectComplete();
            }
        }

        public virtual void Play()
        {
            isPlaying = true;
            currentTime = 0f;
        }

        public virtual void Stop()
        {
            isPlaying = false;
            currentTime = 0f;
        }

        protected virtual void OnEffectComplete()
        {
            Stop();
            if (autoDestroy)
            {
                Destroy(gameObject);
            }
        }

        public virtual void SetDuration(float newDuration)
        {
            duration = newDuration;
        }

        public virtual bool IsPlaying()
        {
            return isPlaying;
        }
    }
}
