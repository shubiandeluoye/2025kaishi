using UnityEngine;
using Core.Interactive.Base;
using Core.Interactive.MiddleShapes;

namespace Core.Scene.Layout
{
    public class InteractionSetup : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private GameObject[] shapePresets;
        [SerializeField] private Transform[] interactionPoints;

        private void Start()
        {
            SetupInteractions();
        }

        private void SetupInteractions()
        {
            if (shapePresets == null || shapePresets.Length == 0)
                return;

            foreach (Transform point in interactionPoints)
            {
                if (point != null)
                {
                    SpawnInteractiveShape(point);
                }
            }
        }

        private void SpawnInteractiveShape(Transform spawnPoint)
        {
            if (shapePresets.Length == 0)
                return;

            int randomIndex = Random.Range(0, shapePresets.Length);
            GameObject shapePrefab = shapePresets[randomIndex];

            if (shapePrefab != null)
            {
                GameObject shape = Instantiate(shapePrefab, spawnPoint.position, spawnPoint.rotation);
                shape.transform.SetParent(spawnPoint);

                BaseInteractive interactive = shape.GetComponent<BaseInteractive>();
                if (interactive != null)
                {
                    EventManager.Publish(EventNames.INTERACTIVE_SPAWNED, 
                        new InteractiveSpawnedEvent(interactive, spawnPoint.position));
                }
            }
        }
    }
}
