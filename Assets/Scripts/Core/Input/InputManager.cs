using UnityEngine;
using Core.Base.Manager;
using Core.Base.Event;

namespace Core.Input
{
    public class InputManager : BaseManager
    {
        [Header("Touch Settings")]
        [SerializeField] private float touchSensitivity = 1f;
        [SerializeField] private float minSwipeDistance = 50f;
        
        private Vector2 touchStartPos;
        private bool isTouching;

        private void Update()
        {
            if (!IsInitialized()) return;
            
            HandleTouchInput();
            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void HandleTouchInput()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos = touch.position;
                        isTouching = true;
                        EventManager.Publish(EventNames.TOUCH_INPUT, 
                            new TouchInputEvent(TouchEventType.Begin, touch.position));
                        break;

                    case TouchPhase.Moved:
                        if (isTouching)
                        {
                            Vector2 delta = touch.position - touchStartPos;
                            if (delta.magnitude > minSwipeDistance)
                            {
                                EventManager.Publish(EventNames.TOUCH_INPUT, 
                                    new TouchInputEvent(TouchEventType.Move, touch.position, delta));
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        isTouching = false;
                        EventManager.Publish(EventNames.TOUCH_INPUT, 
                            new TouchInputEvent(TouchEventType.End, touch.position));
                        break;
                }
            }
        }

        private void HandleKeyboardInput()
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (UnityEngine.Input.GetKeyDown(key))
                {
                    EventManager.Publish(EventNames.KEYBOARD_INPUT, 
                        new KeyboardInputEvent(key, true));
                }
                else if (UnityEngine.Input.GetKeyUp(key))
                {
                    EventManager.Publish(EventNames.KEYBOARD_INPUT, 
                        new KeyboardInputEvent(key, false));
                }
            }
        }

        private void HandleMouseInput()
        {
            Vector2 mousePosition = UnityEngine.Input.mousePosition;

            for (int i = 0; i < 3; i++)
            {
                if (UnityEngine.Input.GetMouseButtonDown(i))
                {
                    EventManager.Publish(EventNames.MOUSE_INPUT, 
                        new MouseInputEvent(mousePosition, i, true));
                }
                else if (UnityEngine.Input.GetMouseButtonUp(i))
                {
                    EventManager.Publish(EventNames.MOUSE_INPUT, 
                        new MouseInputEvent(mousePosition, i, false));
                }
            }
        }
    }
}
