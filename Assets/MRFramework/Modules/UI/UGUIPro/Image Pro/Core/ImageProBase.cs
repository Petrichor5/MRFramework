using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class ImageProBase : Image
    {
        [SerializeField] public ImageMaskExtend ImageMaskExtend = new ImageMaskExtend();

        protected override void Awake()
        {
            base.Awake();
            ImageMaskExtend.Initializa(this);
        }

        private void Update()
        {
            ImageMaskExtend.Update();
        }

        public void ModifyFillPercent(float value)
        {
            ImageMaskExtend.FillPercent = value;
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            ImageMaskExtend.EditorInitializa(this);
        }
#endif
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (!ImageMaskExtend.IsUseMaskImage)
            {
                base.OnPopulateMesh(vh);
            }
            else
            {
                ImageMaskExtend.OnPopulateMesh(vh);
            }
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (!ImageMaskExtend.IsUseMaskImage)
            {
                return base.IsRaycastLocationValid(screenPoint, eventCamera);
            }
            else
            {
                Vector2 local;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera,
                    out local);
                return ImageMaskExtend.Contains(local, ImageMaskExtend.OutterVertices,
                    ImageMaskExtend.InnerVertices);
            }
        }
    }
}