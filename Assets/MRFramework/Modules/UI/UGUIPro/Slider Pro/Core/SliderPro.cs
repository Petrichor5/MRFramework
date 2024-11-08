using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace MRFramework.UGUIPro
{
    [Serializable]
    public class SliderPro : SliderBase
    {
        private CanvasGroup m_CanvasGroup;
        private bool m_IsVisible;

        protected override void Awake()
        {
            base.Awake();
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

        public void SetMaxValue(float v)
        {
            maxValue = v;
        }
    
        public void SetValue(float v)
        {
            this.value = v;
        }
    
        public void SetMinValue(float v)
        {
            minValue = v;
        }

        /// <summary>
        /// 滑动条Tween动画
        /// </summary>
        /// <param name="endValue">最终的值</param>
        /// <param name="duration">持续时间</param>
        /// <param name="onComplete">结束回调</param>
        public void TweenValue(float endValue, float duration, UnityAction onComplete = null)
        {
            this.DOValue(endValue, duration)
                .SetAutoKill()
                .onComplete += () =>
                {
                    onComplete?.Invoke();
                };
        }
        
        public void AddSliderListener(UnityAction<float> callback)
        {
            onValueChanged.AddListener(callback);
        }
    
        public void RemoveSliderListener(UnityAction<float> callback)
        {
            onValueChanged.RemoveListener(callback);
        }
    
        public void RemoveAllSliderListeners()
        {
            onValueChanged.RemoveAllListeners();
        }
    }
}