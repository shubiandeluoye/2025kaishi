using UnityEngine;

namespace Core.Base.Event.Data.Visual
{
    /// <summary>
    /// 基础效果事件数据
    /// </summary>
    public class EffectEventData : EventDataBase
    {
        /// <summary>
        /// 效果名称
        /// </summary>
        public string EffectName { get; private set; }

        /// <summary>
        /// 效果位置
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// 效果旋转
        /// </summary>
        public Quaternion Rotation { get; private set; }

        /// <summary>
        /// 效果持续时间
        /// </summary>
        public float Duration { get; private set; }

        public EffectEventData(string effectName, Vector3 position, 
            Quaternion rotation = default, float duration = 0)
        {
            EffectName = effectName;
            Position = position;
            Rotation = rotation;
            Duration = duration;
        }
    }

    /// <summary>
    /// 效果进度事件数据
    /// </summary>
    public class EffectProgressEventData : EventDataBase
    {
        /// <summary>
        /// 效果名称
        /// </summary>
        public string EffectName { get; private set; }

        /// <summary>
        /// 当前进度(0-1)
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 效果对象引用
        /// </summary>
        public GameObject EffectObject { get; private set; }

        public EffectProgressEventData(string effectName, float progress, GameObject effectObject)
        {
            EffectName = effectName;
            Progress = progress;
            EffectObject = effectObject;
        }
    }

    /// <summary>
    /// 效果对象池事件数据
    /// </summary>
    public class EffectPoolEventData : EventDataBase
    {
        /// <summary>
        /// 效果名称
        /// </summary>
        public string EffectName { get; private set; }

        /// <summary>
        /// 池中对象数量
        /// </summary>
        public int PoolSize { get; private set; }

        public EffectPoolEventData(string effectName, int poolSize)
        {
            EffectName = effectName;
            PoolSize = poolSize;
        }
    }
} 