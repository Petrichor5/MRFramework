using UnityEngine;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ButtonAudioExtend
    {
        [SerializeField] private bool m_IsUseClickAudio;

        public void OnPointerDown(Transform trans)
        {
        }

        public void OnPointerUp(ButtonProBase buttonProBase)
        {
            //buttonProBase.OnPointerClick();
        }

        public bool OnButtonClick()
        {
            if (m_IsUseClickAudio)
            {
                // 通过音频管理类播放对应的按钮点击音效
                AudioManager.Instance.PlayButtonClickSound();
            }

            return true;
        }
    }
}