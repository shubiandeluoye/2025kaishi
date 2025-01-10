using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Base.Event
{
    /// <summary>
    /// 事件分发器
    /// 负责事件的订阅、取消订阅和分发
    /// </summary>
    public class EventDispatcher
    {
        /// <summary>
        /// 存储所有事件处理器的字典
        /// </summary>
        private Dictionary<string, List<Delegate>> eventHandlers = new Dictionary<string, List<Delegate>>();

        /// <summary>
        /// 事件队列，用于异步事件处理
        /// </summary>
        private Queue<EventQueueItem> eventQueue = new Queue<EventQueueItem>();

        /// <summary>
        /// 表示当前是否正在处理事件队列
        /// </summary>
        private bool isProcessingQueue = false;

        /// <summary>
        /// 队列中的事件项
        /// </summary>
        private class EventQueueItem
        {
            public string EventName;
            public EventDataBase Data;
            public bool Immediate;
        }

        // ... 其他实现方法
    }
} 