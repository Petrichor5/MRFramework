using MRFramework;

public class MRF : Architecture<MRF>
{
    #region Module

    public static EventManager EventMgr => EventManager.Instance;
    public static AssetManager AssetMgr => AssetManager.Instance;
    public static UIManager UIMgr => UIManager.Instance;
    public static SceneManager SceneMgr => SceneManager.Instance;
    public static AudioManager AudioMgr => AudioManager.Instance;
    public static ConfigManager ConfigMgr => ConfigManager.Instance;
    public static InputManager InputMgr => InputManager.Instance;
    public static TimerManager TimerMgr => TimerManager.Instance;
    public static GlobalManager GlobalMgr => GlobalManager.Instance;
    public static DataManager DataMgr => DataManager.Instance;
    public static UIDManager UIDMgr => UIDManager.Instance;

    #endregion

    protected override void Init()
    {
        // 由于数据初始化问题，需要分层初始化
        // 因为有些数据是异步拿取的，后面可能要通过事件来等待，或者流程状态机
        FirstInit();
        CommonInit();
        LastInit();
    }

    /// <summary>
    /// 最先初始化
    /// </summary>
    private void FirstInit()
    {

    }

    /// <summary>
    /// 普通初始化
    /// </summary>
    private void CommonInit()
    {

    }

    /// <summary>
    /// 最后初始化
    /// </summary>
    private void LastInit()
    {
        RegisterSystem(new UnlockSystem());
        RegisterSystem(new ReddotSystem());
    }
}