using UnityEngine;
using System.Collections;
using Core.Base.Event;

namespace Core.Game
{
    public class ShapeSpawnManager : MonoBehaviour
    {
        private IEnumerator spawnRoutine;

        private void OnEnable()
        {
            EventManager.Subscribe<GameStartEvent>(EventNames.GAME_START, OnGameStart);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<GameStartEvent>(EventNames.GAME_START, OnGameStart);
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }

        private void OnGameStart(GameStartEvent data)
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
            }
            spawnRoutine = SpawnRoutine();
            StartCoroutine(spawnRoutine);
        }

        private IEnumerator SpawnRoutine()
        {
            yield break;
        }
    }
}
