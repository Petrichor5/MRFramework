using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ScrollViewProBase : ScrollRect
    {
        [Header("Item预制体")]
        public AssetReferenceGameObject ItemPrefab;

        protected float m_ItemWidth;
        protected float m_ItemHeight;
        protected float m_ItemOffsetX;
        protected float m_ItemOffsetY;
        protected float m_ColumnsPerRow;

        protected SimpleObjectPool<GameObject> m_ItemPool; // Item 对象池
        protected List<RectTransform> m_ItemList; // 保存可见项的 RectTransform

        public void Initialize()
        {
            m_ItemPool = new SimpleObjectPool<GameObject>(CreateItem, ResetItem);
            m_ItemList = new List<RectTransform>();

            GridLayoutGroup grid = content.gameObject.GetComponent<GridLayoutGroup>();

            // 获取Item的宽度和高度
            m_ItemWidth = grid.cellSize.x;
            m_ItemHeight = grid.cellSize.y;
            // 偏移
            m_ItemOffsetX = grid.spacing.x;
            m_ItemOffsetY = grid.spacing.y;

            // 获取Content的宽度
            float contentWidth = content.rect.width;

            // 动态计算每行能放下的列数（columnsPerRow）
            m_ColumnsPerRow = Mathf.FloorToInt(contentWidth / (m_ItemWidth + m_ItemOffsetX));
        }

        public void Clear()
        {
            // 回收所有对象并清空池
            if (m_ItemList != null)
            {
                foreach (RectTransform item in m_ItemList)
                {
                    item.gameObject.GetComponent<ScrollItem>().OnDispose();
                    m_ItemPool.Recycle(item.gameObject); // 回收所有对象到对象池中
                }

                m_ItemList.Clear(); // 清空列表
                m_ItemList = null;
            }

            // 清空对象池
            if (m_ItemPool != null)
            {
                m_ItemPool.ClearPool(OnClearItem);
                m_ItemPool = null;
            }
        }

        private GameObject CreateItem()
        {
            var aoh = Addressables.InstantiateAsync(ItemPrefab, content);
            aoh.WaitForCompletion();
            GameObject gameObject = aoh.Result;
            ScrollItem item = gameObject.GetComponent<ScrollItem>();
            item.OnFirstOpen();
            return gameObject;
        }

        private void ResetItem(GameObject obj)
        {
            var item = obj.GetComponent<ScrollItem>();
            item.Close();
        }

        private void OnClearItem(GameObject obj)
        {
            var item = obj.GetComponent<ScrollItem>();
            item.OnDispose();
            Addressables.ReleaseInstance(obj);
        }
    }
}