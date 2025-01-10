using UnityEngine;
using Core.UI.Base;
using UnityEngine.UI;
using TMPro;
using Core.Base.Event;
using Core.UI.Events;
using Core.Base.Event.Data.UI;

namespace Core.UI.Menu
{
    public class MenuManager : BaseUIManager
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Main Menu")]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [Header("Pause Menu")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Settings")]
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Button backButton;

        [Header("Game Over")]
        [SerializeField] private TextMeshProUGUI winnerText;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button exitButton;

        protected override void Awake()
        {
            base.Awake();
            SetupButtonListeners();
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            EventManager.Subscribe<MenuStateEvent>(EventNames.MENU_SHOW, OnMenuShow);
            EventManager.Subscribe<MenuStateEvent>(EventNames.MENU_HIDE, OnMenuHide);
            EventManager.Subscribe<UIEvent>(EventNames.GAME_PAUSE, OnGamePause);
            EventManager.Subscribe<UIEvent>(EventNames.GAME_RESUME, OnGameResume);
        }

        private void SetupButtonListeners()
        {
            // Main Menu
            startGameButton?.onClick.AddListener(OnStartGame);
            settingsButton?.onClick.AddListener(ShowSettings);
            quitButton?.onClick.AddListener(OnQuit);

            // Pause Menu
            resumeButton?.onClick.AddListener(OnResume);
            restartButton?.onClick.AddListener(OnRestart);
            mainMenuButton?.onClick.AddListener(ReturnToMainMenu);

            // Settings
            backButton?.onClick.AddListener(HideSettings);
            if (volumeSlider != null)
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);

            // Game Over
            playAgainButton?.onClick.AddListener(OnPlayAgain);
            exitButton?.onClick.AddListener(ReturnToMainMenu);
        }

        public void ShowMainMenu()
        {
            HideAllPanels();
            mainMenuPanel?.SetActive(true);
        }

        public void ShowPauseMenu()
        {
            HideAllPanels();
            pauseMenuPanel?.SetActive(true);
        }

        public void ShowSettings()
        {
            HideAllPanels();
            settingsPanel?.SetActive(true);
        }

        public void ShowGameOver(string winner)
        {
            HideAllPanels();
            gameOverPanel?.SetActive(true);
            if (winnerText != null)
                winnerText.text = $"Winner: {winner}";
        }

        private void HideAllPanels()
        {
            mainMenuPanel?.SetActive(false);
            pauseMenuPanel?.SetActive(false);
            settingsPanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
        }

        private void HideSettings()
        {
            settingsPanel?.SetActive(false);
            mainMenuPanel?.SetActive(true);
        }

        // Button Actions
        private void OnStartGame()
        {
            // Implement game start logic
        }

        private void OnResume()
        {
            // Implement resume logic
        }

        private void OnRestart()
        {
            // Implement restart logic
        }

        private void OnQuit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void ReturnToMainMenu()
        {
            // Implement return to main menu logic
        }

        private void OnPlayAgain()
        {
            // Implement play again logic
        }

        private void OnVolumeChanged(float value)
        {
            // Implement volume change logic
        }

        private void OnFullscreenToggled(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        private void OnMenuShow(MenuStateEvent evt)
        {
            string menuName = evt.MenuName;
            ShowUI(menuName);
        }

        private void OnMenuHide(MenuStateEvent evt)
        {
            string menuName = evt.MenuName;
            HideUI(menuName);
        }

        private void OnGamePause(UIEvent evt)
        {
            ShowUI("PauseMenu");
            Time.timeScale = 0;
        }

        private void OnGameResume(UIEvent evt)
        {
            HideUI("PauseMenu");
            Time.timeScale = 1;
        }
    }
}
