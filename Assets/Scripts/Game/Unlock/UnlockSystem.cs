using Config;
using MRFramework;
using System.Collections.Generic;

public class UnlockSystem : ASystem
{
    private Dictionary<EUnlockType, AUnlockHandle> m_UnlockHandleDic; // 解锁类型的Hnadle字典
    private Dictionary<EUnlockID, bool> m_UnlockIDDic; // 记录已解锁的解锁ID
    private Dictionary<EUnlockType, List<EUnlockID>> m_Type2LockIDDic; // 记录该解锁类型有关的还没有解锁的ID

    private EventComponent m_EventComponent;

    protected override void OnInit()
    {
        m_UnlockHandleDic = new Dictionary<EUnlockType, AUnlockHandle>();
        m_UnlockIDDic = new Dictionary<EUnlockID, bool>();
        m_Type2LockIDDic = new Dictionary<EUnlockType, List<EUnlockID>>();
        m_EventComponent = new EventComponent();

        InitUnlockHandle();
        InitUnlockInfo();
        AddUnlockEventListeners();
    }

    protected override void OnDispose()
    {
        m_EventComponent.Clear();
        m_EventComponent = null;

        m_UnlockHandleDic.Clear();
        m_UnlockHandleDic = null;
    }

    private void InitUnlockHandle()
    {
        m_UnlockHandleDic.Add(EUnlockType.EPlayerLevelType, new PlayerLevelUnlockHandle());
    }

    private void AddUnlockEventListeners()
    {
        m_EventComponent.AddEventListener(PlayerEvent.OnPlayerLevelUp, OnPlayerLevelUp);
    }

    #region 对外接口

    /// <summary>
    /// 是否解锁
    /// </summary>
    /// <param name="unlockID"></param>
    /// <param name="isShowTips">如果未解锁，是否弹Tips</param>
    /// <returns></returns>
    public bool IsUnlock(EUnlockID unlockID, bool isShowTips = false)
    {
        if (unlockID <= 0) return false;
        bool isUnlock = m_UnlockIDDic.ContainsKey(unlockID);
        if (isShowTips && !isUnlock)
        {
            var unlockInfo = MRF.ConfigMgr.GetConfigByRow<Config_Unlock_Unlock>((int)unlockID);
            Log.Warning(unlockInfo.LockDesc); // 临时用，后续用UI提示
        }
        return isUnlock;
    }

    #endregion

    /// <summary>
    /// 初始化解锁信息
    /// </summary>
    private void InitUnlockInfo()
    {
        var unlockConfig = MRF.ConfigMgr.GetConfig<Config_Unlock_Unlock>();
        foreach (var data in unlockConfig.Values)
        {
            EUnlockID unlockID = data.EUnlockID;
            EUnlockType type = data.EUnlockType;
            if (m_UnlockHandleDic.ContainsKey(type))
            {
                AUnlockHandle unlockHandle = m_UnlockHandleDic[type];
                if (unlockHandle.Execute(data))
                {
                    m_UnlockIDDic.Add(unlockID, true);
                }
                else
                {
                    if (m_Type2LockIDDic.ContainsKey(type))
                        m_Type2LockIDDic[type].Add(unlockID);
                    else
                        m_Type2LockIDDic.Add(type, new List<EUnlockID>() { unlockID });
                }
            }
        }
    }

    /// <summary>
    /// 检测解锁条件
    /// </summary>
    private void CheckUnlock(EUnlockType type)
    {
        AUnlockHandle unlockHandle = m_UnlockHandleDic[type];
        var configMgr = MRF.ConfigMgr;
        var eventMgr = MRF.EventMgr;

        // 查找是否有解锁的ID
        for (int index = 0; index < m_Type2LockIDDic[type].Count; index++)
        {
            var unlockID = m_Type2LockIDDic[type][index];
            var unlockInfo = configMgr.GetConfigByRow<Config_Unlock_Unlock>((int)unlockID);
            if (unlockHandle.Execute(unlockInfo))
            {
                // 记录新解锁的ID到解锁集合
                m_UnlockIDDic.Add(unlockID, true);

                // 从未解锁集合中移除新解锁的ID
                m_Type2LockIDDic[type].Remove(unlockID);
                if (m_Type2LockIDDic[type].Count <= 0)
                {
                    m_Type2LockIDDic.Remove(type);
                }

                // 发送解锁事件
                eventMgr.TriggerEventListener<EUnlockID>(UnlockEvent.UnlockOne, unlockID);
                break;
            }
        }
    }

    private void OnPlayerLevelUp()
    {
        CheckUnlock(EUnlockType.EPlayerLevelType);
    }

    private void OnLevelUnlock()
    {
        CheckUnlock(EUnlockType.ELevelUnlockType);
    }
}
