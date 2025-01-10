using UnityEngine;

namespace Core.Scene
{
    public class SceneSetup : BaseManager
    {
        [Header("Prefab References")]
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject targetPrefab;

        [Header("Arena Settings")]
        [SerializeField] private Vector2 arenaSize = new Vector2(40f, 40f);
        [SerializeField] private float wallHeight = 4f;
        [SerializeField] private float targetSpacing = 5f;

        private void Awake()
        {
            CreateArena();
        }

        private void CreateArena()
        {
            // Create floor
            GameObject floor = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);
            floor.transform.localScale = new Vector3(arenaSize.x, 0.1f, arenaSize.y);

            // Create boundary walls
            CreateWall(new Vector3(-arenaSize.x/2, wallHeight/2, 0), new Vector3(0.5f, wallHeight, arenaSize.y)); // Left wall
            CreateWall(new Vector3(arenaSize.x/2, wallHeight/2, 0), new Vector3(0.5f, wallHeight, arenaSize.y));  // Right wall
            CreateWall(new Vector3(0, wallHeight/2, -arenaSize.y/2), new Vector3(arenaSize.x, wallHeight, 0.5f)); // Back wall
            CreateWall(new Vector3(0, wallHeight/2, arenaSize.y/2), new Vector3(arenaSize.x, wallHeight, 0.5f));  // Front wall

            // Create shooting lanes with targets
            float laneWidth = arenaSize.x / 4;
            for (int i = 0; i < 3; i++)
            {
                float xPos = (i - 1) * laneWidth;
                
                // Create lane dividers
                if (i < 2)
                {
                    CreateWall(new Vector3(xPos + laneWidth/2, wallHeight/2, 0), 
                             new Vector3(0.5f, wallHeight, arenaSize.y * 0.7f));
                }

                // Create targets at different distances
                for (float z = 5f; z < arenaSize.y/2 - 2f; z += targetSpacing)
                {
                    CreateTarget(new Vector3(xPos, 1.5f, z));
                }
            }
        }

        private void CreateWall(Vector3 position, Vector3 scale)
        {
            GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
            wall.transform.localScale = scale;

            // 发布墙体创建事件
            EventManager.Publish(EventNames.WALL_CREATED, 
                new WallCreatedEvent(wall, position, scale));
        }

        private void CreateTarget(Vector3 position)
        {
            GameObject target = Instantiate(targetPrefab, position, Quaternion.identity);
            
            // 发布目标创建事件
            EventManager.Publish(EventNames.TARGET_CREATED, 
                new TargetCreatedEvent(target, position));
        }
    }
}
