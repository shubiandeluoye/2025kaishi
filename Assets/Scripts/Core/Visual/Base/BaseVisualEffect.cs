using UnityEngine;

namespace Core.Visual.Base
{
    public abstract class BaseVisualEffect : MonoBehaviour
    {
        [Header("Visual Effect Settings")]
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected bool autoDestroy = true;

        protected float currentTime;
        protected bool isPlaying;

        public virtual void Play()
        {
            currentTime = 0f;
            isPlaying = true;
            OnEffectStart();
        }

        public virtual void Stop()
        {
            isPlaying = false;
            OnEffectStop();
        }

        protected virtual void Update()
        {
            if (!isPlaying) return;

            currentTime += Time.deltaTime;
            UpdateEffect();

            if (autoDestroy && currentTime >= duration)
            {
                Stop();
                Destroy(gameObject);
            }
        }

        protected abstract void UpdateEffect();
        protected abstract void OnEffectStart();
        protected abstract void OnEffectStop();
    }
}
