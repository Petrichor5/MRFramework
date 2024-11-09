using MRFramework;

public class MRF : Architecture<MRF>
{
    #region Framework

    public static T GetControl<T>() where T : class, IController
    {
        return Instance.GetController<T>();
    }

    public new static T GetSystem<T>() where T : class, ISystem
    {
        return Instance.GetSystem<T>();
    }

    public static T GetUtil<T>() where T : class, IUtility
    {
        return Instance.GetUtility<T>();
    }

    public new static void SendCommand<T>(T command) where T : class, ICommand
    {
        Instance.SendCommand<T>(command);
    }

    public new static TResult SendCommand<TResult>(ICommand<TResult> command)
    {
        return Instance.SendCommand<TResult>(command);
    }

    public new static TResult SendQuery<TResult>(IQuery<TResult> query)
    {
        return Instance.SendQuery<TResult>(query);
    }

    #endregion

    #region Module

    public static EventManager EventMgr => EventManager.Instance;
    public static AssetManager AssetMgr => AssetManager.Instance;
    public static UIManager UIMgr => UIManager.Instance;
    public static SceneManager SceneMgr => SceneManager.Instance;
    public static AudioManager AudioMgr => AudioManager.Instance;
    public static ConfigManager ConfigMgr => ConfigManager.Instance;
    public static InputManager InputMgr => InputManager.Instance;
    public static ReddotManager ReddotMgr => ReddotManager.Instance;
    public static TimerManager TimerMgr => TimerManager.Instance;
    public static GlobalManager GlobalMgr => GlobalManager.Instance;
    public static DataManager DataMgr => DataManager.Instance;
    public static UIDManager UIDMgr => UIDManager.Instance;

    #endregion

    protected override void Init()
    {
        
    }
}