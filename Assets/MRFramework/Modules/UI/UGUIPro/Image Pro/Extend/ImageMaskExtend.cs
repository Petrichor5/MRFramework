using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using System;

namespace MRFramework.UGUIPro
{
    [Serializable]
    public class ImageMaskExtend
    {
        public Image Image;

        private RectTransform m_RectTransform;
        
        [SerializeField]
        public bool IsUseMaskImage = false;
        
        [SerializeField] [Tooltip("圆形或扇形填充比例")] [Range(0, 1)]
        public float FillPercent = 1f;

        [SerializeField] [Tooltip("是否填充圆形")] 
        public bool Fill = true;
        
        [Tooltip("圆环宽度")] 
        public float TrisCont = 5;

        [SerializeField] [Tooltip("圆形")] [Range(3, 100)]
        public int Segements = 20;

        public List<Vector3> InnerVertices;
        public List<Vector3> OutterVertices;

        //private Sprite z_Sprite;
        //public Sprite sprite { get { return z_Sprite; } set { if (SetPropertyUtilityExtend.SetClass(ref z_Sprite, value)) m_Image.SetAllDirty(); } }
        public Sprite overrideSprite
        {
            get { return Image.overrideSprite; }
            set
            {
                Image.overrideSprite = value;
                Image.SetAllDirty();
            }
        }

        public void Initializa(Image image)
        {
            Image = image;
            m_RectTransform = Image.rectTransform;
            InnerVertices = new List<Vector3>();
            OutterVertices = new List<Vector3>();
        }
        
#if UNITY_EDITOR
        public void EditorInitializa(Image image)
        {
            Image = image;
            m_RectTransform = Image.rectTransform;
        }
#endif
        
        // Update is called once per frame
        public void Update()
        {
            this.TrisCont = Mathf.Clamp(this.TrisCont, 0, Image.rectTransform.rect.width / 2);
        }

        public void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            InnerVertices.Clear();
            OutterVertices.Clear();

            float degreeDelta = (2 * Mathf.PI / Segements);
            int curSegements = (int)(Segements * FillPercent);

            float tw = m_RectTransform.rect.width;
            float th = m_RectTransform.rect.height;
            float outerRadius = m_RectTransform.pivot.x * tw;
            float innerRadius = m_RectTransform.pivot.x * tw - TrisCont;

            Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

            float uvCenterX = (uv.x + uv.z) * 0.5f;
            float uvCenterY = (uv.y + uv.w) * 0.5f;
            float uvScaleX = (uv.z - uv.x) / tw;
            float uvScaleY = (uv.w - uv.y) / th;

            float curDegree = 0;
            UIVertex uiVertex;
            int verticeCount;
            int triangleCount;
            Vector2 curVertice;
            //Log.Info("m_Fill:"+ m_Fill);
            if (Fill) //圆形
            {
                curVertice = Vector2.zero;
                verticeCount = curSegements + 1;
                uiVertex = new UIVertex();
                uiVertex.color = Image.color;
                uiVertex.position = curVertice;
                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
                vh.AddVert(uiVertex);

                for (int i = 1; i < verticeCount; i++)
                {
                    float cosA = Mathf.Cos(curDegree);
                    float sinA = Mathf.Sin(curDegree);
                    curVertice = new Vector2(cosA * outerRadius, sinA * outerRadius);
                    curDegree += degreeDelta;

                    uiVertex = new UIVertex();
                    uiVertex.color = Image.color;
                    uiVertex.position = curVertice;
                    uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX,
                        curVertice.y * uvScaleY + uvCenterY);
                    vh.AddVert(uiVertex);

                    OutterVertices.Add(curVertice);
                }

                triangleCount = curSegements * 3;
                for (int i = 0, vIdx = 1; i < triangleCount - 3; i += 3, vIdx++)
                {
                    vh.AddTriangle(vIdx, 0, vIdx + 1);
                }

                if (FillPercent == 1)
                {
                    //首尾顶点相连
                    vh.AddTriangle(verticeCount - 1, 0, 1);
                }
            }
            else //圆环
            {
                verticeCount = curSegements * 2;
                for (int i = 0; i < verticeCount; i += 2)
                {
                    float cosA = Mathf.Cos(curDegree);
                    float sinA = Mathf.Sin(curDegree);
                    curDegree += degreeDelta;

                    curVertice = new Vector3(cosA * innerRadius, sinA * innerRadius);
                    uiVertex = new UIVertex();
                    uiVertex.color = Image.color;
                    uiVertex.position = curVertice;
                    uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX,
                        curVertice.y * uvScaleY + uvCenterY);
                    vh.AddVert(uiVertex);
                    InnerVertices.Add(curVertice);

                    curVertice = new Vector3(cosA * outerRadius, sinA * outerRadius);
                    uiVertex = new UIVertex();
                    uiVertex.color = Image.color;
                    uiVertex.position = curVertice;
                    uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX,
                        curVertice.y * uvScaleY + uvCenterY);
                    vh.AddVert(uiVertex);
                    OutterVertices.Add(curVertice);
                }

                triangleCount = curSegements * 3 * 2;
                for (int i = 0, vIdx = 0; i < triangleCount - 6; i += 6, vIdx += 2)
                {
                    vh.AddTriangle(vIdx + 1, vIdx, vIdx + 3);
                    vh.AddTriangle(vIdx, vIdx + 2, vIdx + 3);
                }

                if (FillPercent == 1)
                {
                    //首尾顶点相连
                    vh.AddTriangle(verticeCount - 1, verticeCount - 2, 1);
                    vh.AddTriangle(verticeCount - 2, 0, 1);
                }
            }
        }

        public bool Contains(Vector2 p, List<Vector3> outterVertices, List<Vector3> innerVertices)
        {
            var crossNumber = 0;
            RayCrossing(p, innerVertices, ref crossNumber); //检测内环
            RayCrossing(p, outterVertices, ref crossNumber); //检测外环
            return (crossNumber & 1) == 1;
        }

        /// <summary>
        /// 使用RayCrossing算法判断点击点是否在封闭多边形里
        /// </summary>
        /// <param name="p"></param>
        /// <param name="vertices"></param>
        /// <param name="crossNumber"></param>
        private void RayCrossing(Vector2 p, List<Vector3> vertices, ref int crossNumber)
        {
            for (int i = 0, count = vertices.Count; i < count; i++)
            {
                var v1 = vertices[i];
                var v2 = vertices[(i + 1) % count];

                //点击点水平线必须与两顶点线段相交
                if (((v1.y <= p.y) && (v2.y > p.y))
                    || ((v1.y > p.y) && (v2.y <= p.y)))
                {
                    //只考虑点击点右侧方向，点击点水平线与线段相交，且交点x > 点击点x，则crossNumber+1
                    if (p.x < v1.x + (p.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
                    {
                        crossNumber += 1;
                    }
                }
            }
        }
    }
}