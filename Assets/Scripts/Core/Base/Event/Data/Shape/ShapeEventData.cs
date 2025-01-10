using UnityEngine;

namespace Core.Base.Event.Data.Shape
{
    /// <summary>
    /// 形状变形事件数据
    /// </summary>
    public class ShapeDeformEventData : EventDataBase
    {
        /// <summary>
        /// 目标形状
        /// </summary>
        public GameObject Shape { get; private set; }

        /// <summary>
        /// 变形类型
        /// </summary>
        public DeformType Type { get; private set; }

        /// <summary>
        /// 变形强度
        /// </summary>
        public float Intensity { get; private set; }

        /// <summary>
        /// 变形方向
        /// </summary>
        public Vector3 Direction { get; private set; }

        public ShapeDeformEventData(GameObject shape, DeformType type, 
            float intensity, Vector3 direction)
        {
            Shape = shape;
            Type = type;
            Intensity = intensity;
            Direction = direction;
        }
    }

    /// <summary>
    /// 形状状态事件数据
    /// </summary>
    public class ShapeStateEventData : EventDataBase
    {
        /// <summary>
        /// 目标形状
        /// </summary>
        public GameObject Shape { get; private set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public ShapeState NewState { get; private set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public ShapeState OldState { get; private set; }

        public ShapeStateEventData(GameObject shape, ShapeState newState, ShapeState oldState)
        {
            Shape = shape;
            NewState = newState;
            OldState = oldState;
        }
    }

    /// <summary>
    /// 变形类型枚举
    /// </summary>
    public enum DeformType
    {
        /// <summary>
        /// 拉伸
        /// </summary>
        Stretch,

        /// <summary>
        /// 压缩
        /// </summary>
        Compress,

        /// <summary>
        /// 扭曲
        /// </summary>
        Twist,

        /// <summary>
        /// 弯曲
        /// </summary>
        Bend
    }

    /// <summary>
    /// 形状状态枚举
    /// </summary>
    public enum ShapeState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 变形中
        /// </summary>
        Deforming,

        /// <summary>
        /// 破碎
        /// </summary>
        Broken,

        /// <summary>
        /// 消失
        /// </summary>
        Vanished
    }
} 