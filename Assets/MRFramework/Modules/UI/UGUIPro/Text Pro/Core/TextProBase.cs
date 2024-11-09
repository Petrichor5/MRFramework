using UnityEngine;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class TextProBase : Text, IMeshModifier
    {
        [SerializeField] public string winName;
        [SerializeField] TextSpacingExtend m_TextSpacingExtend = new TextSpacingExtend();
        [SerializeField] VertexColorExtend m_VertexColorExtend = new VertexColorExtend();
        [SerializeField] TextShadowExtend m_TextShadowExtend = new TextShadowExtend();
        [SerializeField] TextOutlineExtend m_TextOutlineExtend = new TextOutlineExtend();
        [SerializeField] TextEffectExtend m_TextEffectExtend = new TextEffectExtend();

        public TextSpacingExtend TextSpacingExtend
        {
            get { return m_TextSpacingExtend; }
        }

        public VertexColorExtend VertexColorExtend
        {
            get { return m_VertexColorExtend; }
        }

        public TextShadowExtend TextShadowHandler
        {
            get { return m_TextShadowExtend; }
        }

        public TextOutlineExtend TextOutlineHandler
        {
            get { return m_TextOutlineExtend; }
        }

        public TextEffectExtend TextEffectExtend
        {
            get { return m_TextEffectExtend; }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            if (m_TextSpacingExtend.UseTextSpacing)
                m_TextSpacingExtend.PopulateMesh(toFill);
            if (m_VertexColorExtend.UseVertexColor)
                m_VertexColorExtend.PopulateMesh(toFill, rectTransform, color);
            if (m_TextShadowExtend.UseShadow)
                m_TextShadowExtend.PopulateMesh(toFill, rectTransform, color);
            if (m_TextOutlineExtend.UseOutline)
                m_TextOutlineExtend.PopulateMesh(toFill);
        }

        public void ModifyMesh(VertexHelper vh)
        {
            //if (m_TextShadowExtend.UseShadow)
            //    m_TextShadowExtend.PopulateMesh(vh, rectTransform, color);
        }

        public void ModifyMesh(Mesh mesh)
        {
        }

        public void SetTextAlpha(float alpha)
        {
            if (m_TextEffectExtend.UseTextEffect && m_TextEffectExtend.GradientType != 0)
            {
                m_TextEffectExtend.SetAlpha(alpha);
            }
            else
            {
                Color32 color32 = color;
                color32.a = (byte)(alpha * 255);
                color = color32;
            }
        }

        public void SetOutLineColor(Color32 color)
        {
            if (!m_TextEffectExtend.UseTextEffect) return;
            m_TextEffectExtend.m_TextEffect.SetOutLineColor(color);
            m_TextEffectExtend.UseTextEffect = false;
            m_TextEffectExtend.UseTextEffect = true;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
    }
}