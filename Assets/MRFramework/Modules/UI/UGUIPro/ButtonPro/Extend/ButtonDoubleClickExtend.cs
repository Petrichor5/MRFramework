using UnityEngine;
using UnityEngine.Events;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ButtonDoubleClickExtend
    {
        [SerializeField] private bool m_IsUseDoubleClick;
        [Header("有效时间")] [SerializeField] private float m_ClickInterval;
        [SerializeField] private float m_LastPointerDownTime;
        [SerializeField] private ButtonClickEvent m_ButtonClickedEvent;

        public bool IsUseDoubleClick => m_IsUseDoubleClick;
        
        public void OnPointerDown()
        {
            m_LastPointerDownTime = Time.realtimeSinceStartup - m_LastPointerDownTime < m_ClickInterval
                ? 0
                : Time.realtimeSinceStartup;
            if (m_LastPointerDownTime == 0)
                m_ButtonClickedEvent?.Invoke();
        }

        public void AddListener(UnityAction callback, float clickInterval)
        {
            m_ClickInterval = clickInterval;
            m_IsUseDoubleClick = true;
            m_ButtonClickedEvent.AddListener(callback);
        }
    }
}