using MRFramework;
using UnityEngine.Events;

[MonoSingletonPath("GameManager")]
public class GameModule : MonoSingleton<GameModule>
{
    public static UIManager UIMgr => UIManager.Instance;

    public static AssetManager ResMgr => AssetManager.Instance;

    public static AudioManager AudioMgr => AudioManager.Instance;

    public static TimerManager TimerMgr => TimerManager.Instance;

    public static EventManager EventMgr => EventManager.Instance;

    public static InputManager InputMgr => InputManager.Instance;

    private void Update()
    {
        mUpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        mFixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        mLateUpdateEvent?.Invoke();
    }

    private event UnityAction mUpdateEvent;
    private event UnityAction mFixedUpdateEvent;
    private event UnityAction mLateUpdateEvent;

    public void AddUpdateListener(UnityAction updateFun)
    {
        mUpdateEvent += updateFun;
    }
    
    public void RemoveUpdateListener(UnityAction updateFun)
    {
        mUpdateEvent -= updateFun;
    }
    
    public void AddFixedUpdateListener(UnityAction updateFun)
    {
        mFixedUpdateEvent += updateFun;
    }

    public void RemoveFixedUpdateListener(UnityAction updateFun)
    {
        mFixedUpdateEvent -= updateFun;
    }
    
    public void AddLateUpdateListener(UnityAction updateFun)
    {
        mLateUpdateEvent += updateFun;
    }
    
    public void RemoveLateUpdateListener(UnityAction updateFun)
    {
        mLateUpdateEvent -= updateFun;
    }
}