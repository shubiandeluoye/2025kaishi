using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;
using UnityEngine.SceneManagement;

namespace Core.Scene.Management
{
    public class SceneManager : BaseManager
    {
        [Header("Scene Loading")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private float minimumLoadTime = 0.5f;

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<string>(EventManager.EventNames.LEVEL_STARTED, OnLevelStart);
            EventManager.Subscribe<string>(EventManager.EventNames.LEVEL_COMPLETED, OnLevelComplete);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<string>(EventManager.EventNames.LEVEL_STARTED, OnLevelStart);
            EventManager.Unsubscribe<string>(EventManager.EventNames.LEVEL_COMPLETED, OnLevelComplete);
        }

        private void OnLevelStart(string levelName)
        {
            StartCoroutine(LoadSceneRoutine(levelName));
        }

        private void OnLevelComplete(string nextLevel)
        {
            if (!string.IsNullOrEmpty(nextLevel))
            {
                EventManager.Publish(EventManager.EventNames.LEVEL_STARTED, nextLevel);
            }
        }

        private System.Collections.IEnumerator LoadSceneRoutine(string sceneName)
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(true);

            float startTime = Time.realtimeSinceStartup;

            var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                yield return null;
            }

            float elapsedTime = Time.realtimeSinceStartup - startTime;
            if (elapsedTime < minimumLoadTime)
            {
                yield return new WaitForSeconds(minimumLoadTime - elapsedTime);
            }

            operation.allowSceneActivation = true;

            if (loadingScreen != null)
                loadingScreen.SetActive(false);
        }

        public void LoadScene(string sceneName)
        {
            EventManager.Publish(EventManager.EventNames.LEVEL_STARTED, sceneName);
        }
    }
}
