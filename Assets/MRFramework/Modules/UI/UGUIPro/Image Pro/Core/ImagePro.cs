using UnityEngine;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ImagePro : ImageProBase
    {
        private CanvasGroup m_CanvasGroup;
        private bool m_IsVisible;

        private string m_SpriteAssetKey;

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ReleaseSprite();
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
        /// 设置精灵图片
        /// </summary>
        /// <param name="key">资源的Key</param>
        public void SetSprite(string key, bool setNativeSize = false)
        {
            // 相同的图片不做处理
            if (m_SpriteAssetKey == key) return;

            m_SpriteAssetKey = key;
            AssetManager.Instance.LoadAssetAsync<Sprite>(key, (result) =>
            {
                this.sprite = result;
                if (setNativeSize)
                    SetNativeSize();
            });
        }

        /// <summary>
        /// 释放精灵图片资源
        /// </summary>
        public void ReleaseSprite()
        {
            if (!string.IsNullOrEmpty(m_SpriteAssetKey))
            {
                this.sprite = null;
                AssetManager.Instance.ReleaseAsset<Sprite>(m_SpriteAssetKey);
                m_SpriteAssetKey = null;
            }
        }
    }
}