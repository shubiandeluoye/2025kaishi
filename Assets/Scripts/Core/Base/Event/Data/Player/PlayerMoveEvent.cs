using UnityEngine;

namespace Core.Base.Event.Data.Player
{
    /// <summary>
    /// 玩家移动事件数据
    /// </summary>
    public class PlayerMoveEvent : EventDataBase
    {
        /// <summary>
        /// 移动方向
        /// </summary>
        public Vector2 MoveDirection { get; private set; }

        public PlayerMoveEvent(Vector2 moveDirection)
        {
            MoveDirection = moveDirection;
        }
    }
} 