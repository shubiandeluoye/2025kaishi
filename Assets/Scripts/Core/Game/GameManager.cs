using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.Game
{
    /// <summary>
    /// Main game manager that coordinates game state and systems
    /// </summary>
    public class GameManager : BaseManager
    {
        [Header("Game Settings")]
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject victoryUI;
        
        [Header("Out of Bounds Settings")]
        [SerializeField] private float minHeightThreshold = -20f;  // 高度阈值
        [SerializeField] private float checkInterval = 0.1f;       // 检查间隔
        
        private float checkTimer;

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<PlayerHealthEvent>(EventNames.PLAYER_HEALTH_ZERO, OnPlayerHealthZero);
            EventManager.Subscribe<PlayerOutOfBoundsEvent>(EventNames.PLAYER_OUT_OF_BOUNDS, OnPlayerOutOfBounds);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<PlayerHealthEvent>(EventNames.PLAYER_HEALTH_ZERO, OnPlayerHealthZero);
            EventManager.Unsubscribe<PlayerOutOfBoundsEvent>(EventNames.PLAYER_OUT_OF_BOUNDS, OnPlayerOutOfBounds);
        }

        private void Update()
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkInterval)
            {
                checkTimer = 0f;
                CheckPlayersHeight();
            }
        }

        private void CheckPlayersHeight()
        {
            // 获取所有玩家
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            foreach (GameObject player in players)
            {
                // 检查玩家高度
                if (player.transform.position.y < minHeightThreshold)
                {
                    int playerIndex = GetPlayerIndex(player);  // 需要实现这个方法
                    HandlePlayerFellOff(playerIndex, player.transform.position);
                }
            }
        }

        private int GetPlayerIndex(GameObject player)
        {
            // 根据你的玩家标识方式来实现
            // 例如：通过名字判断
            if (player.name.Contains("Player1")) return 0;
            if (player.name.Contains("Player2")) return 1;
            return -1;
        }

        private void HandlePlayerFellOff(int playerIndex, Vector3 lastPosition)
        {
            EventManager.Publish(EventNames.PLAYER_OUT_OF_BOUNDS, 
                new PlayerOutOfBoundsEvent(playerIndex, lastPosition, "Height"));
        }

        private void OnPlayerHealthZero(PlayerHealthEvent data)
        {
            int winnerIndex = data.PlayerIndex == 0 ? 1 : 0;
            HandlePlayerWon(winnerIndex, "Health Zero");
        }

        private void OnPlayerOutOfBounds(PlayerOutOfBoundsEvent data)
        {
            int winnerIndex = data.PlayerIndex == 0 ? 1 : 0;
            HandlePlayerWon(winnerIndex, data.FallReason == "Height" ? "Fell Off" : "Out Of Bounds");
        }

        private void HandlePlayerWon(int winnerIndex, string reason)
        {
            if (gameplayUI != null) gameplayUI.SetActive(false);
            if (victoryUI != null) victoryUI.SetActive(true);

            EventManager.Publish(EventNames.PLAYER_VICTORY, 
                new PlayerVictoryEvent(winnerIndex, reason));
            
            EventManager.Publish(EventNames.GAME_OVER, 
                new GameOverEvent(true));
        }
    }
}
