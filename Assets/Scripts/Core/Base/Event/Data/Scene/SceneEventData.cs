using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Base.Event.Data.Scene
{
    /// <summary>
    /// 场景加载事件数据
    /// </summary>
    public class SceneLoadEventData : EventDataBase
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; private set; }

        /// <summary>
        /// 加载模式
        /// </summary>
        public LoadSceneMode LoadMode { get; private set; }

        /// <summary>
        /// 是否显示加载界面
        /// </summary>
        public bool ShowLoadingUI { get; private set; }

        /// <summary>
        /// 加载参数
        /// </summary>
        public object Parameters { get; private set; }

        public SceneLoadEventData(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, 
            bool showLoadingUI = true, object parameters = null)
        {
            SceneName = sceneName;
            LoadMode = loadMode;
            ShowLoadingUI = showLoadingUI;
            Parameters = parameters;
        }
    }

    /// <summary>
    /// 场景加载进度事件数据
    /// </summary>
    public class SceneLoadProgressEventData : EventDataBase
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; private set; }

        /// <summary>
        /// 加载进度(0-1)
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 加载阶段
        /// </summary>
        public LoadingPhase Phase { get; private set; }

        public SceneLoadProgressEventData(string sceneName, float progress, LoadingPhase phase)
        {
            SceneName = sceneName;
            Progress = progress;
            Phase = phase;
        }
    }

    /// <summary>
    /// 场景切换事件数据
    /// </summary>
    public class SceneTransitionEventData : EventDataBase
    {
        /// <summary>
        /// 当前场景
        /// </summary>
        public string CurrentScene { get; private set; }

        /// <summary>
        /// 目标场景
        /// </summary>
        public string TargetScene { get; private set; }

        /// <summary>
        /// 切换类型
        /// </summary>
        public TransitionType Type { get; private set; }

        /// <summary>
        /// 切换持续时间
        /// </summary>
        public float Duration { get; private set; }

        public SceneTransitionEventData(string currentScene, string targetScene, 
            TransitionType type, float duration)
        {
            CurrentScene = currentScene;
            TargetScene = targetScene;
            Type = type;
            Duration = duration;
        }
    }

    /// <summary>
    /// 场景初始化事件数据
    /// </summary>
    public class SceneInitializeEventData : EventDataBase
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; private set; }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public SceneInitConfig Config { get; private set; }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        public bool Success { get; private set; }

        public SceneInitializeEventData(string sceneName, SceneInitConfig config, bool success)
        {
            SceneName = sceneName;
            Config = config;
            Success = success;
        }
    }

    /// <summary>
    /// 加载阶段枚举
    /// </summary>
    public enum LoadingPhase
    {
        /// <summary>
        /// 准备中
        /// </summary>
        Preparing,

        /// <summary>
        /// 加载资源
        /// </summary>
        LoadingAssets,

        /// <summary>
        /// 初始化场景
        /// </summary>
        Initializing,

        /// <summary>
        /// 完成
        /// </summary>
        Complete
    }

    /// <summary>
    /// 切换类型枚举
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// 淡入淡出
        /// </summary>
        Fade,

        /// <summary>
        /// 交叉淡入淡出
        /// </summary>
        CrossFade,

        /// <summary>
        /// 即时切换
        /// </summary>
        Instant
    }

    /// <summary>
    /// 场景初始化配置
    /// </summary>
    public class SceneInitConfig
    {
        /// <summary>
        /// 玩家出生点
        /// </summary>
        public Vector3 PlayerSpawnPoint { get; set; }

        /// <summary>
        /// 是否重置玩家状态
        /// </summary>
        public bool ResetPlayerState { get; set; }

        /// <summary>
        /// 背景音乐
        /// </summary>
        public string BackgroundMusic { get; set; }

        /// <summary>
        /// 环境参数
        /// </summary>
        public object EnvironmentParams { get; set; }
    }
} 