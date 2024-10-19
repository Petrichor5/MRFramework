using UnityEngine;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ImagePro : ImageProBase
    {
        private string m_SpriteAssetKey;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ReleaseSprite();
        }

        /// <summary>
        /// 设置精灵图片
        /// </summary>
        /// <param name="key">资源的Key</param>
        public void SetSprite(string key)
        {
            // 相同的图片不做处理
            if (m_SpriteAssetKey == key) return;

            m_SpriteAssetKey = key;
            AssetManager.Instance.LoadAssetAsync<Sprite>(key, (result) =>
            {
                this.sprite = result;
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