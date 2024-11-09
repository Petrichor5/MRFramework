using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 唯一ID管理器
/// </summary>
public class UIDManager : MonoSingleton<UIDManager>
{
    private UIDManager() { }

    private const string m_SaveKey = "UniqueIDCounter";
    private int m_UID;
    private bool m_IsDirty = false;

    public override void OnSingletonInit()
    {
        // 默认从 0 开始
        m_UID = PlayerPrefs.GetInt(m_SaveKey, 0);
    }

    /// <summary>
    /// 生成一个唯一ID
    /// </summary>
    /// <param name="isWaitSave">用于批量操作后，手动调用保存操作</param>
    public long GetUID(bool isWaitSave = false)
    {
        m_UID++;
        if (!isWaitSave)
        {
            PlayerPrefs.SetInt(m_SaveKey, m_UID);
            PlayerPrefs.Save();
        }
        else
        {
            m_IsDirty = true;
        }
        return m_UID;
    }

    /// <summary>
    /// 保存最新的唯一到本地，批量操作后调用一次，可以避免在频繁地调用生成UID时造成性能影响
    /// </summary>
    public void SaveUID()
    {
        if (m_IsDirty)
        {
            // 保存更新后的计数器
            PlayerPrefs.SetInt(m_SaveKey, m_UID);
            PlayerPrefs.Save();
            m_IsDirty = false;
        }
    }
}
