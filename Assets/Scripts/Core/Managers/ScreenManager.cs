using UnityEngine;
using Core.Base.Event;
using Core.Base.Manager;

namespace Core.Managers
{
    public class ScreenManager : BaseManager
    {
        [Header("Camera")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float maxShakeIntensity = 0.5f;
        
        [Header("Letterbox")]
        [SerializeField] private GameObject letterboxPrefab;
        [SerializeField] private float letterboxHeight = 0.1f;
        
        private Vector3 originalCameraPosition;
        private Coroutine shakeCoroutine;
        private Coroutine letterboxCoroutine;

        protected override void OnManagerAwake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            originalCameraPosition = mainCamera.transform.localPosition;
        }

        protected override void RegisterEvents()
        {
            EventManager.Subscribe<ScreenEventData>(EventNames.HIGHLIGHT_START, OnScreenShake);
            EventManager.Subscribe<ScreenEventData>(EventNames.BULLET_HIT, OnBulletImpact);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Unsubscribe<EventManager.ScreenEventData>(EventManager.EventNames.HIGHLIGHT_START, OnScreenShake);
            EventManager.Unsubscribe<EventManager.ScreenEventData>(EventManager.EventNames.BULLET_HIT, OnBulletImpact);
        }

        private void OnScreenShake(EventManager.ScreenEventData data)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(ShakeCamera(data));

            if (data.UseLetterbox)
                ShowLetterbox(data.LetterboxDuration);
        }

        private void OnBulletImpact(EventManager.ScreenEventData data)
        {
            var shakeData = new EventManager.ScreenEventData
            {
                ShakeDuration = 0.2f,
                ShakeIntensity = 0.3f,
                UseLetterbox = false
            };
            OnScreenShake(shakeData);
        }

        private System.Collections.IEnumerator ShakeCamera(EventManager.ScreenEventData data)
        {
            float elapsed = 0f;
            
            while (elapsed < data.ShakeDuration)
            {
                float intensity = Mathf.Lerp(data.ShakeIntensity, 0f, elapsed / data.ShakeDuration);
                intensity = Mathf.Min(intensity, maxShakeIntensity);
                mainCamera.transform.localPosition = originalCameraPosition + Random.insideUnitSphere * intensity;
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            mainCamera.transform.localPosition = originalCameraPosition;
        }

        private void ShowLetterbox(float duration)
        {
            if (letterboxCoroutine != null)
                StopCoroutine(letterboxCoroutine);
            letterboxCoroutine = StartCoroutine(LetterboxRoutine(duration));
        }

        private System.Collections.IEnumerator LetterboxRoutine(float duration)
        {
            var topBar = Instantiate(letterboxPrefab);
            var bottomBar = Instantiate(letterboxPrefab);
            
            topBar.transform.position = new Vector3(0, 1 - letterboxHeight, 0);
            bottomBar.transform.position = new Vector3(0, letterboxHeight, 0);
            
            yield return new WaitForSeconds(duration);
            
            Destroy(topBar);
            Destroy(bottomBar);
        }

        public void TriggerScreenShake(float intensity = 0.5f, float duration = 0.5f, bool useLetterbox = false)
        {
            var data = new EventManager.ScreenEventData
            {
                ShakeIntensity = intensity,
                ShakeDuration = duration,
                UseLetterbox = useLetterbox,
                LetterboxDuration = 1f
            };
            
            EventManager.Publish(EventManager.EventNames.HIGHLIGHT_START, data);
        }
    }
} 