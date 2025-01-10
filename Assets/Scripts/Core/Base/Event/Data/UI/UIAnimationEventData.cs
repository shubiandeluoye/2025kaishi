namespace Core.Base.Event.Data.UI
{
    /// <summary>
    /// UI动画事件数据
    /// </summary>
    public class UIAnimationEventData : EventDataBase
    {
        /// <summary>
        /// UI元素名称
        /// </summary>
        public string ElementName { get; private set; }

        /// <summary>
        /// 动画是否完成
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// 动画持续时间
        /// </summary>
        public float Duration { get; private set; }

        public UIAnimationEventData(string elementName, bool isComplete, float duration = 0)
        {
            ElementName = elementName;
            IsComplete = isComplete;
            Duration = duration;
        }
    }
} 