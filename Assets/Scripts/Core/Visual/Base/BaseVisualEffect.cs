using UnityEngine;
using System.Collections;
using Core.Base.Event;

namespace Core.Visual.Base
{
    public abstract class BaseVisualEffect : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected bool autoDestroy = true;

        protected bool isPlaying;
        protected float currentTime;

        public virtual void Play()
        {
            isPlaying = true;
            currentTime = 0f;
            gameObject.SetActive(true);
            
            EventManager.Publish(EventNames.EFFECT_PLAY, 
                new EffectEventData(gameObject.name, transform.position, transform.rotation, duration));
            
            OnEffectStart();
            StartCoroutine(EffectRoutine());
        }

        public virtual void Stop()
        {
            isPlaying = false;
            
            EventManager.Publish(EventNames.EFFECT_STOP, 
                new EffectEventData(gameObject.name, transform.position, transform.rotation));
            
            OnEffectEnd();
            
            if (autoDestroy)
            {
                EffectManager.Instance.ReturnToPool(gameObject);
            }
        }

        protected virtual IEnumerator EffectRoutine()
        {
            while (isPlaying && currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float progress = currentTime / duration;
                
                EventManager.Publish(EventNames.EFFECT_PROGRESS, 
                    new EffectProgressEvent(gameObject.name, progress, gameObject));
                
                OnEffectUpdate(progress);
                yield return null;
            }

            EventManager.Publish(EventNames.EFFECT_COMPLETE, 
                new EffectEventData(gameObject.name, transform.position, transform.rotation));
            
            Stop();
        }

        protected virtual void OnEffectStart() { }
        protected virtual void OnEffectUpdate(float progress) { }
        protected virtual void OnEffectEnd() { }
    }
}
