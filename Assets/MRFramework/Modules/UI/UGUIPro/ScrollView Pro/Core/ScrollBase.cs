using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ScrollBase : ScrollRect
    {
        [Header("Item预制体")]
        public AssetReferenceGameObject ItemPrefab;
        [Header("滚动方向类型")]
        public EScrollType ScrollType;

        private CanvasGroup m_CanvasGroup;
        private bool m_IsVisible;

        protected SimpleObjectPool<GameObject> m_ItemPool; // Item 对象池
        protected List<RectTransform> m_ItemList; // 保存可见项的 RectTransform

        protected Action m_LoadCompletedHandle; // 加载完毕回调
        protected Action<GameObject> m_LoadItemHandle; // 加载每个Item的回调

        // Item的宽高
        protected float m_ItemWidth;
        protected float m_ItemHeight;

        // Item偏移值
        protected float m_ItemOffsetX;
        protected float m_ItemOffsetY;

        // Grid Padding
        protected float m_PaddingTop;
        protected float m_PaddingBottom;
        protected float m_PaddingLeft;
        protected float m_PaddingRight;

        protected bool m_IsInit = false; // 是否初始化
        protected int m_ItemsPerFrame = 5; // 分帧加载，每帧创建的Item数量

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

        #region 对外接口

        public virtual void OnInit()
        {
            if (m_IsInit)
            {
                Log.Error("[ScrollViewPro] 滚动控件重复初始化！");
                return;
            }
            Initialize();
            m_IsInit = true;
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

        /// <summary>
        /// 设置列表项数据
        /// </summary>
        public void SetItems<T>(List<T> datas)
        {
            if (m_IsInit)
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
            if (m_IsInit)
            {
                foreach (var item in m_ItemList)
                {
                    item.gameObject.GetComponent<ScrollItem>().OnDispose();
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

        #endregion

        #region 内部实现

        protected virtual void Initialize()
        {
            m_ItemPool = new SimpleObjectPool<GameObject>(CreateItem, ResetItem);
            m_ItemList = new List<RectTransform>();

            switch (ScrollType)
            {
                case EScrollType.Horizontal:
                    InitHorizontal();
                    break;
                case EScrollType.Vertical:
                    InitVertical();
                    break;
            }
        }

        protected virtual void InitHorizontal()
        {
            
        }

        protected virtual void InitVertical()
        {
            
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
            if (aoh.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject gameObject = aoh.Result;
                ScrollItem item = gameObject.GetComponent<ScrollItem>();
                item.OnFirstOpen();
                return gameObject;
            }
            return null;
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

        /// <summary>
        /// 分帧加载Item
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
                    LoadItemFormPool(i, datas[i]);
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

        private void LoadItemFormPool<T>(int index, T data)
        {
            GameObject gameObject = m_ItemPool.Allocate();
            ScrollItem item = gameObject.GetComponent<ScrollItem>();
            item.SetDisableAnim(false); // 关闭弹窗动画
            item.Open();
            item.OnUpdateItemData(index, data);
            m_ItemList.Add(gameObject.GetComponent<RectTransform>());

            // 调用加载Item的回调
            m_LoadItemHandle?.Invoke(item.gameObject);
        }

        /// <summary>
        /// 计算Content包边范围
        /// </summary>
        protected virtual void UpdateContentBounds()
        {

        }

        public void OpenAllItem()
        {
            foreach (var trans in m_ItemList)
            {
                var item = trans.gameObject.GetComponent<ScrollItem>();
                item.OnOpen();
            }
        }

        public void CloseAllItem()
        {
            foreach (var trans in m_ItemList)
            {
                var item = trans.gameObject.GetComponent<ScrollItem>();
                item.OnClose();
            }
        }

        #endregion
    }
}