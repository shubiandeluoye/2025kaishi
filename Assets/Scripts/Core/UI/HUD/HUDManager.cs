using UnityEngine;
using Core.UI.Base;
using Core.Base.Event;
using TMPro;

namespace Core.UI.HUD
{
    public class HUDManager : BaseUIElement
    {
        [Header("Player Score")]
        [SerializeField] private TextMeshProUGUI player1ScoreText;
        [SerializeField] private TextMeshProUGUI player2ScoreText;

        [Header("Health Display")]
        [SerializeField] private TextMeshProUGUI player1HealthText;
        [SerializeField] private TextMeshProUGUI player2HealthText;

        [Header("Ammo Counter")]
        [SerializeField] private TextMeshProUGUI player1AmmoText;
        [SerializeField] private TextMeshProUGUI player2AmmoText;

        protected override void Awake()
        {
            base.Awake();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventManager.Subscribe<PlayerScoreUpdateEvent>(OnScoreUpdate);
            EventManager.Subscribe<PlayerHealthUpdateEvent>(OnHealthUpdate);
            EventManager.Subscribe<PlayerAmmoUpdateEvent>(OnAmmoUpdate);
        }

        private void OnScoreUpdate(PlayerScoreUpdateEvent evt)
        {
            if (evt.PlayerId == 1)
                player1ScoreText.text = $"Score: {evt.Score}";
            else
                player2ScoreText.text = $"Score: {evt.Score}";
        }

        private void OnHealthUpdate(PlayerHealthUpdateEvent evt)
        {
            if (evt.PlayerId == 1)
                player1HealthText.text = $"Health: {evt.Health}";
            else
                player2HealthText.text = $"Health: {evt.Health}";
        }

        private void OnAmmoUpdate(PlayerAmmoUpdateEvent evt)
        {
            if (evt.PlayerId == 1)
                player1AmmoText.text = $"Ammo: {evt.Ammo}";
            else
                player2AmmoText.text = $"Ammo: {evt.Ammo}";
        }
    }

    public class PlayerScoreUpdateEvent
    {
        public int PlayerId { get; private set; }
        public int Score { get; private set; }
        public PlayerScoreUpdateEvent(int playerId, int score)
        {
            PlayerId = playerId;
            Score = score;
        }
    }

    public class PlayerHealthUpdateEvent
    {
        public int PlayerId { get; private set; }
        public float Health { get; private set; }
        public PlayerHealthUpdateEvent(int playerId, float health)
        {
            PlayerId = playerId;
            Health = health;
        }
    }

    public class PlayerAmmoUpdateEvent
    {
        public int PlayerId { get; private set; }
        public int Ammo { get; private set; }
        public PlayerAmmoUpdateEvent(int playerId, int ammo)
        {
            PlayerId = playerId;
            Ammo = ammo;
        }
    }
}
