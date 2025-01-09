using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Base.Event;

namespace Core.Scene.Management
{
    public class SceneManager : MonoBehaviour
    {
        [Header("Scene Settings")]
        [SerializeField] private float transitionDuration = 1f;
        
        private static SceneManager instance;
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SceneManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("SceneManager");
                        instance = go.AddComponent<SceneManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
        {
            // Trigger before scene load event
            EventManager.Trigger(new SceneLoadEvent(SceneLoadState.Begin, sceneName));

            // Start scene loading
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            // Wait for transition duration
            yield return new WaitForSeconds(transitionDuration);

            // Complete scene loading
            asyncLoad.allowSceneActivation = true;
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Trigger after scene load event
            EventManager.Trigger(new SceneLoadEvent(SceneLoadState.Complete, sceneName));
        }
    }

    public enum SceneLoadState
    {
        Begin,
        Complete
    }

    public class SceneLoadEvent
    {
        public SceneLoadState State { get; private set; }
        public string SceneName { get; private set; }

        public SceneLoadEvent(SceneLoadState state, string sceneName)
        {
            State = state;
            SceneName = sceneName;
        }
    }
}
