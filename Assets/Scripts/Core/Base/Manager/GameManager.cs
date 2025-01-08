using UnityEngine;
using System;
using System.Collections.Generic;

namespace Core.Base.Manager
{
    /// <summary>
    /// Core game manager that handles game state, time, and scoring
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        instance = go.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region State Management
        public enum GameState
        {
            MainMenu,
            Loading,
            Playing,
            Paused,
            GameOver
        }

        private GameState currentState;
        public GameState CurrentState
        {
            get => currentState;
            private set
            {
                if (currentState != value)
                {
                    GameState oldState = currentState;
                    currentState = value;
                    OnGameStateChanged?.Invoke(oldState, currentState);
                }
            }
        }

        public event Action<GameState, GameState> OnGameStateChanged;
        #endregion

        #region Time Management
        private float gameTime;
        private bool isTimePaused;

        public float GameTime => gameTime;
        public bool IsTimePaused => isTimePaused;

        public void PauseTime()
        {
            isTimePaused = true;
            Time.timeScale = 0f;
        }

        public void ResumeTime()
        {
            isTimePaused = false;
            Time.timeScale = 1f;
        }
        #endregion

        #region Score Management
        private int currentScore;
        private int highScore;

        public int CurrentScore => currentScore;
        public int HighScore => highScore;

        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;

        public void AddScore(int points)
        {
            currentScore += points;
            OnScoreChanged?.Invoke(currentScore);

            if (currentScore > highScore)
            {
                highScore = currentScore;
                OnHighScoreChanged?.Invoke(highScore);
                SaveHighScore();
            }
        }

        private void SaveHighScore()
        {
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Update()
        {
            if (!isTimePaused && CurrentState == GameState.Playing)
            {
                gameTime += Time.deltaTime;
            }
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            LoadHighScore();
            currentScore = 0;
            gameTime = 0f;
            isTimePaused = false;
            CurrentState = GameState.MainMenu;
        }
        #endregion

        #region Game Flow Control
        public void StartGame()
        {
            if (CurrentState != GameState.Playing)
            {
                Initialize();
                CurrentState = GameState.Playing;
                ResumeTime();
            }
        }

        public void PauseGame()
        {
            if (CurrentState == GameState.Playing)
            {
                CurrentState = GameState.Paused;
                PauseTime();
            }
        }

        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused)
            {
                CurrentState = GameState.Playing;
                ResumeTime();
            }
        }

        public void EndGame()
        {
            if (CurrentState == GameState.Playing || CurrentState == GameState.Paused)
            {
                CurrentState = GameState.GameOver;
                PauseTime();
            }
        }
        #endregion
    }
}
