using UnityEngine;
using Core.Base.Event;

namespace Core.Combat.ShootPoint
{
    public class ShootPointSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform characterModel;

        [Header("Settings")]
        [SerializeField] private Vector3 defaultOffset = new Vector3(0.5f, 0, 0);
        [SerializeField] private bool showDebugGizmos = true;

        public Vector3 Position => shootPoint.position;
        public Vector3 Direction => shootPoint.right;
        public Transform Transform => shootPoint;

        private void Awake()
        {
            InitializeShootPoint();
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            EventManager.Subscribe<ShootPointEvent>(EventNames.SHOOT_POINT_UPDATE, OnShootPointUpdate);
        }

        private void UnregisterEvents()
        {
            EventManager.Unsubscribe<ShootPointEvent>(EventNames.SHOOT_POINT_UPDATE, OnShootPointUpdate);
        }

        private void InitializeShootPoint()
        {
            if (shootPoint == null)
            {
                GameObject point = new GameObject("ShootPoint");
                shootPoint = point.transform;
                shootPoint.parent = transform;
                shootPoint.localPosition = defaultOffset;
            }

            if (characterModel == null)
            {
                characterModel = transform.Find("Capsule");
            }

            // 发布初始化事件
            EventManager.Publish(EventNames.SHOOT_POINT_INITIALIZED, 
                new ShootPointEvent(ShootPointEventType.Position, shootPoint.position));
        }

        private void OnShootPointUpdate(ShootPointEvent evt)
        {
            switch (evt.Type)
            {
                case ShootPointEventType.Direction:
                    SetDirection(evt.Direction);
                    break;
                case ShootPointEventType.Position:
                    UpdatePosition(evt.Position);
                    break;
            }
        }

        public void SetDirection(Vector3 direction)
        {
            shootPoint.right = direction;
            EventManager.Publish(EventNames.SHOOT_POINT_DIRECTION_CHANGED, 
                new ShootPointEvent(ShootPointEventType.Direction, direction));
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            shootPoint.position = newPosition;
            EventManager.Publish(EventNames.SHOOT_POINT_POSITION_CHANGED, 
                new ShootPointEvent(ShootPointEventType.Position, newPosition));
        }

        private void OnDrawGizmos()
        {
            if (!showDebugGizmos || shootPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPoint.position, 0.1f);
            Gizmos.DrawRay(shootPoint.position, shootPoint.right * 1f);
        }
    }
} 