using UnityEngine;
using Core.UI.Base;
using Core.UI.Events;
using Core.Base.Event;
using Core.Base.Event.Data.UI; 

namespace Core.UI
{
    public class UIAreaManager : BaseUIElement
    {
        [Header("UI Areas")]
        [SerializeField] private RectTransform topUIArea;
        [SerializeField] private RectTransform bottomUIArea;

        protected override void Awake()
        {
            base.Awake();
            SetupUIAreas();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventManager.Subscribe<UIAreaUpdateEvent>(EventNames.UI_AREA_UPDATE, OnUIAreaUpdate);
        }

        protected override void OnDestroy()
        {
            EventManager.Unsubscribe<UIAreaUpdateEvent>(EventNames.UI_AREA_UPDATE, OnUIAreaUpdate);
            base.OnDestroy();
        }

        private void OnUIAreaUpdate(UIAreaUpdateEvent evt)
        {
            UpdateUIArea(evt.IsTopArea, evt.Size, evt.Position);
        }

        private void UpdateUIArea(bool isTopArea, Vector2 size, Vector2 position)
        {
            RectTransform area = isTopArea ? topUIArea : bottomUIArea;
            if (area != null)
            {
                area.sizeDelta = size;
                area.anchoredPosition = position;
            }
        }

        private void SetupUIAreas()
        {
            // 初始化顶部UI区域
            if (topUIArea != null)
            {
                topUIArea.sizeDelta = new Vector2(7f, 1.5f);
                topUIArea.anchoredPosition = new Vector2(0f, 4.25f);
            }

            // 初始化底部UI区域
            if (bottomUIArea != null)
            {
                bottomUIArea.sizeDelta = new Vector2(7f, 1.5f);
                bottomUIArea.anchoredPosition = new Vector2(0f, -4.25f);
            }
        }

        public RectTransform GetTopUIArea() => topUIArea;
        public RectTransform GetBottomUIArea() => bottomUIArea;
    }

    public class UIAreaUpdateData
    {
        public bool IsTopArea { get; private set; }
        public Vector2 Size { get; private set; }
        public Vector2 Position { get; private set; }

        public UIAreaUpdateData(bool isTopArea, Vector2 size, Vector2 position)
        {
            IsTopArea = isTopArea;
            Size = size;
            Position = position;
        }
    }
}
