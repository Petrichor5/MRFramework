using UnityEngine;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ButtonPressScaleExtend
    {
        [SerializeField] private bool m_IsUsePressScale = true;
        [Header("默认缩放")] [SerializeField] private Vector3 m_NormalScale = Vector3.one;
        [Header("按下缩放")] [SerializeField] private Vector3 m_PressScale = new Vector3(0.9f, 0.9f, 0.9f);

        public bool UsePressScale => m_IsUsePressScale;
        
        public void OnPointerDown(Transform trans, bool interactable)
        {
            if (m_IsUsePressScale && interactable)
            {
                trans.localScale = m_PressScale;
            }
        }

        public void OnPointerUp(Transform trans, bool interactable)
        {
            if (m_IsUsePressScale && interactable)
            {
                trans.localScale = m_NormalScale;
            }
        }
    }
}