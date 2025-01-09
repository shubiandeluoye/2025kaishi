using UnityEngine;
using Core.Combat.Bullet;

namespace Core.Combat.Bullet
{
    public class BulletTrajectory : MonoBehaviour
    {
        [SerializeField] private float angle = 0f;
        [SerializeField] private bool isClockwise = true;
        private BaseBullet baseBullet;
        private Vector3 initialDirection;
        private float elapsedTime;

        private void Awake()
        {
            baseBullet = GetComponent<BaseBullet>();
        }

        public void Initialize(Vector3 direction)
        {
            initialDirection = direction.normalized;
            elapsedTime = 0f;
        }

        private void Update()
        {
            if (baseBullet == null) return;

            elapsedTime += Time.deltaTime;
            float currentAngle = angle * (isClockwise ? -1 : 1);
            
            // Rotate the direction vector around Y axis by the specified angle
            Vector3 rotatedDirection = Quaternion.Euler(0f, currentAngle, 0f) * initialDirection;
            
            // Update bullet velocity using the public Settings property
            if (GetComponent<Rigidbody>() is Rigidbody rb)
            {
                rb.velocity = rotatedDirection * baseBullet.Settings.speed;
            }
        }

        public void SetAngle(float newAngle, bool clockwise)
        {
            angle = newAngle;
            isClockwise = clockwise;
        }
    }
}