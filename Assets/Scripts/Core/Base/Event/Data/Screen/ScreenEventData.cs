using UnityEngine;

namespace Core.Base.Event.Data.Screen
{
    /// <summary>
    /// 屏幕效果事件数据
    /// </summary>
    public class ScreenEventData : EventDataBase
    {
        /// <summary>
        /// 震动强度
        /// </summary>
        public float ShakeIntensity { get; set; }

        /// <summary>
        /// 震动持续时间
        /// </summary>
        public float ShakeDuration { get; set; }

        /// <summary>
        /// 是否使用黑边
        /// </summary>
        public bool UseLetterbox { get; set; }

        /// <summary>
        /// 黑边持续时间
        /// </summary>
        public float LetterboxDuration { get; set; }
    }
} 