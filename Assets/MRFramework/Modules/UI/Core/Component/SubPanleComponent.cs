using MRFramework;
using System.Collections.Generic;

public class SubPanleComponent
{
    private List<SubPanel> m_SubPanleList; // 子面板列表

    public void AddSubPanle(SubPanel subPanel)
    {
        if (m_SubPanleList == null)
            m_SubPanleList = new List<SubPanel>();

        if (m_SubPanleList.Contains(subPanel)) return;

        subPanel.OnInit();
        m_SubPanleList.Add(subPanel);
    }

    public void RemoveSubPanel(SubPanel subPanel)
    {
        if (m_SubPanleList == null) return;

        if (m_SubPanleList.Contains(subPanel))
        {
            subPanel.OnClear();
            m_SubPanleList.Remove(subPanel);
        }
    }

    public void ClearAllSubPanle()
    {
        if (m_SubPanleList == null) return;

        for (int i = 0; i < m_SubPanleList.Count; i++)
        {
            m_SubPanleList[i].OnClear();
        }
        m_SubPanleList.Clear();
    }

    public void CloseAllSubPanel()
    {
        if (m_SubPanleList == null) return;

        foreach (var subPanel in m_SubPanleList)
        {
            subPanel.Close();
        }
    }

    public void OpenAllSubPanel()
    {
        if (m_SubPanleList == null) return;

        foreach (var subPanel in m_SubPanleList)
        {
            subPanel.Open();
        }
    }

    public void Clear()
    {
        ClearAllSubPanle();
    }
}