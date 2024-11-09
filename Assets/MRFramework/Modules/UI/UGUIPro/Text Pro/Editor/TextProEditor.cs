using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;

namespace MRFramework.UGUIPro
{
    [CustomEditor(typeof(TextPro), true)]
    [CanEditMultipleObjects]
    public class TextProEditor : GraphicEditor
    {
        private static bool m_TextSpacingPanelOpen = false;
        private static bool m_VertexColorPanelOpen = false;
        private static bool m_TextShadowPanelOpen = false;
        private static bool m_TextOutlinePanelOpen = false;
        private static bool m_LocalizationTextPanelOpen = false;
        private static bool m_TextEffectPanelOpen = false;

        SerializedProperty m_Text;
        SerializedProperty m_FontData;

        //text spacing
        SerializedProperty m_UseTextSpacing;
        SerializedProperty m_TextSpacing;

        // Ver Color
        SerializedProperty m_UseVertexColor;
        SerializedProperty m_VertexColorFilter;
        SerializedProperty m_VertexColorOffset;
        SerializedProperty m_VertexTopLeft;
        SerializedProperty m_VertexTopRight;
        SerializedProperty m_VertexBottomLeft;
        SerializedProperty m_VertexBottomRight;

        //Shadow
        SerializedProperty m_UseShadow;
        SerializedProperty m_ShadowColorTopLeft;
        SerializedProperty m_ShadowColorTopRight;
        SerializedProperty m_ShadowColorBottomLeft;
        SerializedProperty m_ShadowColorBottomRight;
        SerializedProperty m_ShadowEffectDistance;

        //TextEffect
        SerializedProperty m_UseTextEffect;
        SerializedProperty m_GradientType;
        SerializedProperty m_TopColor;
        SerializedProperty m_OpenShaderOutLine;
        SerializedProperty m_MiddleColor;
        SerializedProperty m_BottomColor;
        SerializedProperty m_ColorOffset;
        SerializedProperty m_EnableOutLine;
        SerializedProperty m_OutLineColor;
        SerializedProperty m_OutLineWidth;
        SerializedProperty m_Camera;
        SerializedProperty m_LerpValue;
        SerializedProperty m_Alpha;
        SerializedProperty m_TextEffect;

