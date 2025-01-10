using UnityEngine;

namespace Core.Base.Event.Data.Game
{
    /// <summary>
    /// 游戏状态事件数据
    /// </summary>
    public class GameStateEventData : EventDataBase
    {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public GameState State { get; private set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object Data { get; private set; }

        public GameStateEventData(GameState state, object data = null)
        {
            State = state;
            Data = data;
        }
    }

    /// <summary>
    /// 分数更新事件数据
    /// </summary>
    public class ScoreUpdateEventData : EventDataBase
    {
        /// <summary>
        /// 分数变化值
        /// </summary>
        public int ScoreChange { get; private set; }

        /// <summary>
        /// 当前总分
        /// </summary>
        public int TotalScore { get; private set; }

        /// <summary>
        /// 分数来源
        /// </summary>
        public string Source { get; private set; }

        public ScoreUpdateEventData(int scoreChange, int totalScore, string source = "")
        {
            ScoreChange = scoreChange;
            TotalScore = totalScore;
            Source = source;
        }
    }

    /// <summary>
    /// 游戏开始事件数据
    /// </summary>
    public class GameStartEvent : EventDataBase
    {
        /// <summary>
        /// 游戏难度
        /// </summary>
        public int Difficulty { get; private set; }

        /// <summary>
        /// 游戏模式
        /// </summary>
        public GameMode Mode { get; private set; }

        public GameStartEvent(int difficulty = 1, GameMode mode = GameMode.Normal)
        {
            Difficulty = difficulty;
            Mode = mode;
        }
    }

    /// <summary>
    /// 游戏状态枚举
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// 初始化
        /// </summary>
        Initialize,

        /// <summary>
        /// 游戏开始
        /// </summary>
        Start,

        /// <summary>
        /// 游戏进行中
        /// </summary>
        Playing,

        /// <summary>
        /// 游戏暂停
        /// </summary>
        Paused,

        /// <summary>
        /// 游戏胜利
        /// </summary>
        Victory,

        /// <summary>
        /// 游戏失败
        /// </summary>
        GameOver
    }

    /// <summary>
    /// 游戏模式枚举
    /// </summary>
    public enum GameMode
    {
        /// <summary>
        /// 普通模式
        /// </summary>
        Normal,

        /// <summary>
        /// 挑战模式
        /// </summary>
        Challenge,

        /// <summary>
        /// 练习模式
        /// </summary>
        Practice
    }

    /// <summary>
    /// 玩家生命值事件数据
    /// </summary>
    public class PlayerHealthEvent : EventDataBase
    {
        public int PlayerIndex { get; private set; }
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        public PlayerHealthEvent(int playerIndex, float currentHealth, float maxHealth)
        {
            PlayerIndex = playerIndex;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }

    /// <summary>
    /// 玩家出界事件数据
    /// </summary>
    public class PlayerOutOfBoundsEvent : EventDataBase
    {
        public int PlayerIndex { get; private set; }
        public Vector3 Position { get; private set; }
        public string FallReason { get; private set; }

        public PlayerOutOfBoundsEvent(int playerIndex, Vector3 position, string fallReason)
        {
            PlayerIndex = playerIndex;
            Position = position;
            FallReason = fallReason;
        }
    }

    /// <summary>
    /// 玩家胜利事件数据
    /// </summary>
    public class PlayerVictoryEvent : EventDataBase
    {
        public int WinnerIndex { get; private set; }
        public string VictoryReason { get; private set; }

        public PlayerVictoryEvent(int winnerIndex, string victoryReason)
        {
            WinnerIndex = winnerIndex;
            VictoryReason = victoryReason;
        }
    }

    /// <summary>
    /// 游戏结束事件数据
    /// </summary>
    public class GameOverEvent : EventDataBase
    {
        public bool IsNormalEnd { get; private set; }

        public GameOverEvent(bool isNormalEnd)
        {
            IsNormalEnd = isNormalEnd;
        }
    }
} 