using UnityEngine;
using Core.Base.Event;
using Core.UI.Base;
using Core.UI.Events;

namespace Core.UI.HUD
{
    public class HUDManager : BaseUIManager
    {
        [Header("UI References")]
        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private GameObject hitMarker;
        [SerializeField] private GameObject damageIndicator;

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            EventManager.Subscribe<UIInteractionEvent>(EventNames.HUD_INTERACTION, OnInteractionEvent);
            EventManager.Subscribe<UIEvent>(EventNames.HUD_COMBAT, OnCombatEvent);
            EventManager.Subscribe<HUDUpdateEvent>(EventNames.HUD_UPDATE, OnHUDUpdate);
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();
            EventManager.Unsubscribe<UIEvent>("HUD_INTERACTION", OnInteractionEvent);
            EventManager.Unsubscribe<UIEvent>("HUD_COMBAT", OnCombatEvent);
            EventManager.Unsubscribe<UIEvent>("HUD_UPDATE", OnHUDUpdate);
        }

        private void OnInteractionEvent(UIEvent evt)
        {
            if (interactionPrompt != null)
            {
                bool shouldShow = (bool)evt.Data;
                interactionPrompt.SetActive(shouldShow);
            }
        }

        private void OnCombatEvent(UIEvent evt)
        {
            if (evt.Type == UIEventType.CombatHit)
            {
                if (hitMarker != null)
                {
                    StartCoroutine(ShowHitMarker());
                }
            }
            else if (evt.Type == UIEventType.CombatDamage)
            {
                if (damageIndicator != null)
                {
                    float damage = (float)evt.Data;
                    ShowDamageIndicator(damage);
                }
            }
        }

        private void OnHUDUpdate(UIEvent evt)
        {
            // 处理HUD更新事件
            // 比如更新血量、弹药、分数等
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
