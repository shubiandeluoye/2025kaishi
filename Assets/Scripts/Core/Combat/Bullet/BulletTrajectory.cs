using UnityEngine;
using Core.Base.Event;
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
            
            // 发布轨迹初始化事件
            EventManager.Publish(EventNames.BULLET_TRAJECTORY_INITIALIZED, 
                new BulletTrajectoryEvent(gameObject, direction, angle, isClockwise));
        }

        private void Update()
        {
            if (baseBullet == null) return;

            elapsedTime += Time.deltaTime;
            float currentAngle = angle * (isClockwise ? -1 : 1);
            
            Vector3 rotatedDirection = Quaternion.Euler(0f, currentAngle, 0f) * initialDirection;
            
            if (GetComponent<Rigidbody>() is Rigidbody rb)
            {
                rb.velocity = rotatedDirection * baseBullet.Settings.speed;
                
                // 发布轨迹更新事件
                EventManager.Publish(EventNames.BULLET_TRAJECTORY_UPDATED, 
                    new BulletTrajectoryEvent(gameObject, rotatedDirection, currentAngle, isClockwise));
            }
        }

        public void SetAngle(float newAngle, bool clockwise)
        {
            angle = newAngle;
            isClockwise = clockwise;
            
            // 发布轨迹修改事件
            EventManager.Publish(EventNames.BULLET_TRAJECTORY_MODIFIED, 
                new BulletTrajectoryEvent(gameObject, initialDirection, newAngle, clockwise));
        }
    }

    public class BulletTrajectoryEvent
    {
        public GameObject Bullet { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Angle { get; private set; }
        public bool IsClockwise { get; private set; }

        public BulletTrajectoryEvent(GameObject bullet, Vector3 direction, float angle, bool isClockwise)
        {
            Bullet = bullet;
            Direction = direction;
            Angle = angle;
            IsClockwise = isClockwise;
        }
    }
}