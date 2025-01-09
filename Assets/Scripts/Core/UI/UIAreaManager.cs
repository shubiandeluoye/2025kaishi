using UnityEngine;
using Core.UI.Base;

namespace Core.UI
{
    public class UIAreaManager : BaseUIElement
    {
        [Header("UI Areas")]
        [SerializeField] private RectTransform topUIArea;
        [SerializeField] private RectTransform bottomUIArea;

        private void Start()
        {
            SetupUIAreas();
        }

        private void SetupUIAreas()
        {
            // Set up top UI area (7x1.5)
            if (topUIArea != null)
            {
                topUIArea.sizeDelta = new Vector2(7f, 1.5f);
                topUIArea.anchoredPosition = new Vector2(0f, 4.25f);
            }

            // Set up bottom UI area (7x1.5)
            if (bottomUIArea != null)
            {
                bottomUIArea.sizeDelta = new Vector2(7f, 1.5f);
                bottomUIArea.anchoredPosition = new Vector2(0f, -4.25f);
            }
        }

        public RectTransform GetTopUIArea() => topUIArea;
        public RectTransform GetBottomUIArea() => bottomUIArea;
    }
}
