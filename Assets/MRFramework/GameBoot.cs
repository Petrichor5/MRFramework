using UnityEngine;
using UnityEngine.SceneManagement;

namespace MRFramework
{
    public class GameBoot : MonoBehaviour
    {
        [Tooltip("开启事件系统日志")]
        public bool OpenEventLog = false;
        
        private void Awake()
        {
            EventManager.Instance.IsOpenEventLog = OpenEventLog;
        }

        private void Start()
        {
            // 初始化日志系统
            Log.Initalize();

            // 初始化模块
            var eventManager = EventManager.Instance;
            eventManager.IsOpenEventLog = OpenEventLog;

            var inputManager = InputManager.Instance;
            inputManager.SetInputListener(false);

            var assetManager = AssetManager.Instance;
            assetManager.SetAssetLoaderPoolCount(60);

            var uiManager = UIManager.Instance;

            var audioManager =  AudioManager.Instance;
            audioManager.SetAudioPlayerPoolCount(30);
            audioManager.SetATimeClockPoolCount(30);
            
            var timerManager = TimerManager.Instance;
            timerManager.SetPoolCapacity(30);
            
            var gameModule = GameModule.Instance;

            // 初始化框架
            var gameArch = GameArch.Instance;

            // 切换到主页面场景
            SceneManager.LoadScene("MainScene");
        }
    }
}