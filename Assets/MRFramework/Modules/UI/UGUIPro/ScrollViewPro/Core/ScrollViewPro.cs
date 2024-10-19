using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    [Serializable]
    public class ScrollViewPro : ScrollViewProBase
    {
        private Action m_LoadCompletedHandle; // 加载完毕回调
        private Action<GameObject> m_LoadItemHandle; // 加载每个Item的回调

        private int m_ItemsPerFrame = 5; // 每帧创建的Item数量
        private bool m_IsInitList = false;

        /// <summary>
        /// 初始化 ScrollView
        /// </summary>
        public void InitScrollView()
        {
            if (m_IsInitList)
            {
                Log.Error("[ScrollViewPro] 滚动视图重复初始化！");
                return;
            }
            Initialize();
            m_IsInitList = true;
        }

        /// <summary>
        /// 设置加载结束回调
        /// </summary>
        public void SetLoadCompletedCallback(Action loadCompleted)
        {
            m_LoadCompletedHandle = loadCompleted;
        }

        /// <summary>
        /// 设置Item加载结束回调
        /// </summary>
        public void SetLoadItemCallback(Action<GameObject> loadItem)
        {
            m_LoadItemHandle = loadItem;
        }

        /// <summary>
        /// 设置列表项数据
        /// </summary>
        public void SetItems<T>(List<T> datas)
        {
            if (m_IsInitList)
            {
                StartCoroutine(LoadItemsOverFrames(datas));
            }
            else
            {
                Log.Error("[ScrollViewPro] 滚动视图没有初始化");
            }
        }

        /// <summary>
        /// 清空列表项数据
        /// </summary>
        public void ClearItems()
        {
            if (m_IsInitList)
            {
                foreach (var item in m_ItemList)
                {
                    item.gameObject.GetComponent<ScrollViewItem>().OnDispose();
                    m_ItemPool.Recycle(item.gameObject); // 回收所有对象到对象池中
                }
                m_ItemList.Clear();
            }
            else
            {
                Log.Error("[ScrollViewPro] 滚动视图没有初始化");
            }
        }

        /// <summary>
        /// 计算Content包边范围
        /// </summary>
        public void UpdateContentBounds()
        {
            if (m_ItemList == null || m_ItemList.Count == 0) return;

            // 计算总行数
            Debug.Log($"ItemListCount = {m_ItemList.Count}, ColumnsPerRow = {m_ColumnsPerRow}");
            int totalRows = Mathf.CeilToInt(m_ItemList.Count / m_ColumnsPerRow);

            // 计算Content的总高度
            Debug.Log($"totalRows = {totalRows}, ItemHeight = {(m_ItemHeight + m_ItemOffsetY)}");
            float totalHeight = totalRows * (m_ItemHeight + m_ItemOffsetY);

            // 更新Content的高度，宽度保持不变
            Debug.Log($"Width = {content.sizeDelta.x}, Height = {totalHeight}");
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
        }

        /// <summary>
        /// 根据索引获取指定类型的Item组件
        /// </summary>
        /// <typeparam name="T">Item上组件的类型</typeparam>
        /// <param name="index">Item的索引</param>
        /// <returns>返回指定类型的组件</returns>
        public T GetItemAtIndex<T>(int index) where T : new()
        {
            if (m_ItemList == null || m_ItemList.Count < index) return default;
            return m_ItemList[index].gameObject.GetComponent<T>();
        }

        /// <summary>
        /// 根据索引获取Item对象
        /// </summary>
        /// <param name="index">Item的索引</param>
        /// <returns>返回Item的GameObject</returns>
        public GameObject GetItemAtIndex(int index)
        {
            if (m_ItemList == null || m_ItemList.Count < index) return null;
            return m_ItemList[index].gameObject;
        }

        #region 内部实现

        /// <summary>
        /// 协程分帧加载列表项
        /// </summary>
        private IEnumerator LoadItemsOverFrames<T>(List<T> datas)
        {
            ClearItems(); // 清空列表项数据

            yield return null;

            int currentIndex = 0;
            while (currentIndex < datas.Count)
            {
                // 每帧处理 itemsPerFrame 个元素
                int endIndex = Mathf.Min(currentIndex + m_ItemsPerFrame, datas.Count);

                for (int i = currentIndex; i < endIndex; i++)
                {
                    GameObject gameObject = m_ItemPool.Allocate();
                    ScrollViewItem item = gameObject.GetComponent<ScrollViewItem>();
                    item.SetDisableAnim(false); // 关闭弹窗动画
                    item.Open();
                    item.OnUpdateItemData(i, datas[i]);
                    m_ItemList.Add(gameObject.GetComponent<RectTransform>());

                    // 调用加载Item的回调
                    m_LoadItemHandle?.Invoke(item.gameObject);
                }

                // 当前帧处理完，暂停到下一帧
                currentIndex = endIndex;
                yield return null; // 等待一帧
            }

            yield return null;

            // 加载完成，触发回调并更新内容边界
            m_LoadCompletedHandle?.Invoke();
            UpdateContentBounds();
        }

        public void OpenAllItem()
        {
            foreach (var trans in m_ItemList)
            {
                var item = trans.gameObject.GetComponent<ScrollViewItem>();
                item.OnOpen();
            }
        }

        public void CloseAllItem()
        {
            foreach (var trans in m_ItemList)
            {
                var item = trans.gameObject.GetComponent<ScrollViewItem>();
                item.OnClose();
            }
        }

        #endregion
    }
}