using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.Game
{
    public class VictoryManager : BaseManager
    {
        protected override void RegisterEvents()
        {
            EventManager.Subscribe<PlayerVictoryEvent>(EventNames.PLAYER_VICTORY, OnPlayerVictory);
            EventManager.Subscribe<GameOverEvent>(EventNames.GAME_OVER, OnGameOver);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<PlayerVictoryEvent>(EventNames.PLAYER_VICTORY, OnPlayerVictory);
            EventManager.Unsubscribe<GameOverEvent>(EventNames.GAME_OVER, OnGameOver);
        }

        private void OnPlayerVictory(PlayerVictoryEvent data)
        {
            Debug.Log($"Player {data.WinnerIndex} Victory! Reason: {data.VictoryReason}");
        }

        private void OnGameOver(GameOverEvent data)
        {
            Debug.Log($"Game Over! Normal End: {data.IsNormalEnd}");
        }
    }
}
