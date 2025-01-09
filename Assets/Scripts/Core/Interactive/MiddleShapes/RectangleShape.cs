using UnityEngine;

/// <summary>
/// Handles the behavior of the rectangle shape in the middle section.
/// Creates a 0.5x0.5 disappearance area and destroys after 30 seconds.
/// Only interacts with level 1 and 2 bullets.
/// </summary>
public class RectangleShape : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float lifespan = 30f;
    [SerializeField] private Vector3 disappearanceAreaSize = new Vector3(0.5f, 1f, 0.5f);
    [SerializeField] private LayerMask bulletLayers;

    private float timer = 0f;
    private BoxCollider disappearanceArea;

    private void Start()
    {
        // Create disappearance area as child object
        var disappearanceObj = new GameObject("DisappearanceArea");
        disappearanceObj.transform.SetParent(transform);
        disappearanceObj.transform.localPosition = Vector3.zero;
        disappearanceObj.transform.localRotation = Quaternion.identity;
        disappearanceObj.transform.localScale = disappearanceAreaSize;

        // Add collider for bullet detection
        disappearanceArea = disappearanceObj.AddComponent<BoxCollider>();
        disappearanceArea.isTrigger = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        var bullet = other.GetComponent<BaseBullet>();
        if (bullet == null || bullet.Level > 2)
            return;

        // Only destroy bullets that enter the disappearance area
        if (other.bounds.Intersects(disappearanceArea.bounds))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize disappearance area in editor
        Gizmos.color = Color.yellow;
        if (disappearanceArea != null)
        {
            Gizmos.matrix = disappearanceArea.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}