        protected override void OnEnable()
        {
            base.OnEnable();

            TextPro m_TextPlus = (TextPro)this.target;
            m_TextPlus.TextEffectExtend.SaveSerializeData(m_TextPlus);


            m_Text = serializedObject.FindProperty("m_Text");
            m_FontData = serializedObject.FindProperty("m_FontData");

            //text spacing
            m_UseTextSpacing = serializedObject.FindProperty("m_TextSpacingExtend.m_UseTextSpacing");
            m_TextSpacing = serializedObject.FindProperty("m_TextSpacingExtend.m_TextSpacing");

            // VertexColor
            m_UseVertexColor = serializedObject.FindProperty("m_VertexColorExtend.m_UseVertexColor");
            m_VertexColorFilter = serializedObject.FindProperty("m_VertexColorExtend.m_VertexColorFilter");
            m_VertexTopLeft = serializedObject.FindProperty("m_VertexColorExtend.m_VertexTopLeft");
            m_VertexTopRight = serializedObject.FindProperty("m_VertexColorExtend.m_VertexTopRight");
            m_VertexBottomLeft = serializedObject.FindProperty("m_VertexColorExtend.m_VertexBottomLeft");
            m_VertexBottomRight = serializedObject.FindProperty("m_VertexColorExtend.m_VertexBottomRight");
            m_VertexColorOffset = serializedObject.FindProperty("m_VertexColorExtend.m_VertexColorOffset");

            //Shadow
            m_UseShadow = serializedObject.FindProperty("m_TextShadowExtend.m_UseShadow");
            m_ShadowColorTopLeft = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorTopLeft");
            m_ShadowColorTopRight = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorTopRight");
            m_ShadowColorBottomLeft = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorBottomLeft");
            m_ShadowColorBottomRight = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorBottomRight");
            m_ShadowEffectDistance = serializedObject.FindProperty("m_TextShadowExtend.m_EffectDistance");

            //TextEffect
            m_UseTextEffect = serializedObject.FindProperty("m_TextEffectExtend.m_UseTextEffect");
            m_Alpha = this.serializedObject.FindProperty("m_TextEffectExtend.m_Alpha");
            m_GradientType = this.serializedObject.FindProperty("m_TextEffectExtend.m_GradientType");
            m_TopColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_TopColor");
            m_OpenShaderOutLine = this.serializedObject.FindProperty("m_TextEffectExtend.m_OpenShaderOutLine");
            m_MiddleColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_MiddleColor");
            m_BottomColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_BottomColor");
            m_ColorOffset = this.serializedObject.FindProperty("m_TextEffectExtend.m_ColorOffset");
            m_EnableOutLine = this.serializedObject.FindProperty("m_TextEffectExtend.m_EnableOutLine");
            m_OutLineColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_OutLineColor");
            m_OutLineWidth = this.serializedObject.FindProperty("m_TextEffectExtend.m_OutLineWidth");
            m_Camera = this.serializedObject.FindProperty("m_TextEffectExtend.m_Camera");
            m_LerpValue = this.serializedObject.FindProperty("m_TextEffectExtend.m_LerpValue");
            m_TextEffect = this.serializedObject.FindProperty("m_TextEffectExtend.m_TextEffect");
            // Panel Open
            m_TextSpacingPanelOpen = EditorPrefs.GetBool("UGUIPro.m_TextSpacingPanelOpen", m_TextSpacingPanelOpen);
            m_VertexColorPanelOpen = EditorPrefs.GetBool("UGUIPro.m_VertexColorPanelOpen", m_VertexColorPanelOpen);
            m_TextShadowPanelOpen = EditorPrefs.GetBool("UGUIPro.m_TextShadowPanelOpen", m_TextShadowPanelOpen);
            m_TextOutlinePanelOpen = EditorPrefs.GetBool("UGUIPro.m_TextOutlinePanelOpen", m_TextOutlinePanelOpen);
            m_LocalizationTextPanelOpen =
                EditorPrefs.GetBool("UGUIPro.m_LocalizationTextPanelOpen", m_LocalizationTextPanelOpen);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Text);
            EditorGUILayout.PropertyField(m_FontData);
            AppearanceControlsGUI();
            RaycastControlsGUI();
            TextProGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void TextProGUI()
        {
            GUI.enabled = false;
            if (m_TextEffect.objectReferenceValue != null)
            {
                EditorGUILayout.ObjectField("Graphic", ((TextEffect)m_TextEffect.objectReferenceValue).TextGraphic,
                    typeof(Text), false);
            }

            GUI.enabled = true;

            TextProDrawEditor.TextSpacingGUI("字符间距", m_UseTextSpacing, m_TextSpacing, ref m_TextSpacingPanelOpen);

            TextProDrawEditor.VertexColorGUI(
                "顶点颜色",
                m_UseVertexColor,
                m_VertexTopLeft,
                m_VertexTopRight,
                m_VertexBottomLeft,
                m_VertexBottomRight,
                m_VertexColorFilter,
                m_VertexColorOffset,
                ref m_VertexColorPanelOpen
            );

            TextProDrawEditor.TextShadowGUI(
                "阴影",
                m_UseShadow,
                m_ShadowColorTopLeft,
                m_ShadowColorTopRight,
                m_ShadowColorBottomLeft,
                m_ShadowColorBottomRight,
                m_ShadowEffectDistance,
                ref m_TextShadowPanelOpen
            );

            TextProDrawEditor.TextEffectGUI("描边&渐变", m_UseTextEffect, ref m_TextEffectPanelOpen,
                m_GradientType,
                m_TopColor,
                m_OpenShaderOutLine,
                m_MiddleColor,
                m_BottomColor,
                m_ColorOffset,
                m_EnableOutLine,
                m_OutLineColor,
                m_OutLineWidth,
                m_Camera,
                m_LerpValue,
                m_Alpha,
                (TextEffect)m_TextEffect.objectReferenceValue
            );

            if (GUI.changed)
            {
                EditorPrefs.SetBool("UGUIPro.m_TextSpacingPanelOpen", m_TextSpacingPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_VertexColorPanelOpen", m_VertexColorPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_TextShadowPanelOpen", m_TextShadowPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_TextOutlinePanelOpen", m_TextOutlinePanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_LocalizationTextPanelOpen", m_LocalizationTextPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_TextEffectPanelOpen", m_TextEffectPanelOpen);
            }
        }
    }
}