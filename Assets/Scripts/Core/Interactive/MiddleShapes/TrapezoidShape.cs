using UnityEngine;

/// <summary>
/// Handles the behavior of the trapezoid shape in the middle section.
/// Reflects bullets upon collision and rotates at a defined speed.
/// Only interacts with level 1 and 2 bullets.
/// </summary>
public class TrapezoidShape : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float rotationSpeed = 45f; // degrees per second
    [SerializeField] private float lifespan = 20f;
    [SerializeField] private LayerMask bulletLayers;

    private float timer = 0f;

    private void Update()
    {
        // Rotate the shape
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Handle lifetime
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

        // Get bullet's rigidbody for reflection
        var bulletRb = other.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            // Calculate reflection vector based on collision normal
            Vector3 normal = transform.up; // Using up vector as reflection normal
            Vector3 velocity = bulletRb.velocity;
            Vector3 reflection = Vector3.Reflect(velocity, normal);

            // Apply reflected velocity
            bulletRb.velocity = reflection;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize rotation axis in editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 2f);
    }
}
