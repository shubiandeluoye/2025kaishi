using UnityEngine;

namespace Core.UI.Events
{
    public enum UIEventType
    {
        // 菜单事件
        MenuShow,
        MenuHide,
        MenuBack,
        MenuForward,

        // 游戏状态事件
        GamePause,
        GameResume,
        GameStart,
        GameOver,

        // 场景事件
        SceneLoad,
        SceneUnload,
        SceneTransitionStart,
        SceneTransitionComplete,

        // UI动画事件
        AnimationStart,
        AnimationComplete,
        TransitionInStart,
        TransitionInComplete,
        TransitionOutStart,
        TransitionOutComplete,

        // UI交互事件
        Click,
        Drag,
        Scroll,
        Hover,
        Focus,

        // HUD事件
        CombatHit,
        CombatDamage,
        CombatKill,
        InteractionPrompt,
        HealthUpdate,
        AmmoUpdate,
        ScoreUpdate,
        ObjectiveUpdate
    }

    public class UIEvent
    {
        public UIEventType Type { get; private set; }
        public object Data { get; private set; }

        public UIEvent(UIEventType type, object data = null)
        {
            Type = type;
            Data = data;
        }
    }

    public class UIAnimationEvent
    {
        public string ElementName { get; private set; }
        public bool IsComplete { get; private set; }
        public float Duration { get; private set; }

        public UIAnimationEvent(string elementName, bool isComplete, float duration = 0)
        {
            ElementName = elementName;
            IsComplete = isComplete;
            Duration = duration;
        }
    }

    public class UITransitionEvent
    {
        public string FromScene { get; private set; }
        public string ToScene { get; private set; }
        public float Progress { get; private set; }

        public UITransitionEvent(string fromScene, string toScene, float progress)
        {
            FromScene = fromScene;
            ToScene = toScene;
            Progress = progress;
        }
    }

    public class UIInteractionEvent
    {
        public string ElementName { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Delta { get; private set; }
        public bool IsActive { get; private set; }

        public UIInteractionEvent(string elementName, Vector2 position, Vector2 delta = default, bool isActive = true)
        {
            ElementName = elementName;
            Position = position;
            Delta = delta;
            IsActive = isActive;
        }
    }
} 