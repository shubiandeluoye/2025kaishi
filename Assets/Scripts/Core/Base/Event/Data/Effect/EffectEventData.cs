using UnityEngine;

namespace Core.Base.Event.Data.Effect
{
    /// <summary>
    /// 特效播放事件数据
    /// </summary>
    public class EffectPlayEventData : EventDataBase
    {
        /// <summary>
        /// 特效ID
        /// </summary>
        public string EffectId { get; private set; }

        /// <summary>
        /// 播放位置
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// 播放旋转
        /// </summary>
        public Quaternion Rotation { get; private set; }

        /// <summary>
        /// 播放缩放
        /// </summary>
        public Vector3 Scale { get; private set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 附加对象
        /// </summary>
        public GameObject AttachTarget { get; private set; }

        public EffectPlayEventData(string effectId, Vector3 position, 
            Quaternion rotation, Vector3 scale, float duration = 0, GameObject attachTarget = null)
        {
            EffectId = effectId;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Duration = duration;
            AttachTarget = attachTarget;
        }
    }

    /// <summary>
    /// 特效参数更新事件数据
    /// </summary>
    public class EffectParamEventData : EventDataBase
    {
        /// <summary>
        /// 特效对象
        /// </summary>
        public GameObject EffectObject { get; private set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; private set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object ParamValue { get; private set; }

        /// <summary>
        /// 更新类型
        /// </summary>
        public EffectParamType ParamType { get; private set; }

        public EffectParamEventData(GameObject effectObject, string paramName, 
            object paramValue, EffectParamType paramType)
        {
            EffectObject = effectObject;
            ParamName = paramName;
            ParamValue = paramValue;
            ParamType = paramType;
        }
    }

    /// <summary>
    /// 特效系统初始化事件数据
    /// </summary>
    public class EffectSystemInitEventData : EventDataBase
    {
        /// <summary>
        /// 初始化配置
        /// </summary>
        public EffectInitConfig Config { get; private set; }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        public EffectSystemInitEventData(EffectInitConfig config, bool success, string errorMessage = "")
        {
            Config = config;
            Success = success;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// 特效参数类型枚举
    /// </summary>
    public enum EffectParamType
    {
        /// <summary>
        /// 颜色
        /// </summary>
        Color,

        /// <summary>
        /// 速度
        /// </summary>
        Speed,

        /// <summary>
        /// 大小
        /// </summary>
        Size,

        /// <summary>
        /// 强度
        /// </summary>
        Intensity
    }

    /// <summary>
    /// 特效初始化配置
    /// </summary>
    public class EffectInitConfig
    {
        /// <summary>
        /// 最大特效数量
        /// </summary>
        public int MaxEffectCount { get; set; }

        /// <summary>
        /// 是否使用对象池
        /// </summary>
        public bool UseObjectPool { get; set; }

        /// <summary>
        /// 预加载特效列表
        /// </summary>
        public string[] PreloadEffects { get; set; }
    }
} 