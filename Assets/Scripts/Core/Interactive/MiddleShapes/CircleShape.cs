using UnityEngine;

/// <summary>
/// Handles the behavior of the circle shape in the middle section.
/// Only interacts with level 1 and 2 bullets.
/// </summary>
public class CircleShape : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int maxHits = 20;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask bulletLayers;

    private int hitCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        var bullet = other.GetComponent<BaseBullet>();
        if (bullet == null || bullet.Level > 2)
            return;

        hitCount++;
        Destroy(other.gameObject);

        if (hitCount >= maxHits)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Find all bullets within explosion radius
        var hits = Physics.OverlapSphere(transform.position, explosionRadius, bulletLayers);
        
        foreach (var hit in hits)
        {
            var bullet = hit.GetComponent<BaseBullet>();
            if (bullet != null && bullet.Level <= 2)
            {
                Destroy(hit.gameObject);
            }
        }

        // Optionally destroy this shape after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize explosion radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
