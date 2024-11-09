using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    public class Group : MonoBehaviour
    {
        private CanvasGroup m_CanvasGroup;
        private bool m_IsVisible;

        private void Awake()
        {
            m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (m_CanvasGroup != null)
            {
                m_IsVisible = m_CanvasGroup.alpha == 1;
                m_CanvasGroup.interactable = m_IsVisible;
                m_CanvasGroup.blocksRaycasts = m_IsVisible;
            }
        }

        /// <summary>
        /// 设置显示隐藏
        /// </summary>
        public void SetVisible(bool visible)
        {
            if (m_IsVisible == visible) return;
            m_CanvasGroup.alpha = visible ? 1 : 0;
            m_CanvasGroup.interactable = visible;
            m_CanvasGroup.blocksRaycasts = visible;
        }
    }
}
