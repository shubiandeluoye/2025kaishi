namespace Core.Base.Event.Data.Combat
{
    /// <summary>
    /// 角度切换事件数据
    /// </summary>
    public class AngleToggleEvent : EventDataBase
    {
        /// <summary>
        /// 是否启用角度
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// 角度值
        /// </summary>
        public float Angle { get; private set; }

        public AngleToggleEvent(bool isEnabled, float angle = 0)
        {
            IsEnabled = isEnabled;
            Angle = angle;
        }
    }
} 