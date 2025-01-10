using UnityEngine;
using Core.Visual.Base;

namespace Core.Visual.Effects
{
    public class HitEffect : BaseVisualEffect
    {
        [Header("Hit Effect Settings")]
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private float fadeOutSpeed = 1f;
        
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        protected override void OnEffectStart()
        {
            if (hitParticles != null)
            {
                hitParticles.Play();
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }

        protected override void OnEffectUpdate(float progress)
        {
            if (spriteRenderer != null)
            {
                // 使用 fadeOutSpeed 来控制淡出速度
                Color currentColor = spriteRenderer.color;
                currentColor.a = Mathf.Lerp(originalColor.a, 0f, progress * fadeOutSpeed);
                spriteRenderer.color = currentColor;
            }
        }

        protected override void OnEffectEnd()
        {
            if (hitParticles != null)
            {
                hitParticles.Stop();
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }
} 