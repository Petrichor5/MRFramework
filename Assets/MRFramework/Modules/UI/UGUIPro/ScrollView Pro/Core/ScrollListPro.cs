using System;
using UnityEngine;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    /// <summary>
    /// 滚动列表
    /// </summary>
    [Serializable]
    public class ScrollListPro : ScrollBase
    {
        [Header("Item宽度")]
        public float ItemWidth;

        [Header("Item高度")]
        public float ItemHeight;

        #region 内部实现

        protected override void InitHorizontal()
        {
            HorizontalLayoutGroup group = content.gameObject.GetComponent<HorizontalLayoutGroup>();
            m_ItemWidth = ItemWidth * ItemScaleX;
            m_ItemOffsetX = group.spacing + group.padding.left;
        }

        protected override void InitVertical()
        {
            VerticalLayoutGroup group = content.gameObject.GetComponent<VerticalLayoutGroup>();
            m_ItemHeight = ItemHeight * ItemScaleY;
            m_ItemOffsetY = group.spacing + group.padding.top;
        }

        protected override void UpdateContentBounds()
        {
            if (m_ItemList == null || m_ItemList.Count == 0) return;

            int totalCount = m_ItemList.Count;

            switch (ScrollType)
            {
                case EScrollType.Horizontal:
                    // 更新Content的宽度，高度保持不变
                    float totalWidth = totalCount * (m_ItemWidth + m_ItemOffsetX); // 计算总宽度
                    content.sizeDelta = new Vector2(totalWidth, content.sizeDelta.y);
                    break;
                case EScrollType.Vertical:
                    // 更新Content的高度，宽度保持不变
                    float totalHeight = totalCount * (m_ItemHeight + m_ItemOffsetY); // 计算总高度
                    content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
                    break;
            }
        }

        #endregion
    }
}
