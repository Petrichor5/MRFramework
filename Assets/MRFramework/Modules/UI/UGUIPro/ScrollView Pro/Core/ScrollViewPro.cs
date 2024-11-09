using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    /// <summary>
    /// 滚动视窗
    /// </summary>
    [Serializable]
    public class ScrollViewPro : ScrollBase
    {
        private float m_ItemNumPerRow; // 每行能放下的Item个数
        private float m_ItemNumPerColumns; // 每列能放下的Item个数

        #region 内部实现

        private void Init()
        {
            GridLayoutGroup group = content.gameObject.GetComponent<GridLayoutGroup>();

            m_ItemWidth = group.cellSize.x;
            m_ItemHeight = group.cellSize.y;

            m_ItemOffsetX = group.spacing.x;
            m_ItemOffsetY = group.spacing.y;

            m_PaddingTop = group.padding.top;
            m_PaddingBottom = group.padding.bottom;
            m_PaddingLeft = group.padding.left;
            m_PaddingRight = group.padding.right;
        }

        protected override void InitHorizontal()
        {
            Init();
            float contentHeight = content.rect.height;
            m_ItemNumPerColumns = Mathf.FloorToInt((contentHeight - m_PaddingTop - m_PaddingBottom) / (m_ItemHeight + m_ItemOffsetY));
        }

        protected override void InitVertical()
        {
            Init();
            float contentWidth = content.rect.width;
            m_ItemNumPerRow = Mathf.FloorToInt((contentWidth - m_PaddingLeft - m_PaddingRight) / (m_ItemWidth + m_ItemOffsetX));
        }

        protected override void UpdateContentBounds()
        {
            if (m_ItemList == null || m_ItemList.Count == 0) return;

            switch (ScrollType)
            {
                case EScrollType.Horizontal:
                    // 更新Content的宽度，高度保持不变
                    int totalColumns = Mathf.CeilToInt(m_ItemList.Count / m_ItemNumPerColumns); // 计算总列数
                    float totalWidth = totalColumns * (m_ItemWidth + m_ItemOffsetX); // 计算总宽度
                    content.sizeDelta = new Vector2(totalWidth + m_PaddingLeft + m_PaddingRight, content.sizeDelta.y);
                    break;
                case EScrollType.Vertical:
                    // 更新Content的高度，宽度保持不变
                    int totalRows = Mathf.CeilToInt(m_ItemList.Count / m_ItemNumPerRow); // 计算总行数
                    float totalHeight = totalRows * (m_ItemHeight + m_ItemOffsetY); // 计算总高度
                    content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight + m_PaddingTop + m_PaddingBottom);
                    break;
            }
        }

        #endregion
    }
}