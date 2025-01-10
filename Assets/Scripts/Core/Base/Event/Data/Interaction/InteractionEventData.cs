using UnityEngine;

namespace Core.Base.Event.Data.Interaction
{
    /// <summary>
    /// 交互事件数据基类
    /// </summary>
    public class InteractionEventData : EventDataBase
    {
        /// <summary>
        /// 交互发起者
        /// </summary>
        public GameObject Initiator { get; private set; }

        /// <summary>
        /// 交互目标
        /// </summary>
        public GameObject Target { get; private set; }

        /// <summary>
        /// 交互点
        /// </summary>
        public Vector3 InteractionPoint { get; private set; }

        /// <summary>
        /// 交互类型
        /// </summary>
        public InteractionType Type { get; private set; }

        public InteractionEventData(GameObject initiator, GameObject target, 
            Vector3 interactionPoint, InteractionType type)
        {
            Initiator = initiator;
            Target = target;
            InteractionPoint = interactionPoint;
            Type = type;
        }
    }

    /// <summary>
    /// 形状碰撞事件数据
    /// </summary>
    public class ShapeCollisionEventData : EventDataBase
    {
        /// <summary>
        /// 碰撞形状A
        /// </summary>
        public GameObject ShapeA { get; private set; }

        /// <summary>
        /// 碰撞形状B
        /// </summary>
        public GameObject ShapeB { get; private set; }

        /// <summary>
        /// 碰撞点
        /// </summary>
        public Vector3 CollisionPoint { get; private set; }

        /// <summary>
        /// 碰撞法线
        /// </summary>
        public Vector3 Normal { get; private set; }

        /// <summary>
        /// 碰撞强度
        /// </summary>
        public float Force { get; private set; }

        public ShapeCollisionEventData(GameObject shapeA, GameObject shapeB, 
            Vector3 collisionPoint, Vector3 normal, float force)
        {
            ShapeA = shapeA;
            ShapeB = shapeB;
            CollisionPoint = collisionPoint;
            Normal = normal;
            Force = force;
        }
    }

    /// <summary>
    /// 交互类型枚举
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// 触发
        /// </summary>
        Trigger,

        /// <summary>
        /// 碰撞
        /// </summary>
        Collision,

        /// <summary>
        /// 点击
        /// </summary>
        Click,

        /// <summary>
        /// 悬停
        /// </summary>
        Hover
    }
} 