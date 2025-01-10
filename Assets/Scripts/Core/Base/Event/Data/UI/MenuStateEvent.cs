using UnityEngine;

namespace Core.Base.Event.Data.UI
{
    /// <summary>
    /// 菜单状态事件数据
    /// </summary>
    public class MenuStateEvent : EventDataBase
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; private set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public MenuState NewState { get; private set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public MenuState OldState { get; private set; }

        /// <summary>
        /// 菜单对象
        /// </summary>
        public GameObject MenuObject { get; private set; }

        public MenuStateEvent(string menuName, MenuState newState, MenuState oldState, GameObject menuObject = null)
        {
            MenuName = menuName;
            NewState = newState;
            OldState = oldState;
            MenuObject = menuObject;
        }
    }

    /// <summary>
    /// 菜单状态枚举
    /// </summary>
    public enum MenuState
    {
        /// <summary>
        /// 关闭
        /// </summary>
        Closed,

        /// <summary>
        /// 打开中
        /// </summary>
        Opening,

        /// <summary>
        /// 已打开
        /// </summary>
        Opened,

        /// <summary>
        /// 关闭中
        /// </summary>
        Closing
    }
} 