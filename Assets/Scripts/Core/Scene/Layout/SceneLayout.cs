using UnityEngine;
using Core.Interactive.Base;

namespace Core.Scene.Layout
{
    public class SceneLayout : BaseManager
    {
        [Header("Layout Settings")]
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform[] interactivePoints;
        [SerializeField] private Transform[] obstaclePoints;

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<SpawnRequestEvent>(EventNames.SPAWN_REQUEST, OnSpawnRequest);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<SpawnRequestEvent>(EventNames.SPAWN_REQUEST, OnSpawnRequest);
        }

        private void OnSpawnRequest(SpawnRequestEvent evt)
        {
            switch (evt.SpawnType)
            {
                case SpawnType.Interactive:
                    SpawnInteractiveElements();
                    break;
                case SpawnType.Obstacle:
                    SpawnObstacles();
                    break;
            }
        }

        private void Start()
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            // Initialize scene layout
            SpawnInteractiveElements();
            SpawnObstacles();
        }

        private void SpawnInteractiveElements()
        {
            foreach (Transform point in interactivePoints)
            {
                // Spawn interactive elements at designated points
                if (point != null)
                {
                    // Implementation for spawning interactive elements
                }
            }
        }

        private void SpawnObstacles()
        {
            foreach (Transform point in obstaclePoints)
            {
                // Spawn obstacles at designated points
                if (point != null)
                {
                    // Implementation for spawning obstacles
                }
            }
        }

        public Vector3 GetPlayerSpawnPosition()
        {
            return playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
        }
    }
}
