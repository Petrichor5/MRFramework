using System;
using UnityEngine;

public class UIDUtil
{
    private const string m_IDKey = "UniqueIDCounter";
    private static long m_CurrentID = 0;
    private static bool m_IsInit = false;

    public static void Init()
    {
        // 加载保存的ID计数器
        if (PlayerPrefs.HasKey(m_IDKey))
        {
            m_CurrentID = PlayerPrefs.GetInt(m_IDKey);
        }
        m_IsInit = true;
    }

    public static long GenerateUniqueID()
    {
        if (!m_IsInit) Init();

        m_CurrentID++;
        PlayerPrefs.SetInt(m_IDKey, (int)m_CurrentID); // 保存更新后的计数器
        PlayerPrefs.Save(); // 确保数据被保存
        return m_CurrentID;
    }
}