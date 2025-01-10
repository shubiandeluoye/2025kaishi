using UnityEngine;

namespace Core.Base.Event.Data.Input
{
    /// <summary>
    /// 触摸输入事件数据
    /// </summary>
    public class TouchInputEventData : EventDataBase
    {
        /// <summary>
        /// 触摸位置
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// 触摸阶段
        /// </summary>
        public TouchPhase Phase { get; private set; }

        /// <summary>
        /// 触摸手指ID
        /// </summary>
        public int FingerId { get; private set; }

        public TouchInputEventData(Vector2 position, TouchPhase phase, int fingerId)
        {
            Position = position;
            Phase = phase;
            FingerId = fingerId;
        }
    }

    /// <summary>
    /// 键盘输入事件数据
    /// </summary>
    public class KeyboardInputEventData : EventDataBase
    {
        /// <summary>
        /// 按键码
        /// </summary>
        public KeyCode KeyCode { get; private set; }

        /// <summary>
        /// 按键状态
        /// </summary>
        public KeyState State { get; private set; }

        public KeyboardInputEventData(KeyCode keyCode, KeyState state)
        {
            KeyCode = keyCode;
            State = state;
        }
    }

    /// <summary>
    /// 鼠标输入事件数据
    /// </summary>
    public class MouseInputEventData : EventDataBase
    {
        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// 鼠标按键
        /// </summary>
        public int Button { get; private set; }

        /// <summary>
        /// 鼠标滚轮值
        /// </summary>
        public float ScrollDelta { get; private set; }

        public MouseInputEventData(Vector2 position, int button = -1, float scrollDelta = 0)
        {
            Position = position;
            Button = button;
            ScrollDelta = scrollDelta;
        }
    }

    /// <summary>
    /// 玩家输入事件数据
    /// </summary>
    public class PlayerInputEventData : EventDataBase
    {
        /// <summary>
        /// 输入类型
        /// </summary>
        public PlayerInputType InputType { get; private set; }

        /// <summary>
        /// 输入值
        /// </summary>
        public Vector2 InputValue { get; private set; }

        public PlayerInputEventData(PlayerInputType inputType, Vector2 inputValue)
        {
            InputType = inputType;
            InputValue = inputValue;
        }
    }

    /// <summary>
    /// 按键状态枚举
    /// </summary>
    public enum KeyState
    {
        /// <summary>
        /// 按下
        /// </summary>
        Down,

        /// <summary>
        /// 持续按住
        /// </summary>
        Hold,

        /// <summary>
        /// 释放
        /// </summary>
        Up
    }

    /// <summary>
    /// 玩家输入类型枚举
    /// </summary>
    public enum PlayerInputType
    {
        /// <summary>
        /// 移动
        /// </summary>
        Move,

        /// <summary>
        /// 射击
        /// </summary>
        Shoot,

        /// <summary>
        /// 角度切换
        /// </summary>
        AngleToggle
    }
} 