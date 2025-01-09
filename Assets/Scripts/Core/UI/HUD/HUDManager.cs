using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.UI.HUD
{
    public class HUDManager : BaseManager
    {
        [Header("UI References")]
        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private GameObject hitMarker;
        [SerializeField] private GameObject damageIndicator;

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<EventManager.InteractionEventData>(EventManager.EventNames.INTERACTION_START, OnInteractionStart);
            EventManager.Subscribe<EventManager.InteractionEventData>(EventManager.EventNames.INTERACTION_END, OnInteractionEnd);
            EventManager.Subscribe<EventManager.BulletEventData>(EventManager.EventNames.BULLET_HIT, OnBulletHit);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<EventManager.InteractionEventData>(EventManager.EventNames.INTERACTION_START, OnInteractionStart);
            EventManager.Unsubscribe<EventManager.InteractionEventData>(EventManager.EventNames.INTERACTION_END, OnInteractionEnd);
            EventManager.Unsubscribe<EventManager.BulletEventData>(EventManager.EventNames.BULLET_HIT, OnBulletHit);
        }

        private void OnInteractionStart(EventManager.InteractionEventData data)
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }

        private void OnInteractionEnd(EventManager.InteractionEventData data)
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void OnBulletHit(EventManager.BulletEventData data)
        {
            if (hitMarker != null)
            {
                StartCoroutine(ShowHitMarker());
            }

            if (damageIndicator != null && data.Damage > 0)
            {
                ShowDamageIndicator(data.Damage);
            }
        }

        private System.Collections.IEnumerator ShowHitMarker()
        {
            hitMarker.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitMarker.SetActive(false);
        }

        private void ShowDamageIndicator(float damage)
        {
            // 实现伤害指示器的显示逻辑
        }
    }
}
