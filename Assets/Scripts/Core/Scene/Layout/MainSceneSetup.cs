using UnityEngine;
using Core.Game;
using Core.Base.Event;

namespace Core.Scene
{
    /// <summary>
    /// 处理主游戏场景的设置和配置
    /// </summary>
    public class MainSceneSetup : MonoBehaviour
    {
        [Header("Scene Dimensions")]
        [SerializeField] private float sceneWidth = 17f;
        [SerializeField] private float sceneHeight = 10f;
        [SerializeField] private float centralAreaWidth = 3f;
        [SerializeField] private float playerAreaSize = 7f;
        [SerializeField] private float uiAreaHeight = 1.5f;

        [Header("Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject playerPrefab;

        private void Start()
        {
            SetupScene();
        }

        private void SetupScene()
        {
            // Create central area (3x10)
            CreateArea(Vector3.zero, centralAreaWidth, sceneHeight, floorPrefab, "CentralArea");

            // Create left player area (7x7)
            Vector3 leftAreaPos = new Vector3(-5f, 0f, 0f);
            CreateArea(leftAreaPos, playerAreaSize, playerAreaSize, floorPrefab, "LeftPlayerArea");

            // Create right player area (7x7)
            Vector3 rightAreaPos = new Vector3(5f, 0f, 0f);
            CreateArea(rightAreaPos, playerAreaSize, playerAreaSize, floorPrefab, "RightPlayerArea");

            // Create UI areas
            Vector3 topUIPos = new Vector3(0f, 4.25f, 0f);
            CreateArea(topUIPos, playerAreaSize, uiAreaHeight, null, "TopUIArea");

            Vector3 bottomUIPos = new Vector3(0f, -4.25f, 0f);
            CreateArea(bottomUIPos, playerAreaSize, uiAreaHeight, null, "BottomUIArea");

            CreateBoundaryWalls();
            SetupManagers();
        }

        private void CreateArea(Vector3 position, float width, float height, GameObject prefab, string areaName)
        {
            GameObject area = Instantiate(prefab, position, Quaternion.identity);
            area.name = areaName;
            area.transform.localScale = new Vector3(width, 1f, height);

            // 发布区域创建事件
            EventManager.Publish(EventNames.AREA_CREATED, 
                new AreaCreatedEvent(area, areaName, position, new Vector2(width, height)));
        }

        private void CreateBoundaryWalls()
        {
            if (wallPrefab == null) return;

            // Left wall
            CreateWall(new Vector3(-8.5f, 0f, 0f), sceneHeight);
            // Right wall
            CreateWall(new Vector3(8.5f, 0f, 0f), sceneHeight);
            // Top wall
            CreateWall(new Vector3(0f, 5f, 0f), sceneWidth, true);
            // Bottom wall
            CreateWall(new Vector3(0f, -5f, 0f), sceneWidth, true);
        }

        private void CreateWall(Vector3 position, float size, bool horizontal = false)
        {
            GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
            wall.name = $"Wall_{position}";
            wall.transform.localScale = horizontal ? 
                new Vector3(size, 1f, 1f) : 
                new Vector3(1f, 1f, size);
        }

        private void SetupManagers()
        {
            // Ensure GameManager exists
            if (FindObjectOfType<GameManager>() == null)
            {
                GameObject gameManagerObj = new GameObject("GameManager");
                gameManagerObj.AddComponent<GameManager>();
            }

            // Ensure VictoryManager exists
            if (FindObjectOfType<VictoryManager>() == null)
            {
                GameObject victoryManagerObj = new GameObject("VictoryManager");
                victoryManagerObj.AddComponent<VictoryManager>();
            }
        }
    }
}
