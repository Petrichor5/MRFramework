using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace MRFramework
{
    /// <summary>
    /// 游戏启动器
    /// </summary>
    public class GameLauncher : MonoBehaviour
    {
        // 用于游戏开发阶段测试查Bug
        [Tooltip("开启事件Debug日志")]
        public bool OpenEventDebug = false;
        [Tooltip("开启UI窗口Debug日志")]
        public bool OpenUIDebug = false;

        private void Awake()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            InitAwake();
        }

        private void Start()
        {
            InitGameModule();
            InitDone();
        }

        public void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F6))
            {
                GameObject go = GameObject.Find("TestCode");
                if (go == null)
                {
                    go = new GameObject("TestCode");
                    go.AddComponent<TestCode>();
                }

                TestCode testCode = go.GetComponent<TestCode>();
                Debug.Log("RunTestCode!");
                testCode.RunTestCode();
            }
#endif
        }

        private void OnApplicationQuit()
        {
            OnGameQuit();
        }

        /// <summary>
        /// 最先初始化的
        /// </summary>
        private void InitAwake()
        {
            // 事件系统
            _ = EventManager.Instance;
            EventManager.Instance.IsOpenEventLog = OpenEventDebug;

            // 初始化日志系统
            Log.Initalize();
        }

        /// <summary>
        /// 初始化游戏各个模块
        /// </summary>
        private void InitGameModule()
        {
            // 输入控制
            var inputManager = InputManager.Instance;
            inputManager.SetInputListener(false);

            // 资源加载
            _ = AssetManager.Instance;

            // UI面板
            _ = UIManager.Instance;

            // 音乐音效
            _ = AudioManager.Instance;

            // 计时器
            _ = TimerManager.Instance;

            // 游戏全局管理器
            _ = GlobalManager.Instance;

            // 初始化框架
            _ = MRF.Instance;
        }

        /// <summary>
        /// 初始化完成
        /// </summary>
        private void InitDone()
        {
            // 切换到主页面场景
            SceneManager.Instance.LoadSceneAsync("Common/Scene_Common_GameStart.unity", null, () =>
            {
                UIManager.Instance.OpenPanel<WBP_Main_Main>();
            }, false);
        }

        /// <summary>
        /// 游戏退出流程
        /// </summary>
        private void OnGameQuit()
        {
            //// 1.取消全局相关逻辑
            //GlobalManager.Instance.OnDispose();

            //// 2.游戏操作相关逻辑
            //EventManager.Instance.OnDispose();
            //InputManager.Instance.OnDispose();

            //// 3.游戏表现相关逻辑
            //AudioManager.Instance.OnDispose();
            //TimerManager.Instance.OnDispose();

            //// 3.卸载资源 释放引用
            //AssetManager.Instance.OnDispose();
            //UIManager.Instance.OnDispose();
            //SceneManager.Instance.OnDispose();

            // 4.底层框架数据（Model层、数据保存 等）
            MRF.Instance.OnDispose();
        }
    }
}