using UnityEngine;

/// <summary>
/// Handles the behavior of the Level 2-3 transition shape in the middle section.
/// Transforms bullets entering from bottom into two new angled bullets of higher level.
/// Only interacts with level 1 and 2 bullets.
/// </summary>
public class Level2To3Shape : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float angleOffset = 30f; // Angle for new bullets
    [SerializeField] private float spawnOffset = 0.2f; // Horizontal offset for spawned bullets
    [SerializeField] private GameObject level2BulletPrefab;
    [SerializeField] private GameObject level3BulletPrefab;
    [SerializeField] private LayerMask bulletLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        var bullet = other.GetComponent<BaseBullet>();
        if (bullet == null || bullet.Level > 2)
            return;

        // Check if bullet is entering from bottom
        Vector3 bulletDirection = other.GetComponent<Rigidbody>().velocity.normalized;
        float entryAngle = Vector3.Angle(Vector3.up, bulletDirection);
        
        if (entryAngle < 45f) // Consider it entering from bottom if angle is less than 45 degrees
        {
            // Determine which prefab to use based on current bullet level
            GameObject newBulletPrefab = bullet.Level == 1 ? level2BulletPrefab : level3BulletPrefab;

            // Spawn two new bullets
            SpawnAngledBullet(newBulletPrefab, -angleOffset, -spawnOffset);
            SpawnAngledBullet(newBulletPrefab, angleOffset, spawnOffset);

            // Destroy the original bullet
            Destroy(other.gameObject);
        }
    }

    private void SpawnAngledBullet(GameObject prefab, float angle, float horizontalOffset)
    {
        // Calculate spawn position with offset
        Vector3 spawnPos = transform.position + new Vector3(horizontalOffset, 0f, 0f);

        // Instantiate new bullet
        GameObject newBullet = Instantiate(prefab, spawnPos, Quaternion.identity);
        
        // Calculate direction based on angle
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.up;
        
        // Set velocity for new bullet
        var rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 10f; // Use appropriate speed value
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize spawn points and angles in editor
        Gizmos.color = Color.green;
        Vector3 leftSpawn = transform.position + new Vector3(-spawnOffset, 0f, 0f);
        Vector3 rightSpawn = transform.position + new Vector3(spawnOffset, 0f, 0f);
        
        Gizmos.DrawWireSphere(leftSpawn, 0.1f);
        Gizmos.DrawWireSphere(rightSpawn, 0.1f);
        
        // Draw angle indicators
        Gizmos.DrawRay(leftSpawn, Quaternion.Euler(0f, -angleOffset, 0f) * Vector3.up);
        Gizmos.DrawRay(rightSpawn, Quaternion.Euler(0f, angleOffset, 0f) * Vector3.up);
    }
}
