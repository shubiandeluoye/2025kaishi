using UnityEngine;
using System.Collections.Generic;
using Core.Visual.Base;
using Core.Base.Event;

namespace Core.Visual
{
    public class EffectManager : MonoBehaviour
    {
        private static EffectManager instance;
        public static EffectManager Instance => instance;

        private Dictionary<string, Queue<GameObject>> effectPools = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void PlayEffect(string effectName, Vector3 position, Quaternion rotation = default)
        {
            GameObject effect = GetEffectFromPool(effectName);
            if (effect != null)
            {
                effect.transform.position = position;
                effect.transform.rotation = rotation;
                effect.SetActive(true);

                var visualEffect = effect.GetComponent<BaseVisualEffect>();
                if (visualEffect != null)
                {
                    visualEffect.Play();
                }
            }
            else
            {
                EventManager.Publish(EventNames.EFFECT_ERROR, 
                    new SystemErrorEvent($"Failed to load effect: {effectName}"));
            }
        }

        private GameObject GetEffectFromPool(string effectName)
        {
            if (!effectPools.ContainsKey(effectName))
            {
                effectPools[effectName] = new Queue<GameObject>();
                EventManager.Publish(EventNames.EFFECT_POOL_CREATED, 
                    new EffectPoolEvent(effectName, 0));
            }

            Queue<GameObject> pool = effectPools[effectName];
            
            if (pool.Count == 0)
            {
                GameObject prefab = Resources.Load<GameObject>($"Effects/{effectName}");
                if (prefab == null)
                {
                    EventManager.Publish(EventNames.EFFECT_ERROR, 
                        new SystemErrorEvent($"Effect prefab not found: {effectName}"));
                    return null;
                }
                
                GameObject instance = Instantiate(prefab, transform);
                instance.name = effectName;
                return instance;
            }

            return pool.Dequeue();
        }

        public void ReturnToPool(GameObject effect)
        {
            string effectName = effect.name.Replace("(Clone)", "");
            effect.SetActive(false);
            
            if (!effectPools.ContainsKey(effectName))
            {
                effectPools[effectName] = new Queue<GameObject>();
            }
            
            effectPools[effectName].Enqueue(effect);
            
            EventManager.Publish(EventNames.EFFECT_POOL_CREATED, 
                new EffectPoolEvent(effectName, effectPools[effectName].Count));
        }
    }
} 