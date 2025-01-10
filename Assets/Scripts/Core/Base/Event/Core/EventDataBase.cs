namespace Core.Base.Event
{
    /// <summary>
    /// 事件数据基类
    /// 所有事件数据类型都必须继承自此类
    /// </summary>
    public abstract class EventDataBase 
    {
        /// <summary>
        /// 事件发生的时间戳
        /// </summary>
        public float Timestamp { get; private set; }

        protected EventDataBase()
        {
            Timestamp = UnityEngine.Time.time;
        }
    }
} 