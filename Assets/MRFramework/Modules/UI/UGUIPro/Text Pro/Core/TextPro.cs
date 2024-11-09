using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace MRFramework.UGUIPro
{
    //不允许添加多个TextPro组件添加到同一个节点上
    [DisallowMultipleComponent]
    public class TextPro : TextProBase
    {
        private CanvasGroup m_CanvasGroup;
        private bool m_IsVisible;
        private Regex m_Regex;

        protected override void Awake()
        {
            base.Awake();
            m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (m_CanvasGroup != null )
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

        public void SetText(string sContent)
        {
            text = sContent;
        }
    
        public void SetTemplateText(params string[] strParams)
        {
            CheckInitTemplate();
            
            var sContent = text;
            var matches = m_Regex.Matches(sContent);

            // 确保参数数量与占位符数量匹配
            if (matches.Count != strParams.Length) return;
            
            // 使用StringBuilder，防止String多次重复替换导致性能消耗
            var sb = new StringBuilder();
            int lastIndex = 0;
            // 遍历匹配项，构建结果字符串
            foreach (Match match in matches)
            {
                // 将之前的文本部分添加到 StringBuilder
                sb.Append(text.Substring(lastIndex, match.Index - lastIndex));
                // 添加对应的参数值
                sb.Append(strParams[match.Index]);
                // 更新最后索引
                lastIndex = match.Index + match.Length;
            }
            // 添加最后的文本部分
            sb.Append(text.Substring(lastIndex));
            
            text = sb.ToString();
        }

        /// <summary>
        /// 打字机效果
        /// </summary>
        /// <param name="endValue">显示的文本</param>
        /// <param name="duration">持续时间</param>
        /// <param name="onComplete">结束回调</param>
        public void TweenText(string endValue, float duration, UnityAction onComplete = null)
        {
            this.DOText(endValue, duration)
                .SetAutoKill()
                .onComplete += () =>
                {
                    onComplete?.Invoke();
                };
        }

        private void CheckInitTemplate()
        {
            if (m_Regex == null)
            {
                m_Regex = new Regex(@"\{(\w+)\}", RegexOptions.Compiled);
            }
        }
    }
}