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

        #region Prefab References
        [Header("Bullet Prefabs")]
        [SerializeField] private GameObject standardBulletPrefab;
        [SerializeField] private GameObject level1BulletPrefab;
        [SerializeField] private GameObject level2BulletPrefab;
        [SerializeField] private GameObject level3BulletPrefab;
        [SerializeField] private GameObject angle30ClockwiseBulletPrefab;
        [SerializeField] private GameObject angle30CounterClockwiseBulletPrefab;
        [SerializeField] private GameObject angle45ClockwiseBulletPrefab;
        [SerializeField] private GameObject angle45CounterClockwiseBulletPrefab;
        [SerializeField] private GameObject explosiveBulletPrefab;
        [SerializeField] private GameObject homingBulletPrefab;

        [Header("Environment Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject targetPrefab;
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
            RegisterPrefabs();
        }

        private void RegisterPrefabs()
        {
            if (PoolManager.Instance == null) return;

            // Register bullet prefabs
            if (standardBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Bullet", standardBulletPrefab);
            
            if (level1BulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Level1Bullet", level1BulletPrefab);
            
            if (level2BulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Level2Bullet", level2BulletPrefab);
            
            if (level3BulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Level3Bullet", level3BulletPrefab);
            
            if (angle30ClockwiseBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Angle30ClockwiseBullet", angle30ClockwiseBulletPrefab);
            
            if (angle30CounterClockwiseBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Angle30CounterClockwiseBullet", angle30CounterClockwiseBulletPrefab);
            
            if (angle45ClockwiseBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Angle45ClockwiseBullet", angle45ClockwiseBulletPrefab);
            
            if (angle45CounterClockwiseBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("Angle45CounterClockwiseBullet", angle45CounterClockwiseBulletPrefab);
            
            if (explosiveBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("ExplosiveBullet", explosiveBulletPrefab);
            
            if (homingBulletPrefab != null)
                PoolManager.Instance.RegisterPrefab("HomingBullet", homingBulletPrefab);

            // Register environment prefabs
            if (floorPrefab != null)
                PoolManager.Instance.RegisterPrefab("Floor", floorPrefab);
            
            if (wallPrefab != null)
                PoolManager.Instance.RegisterPrefab("Wall", wallPrefab);
            
            if (targetPrefab != null)
                PoolManager.Instance.RegisterPrefab("Target", targetPrefab);
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
