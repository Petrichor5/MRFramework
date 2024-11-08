using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ButtonLongPressExtend
    {
        [SerializeField] private bool m_IsUseLongPress;
        [Header("长按时间")] [SerializeField] private float m_Duration;
        [SerializeField] private ButtonClickEvent m_ButtonLongPressEvent;
        private float m_PointerDownTime;

        public bool IsUseLongPress => m_IsUseLongPress;
        
        public void OnPointerDown()
        {
            m_PointerDownTime = Time.realtimeSinceStartup;
        }

        public void OnUpdateSelected()
        {
            if (m_Duration >= 0 && Time.realtimeSinceStartup - m_PointerDownTime >= m_Duration)
            {
                m_ButtonLongPressEvent?.Invoke();
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void AddListener(UnityAction callback, float duration)
        {
            m_Duration = duration;
            m_IsUseLongPress = true;
            m_ButtonLongPressEvent.AddListener(callback);
        }
    }
}