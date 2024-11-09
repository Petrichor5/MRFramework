using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace MRFramework.UGUIPro
{
    [CustomEditor(typeof(ImagePro), true)]
    [CanEditMultipleObjects]
    public class ImageProEditor : ImageEditor
    {
        private static bool m_ImageMaskPanelOpen = false;
        
        private SerializedProperty m_IsUseMask;
        private SerializedProperty m_FillPercent;
        private SerializedProperty m_Fill;
        private SerializedProperty m_TrisCont;
        private SerializedProperty m_Segements;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            //ImageMask
            m_IsUseMask = serializedObject.FindProperty("ImageMaskExtend.IsUseMaskImage");
            m_FillPercent = serializedObject.FindProperty("ImageMaskExtend.FillPercent");
            m_Fill = serializedObject.FindProperty("ImageMaskExtend.Fill");
            m_TrisCont = serializedObject.FindProperty("ImageMaskExtend.TrisCont");
            m_Segements = serializedObject.FindProperty("ImageMaskExtend.Segements");

            m_ImageMaskPanelOpen = EditorPrefs.GetBool("UGUIPro.m_ImageMaskPanelOpen", m_ImageMaskPanelOpen);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ImageProGUI();

            serializedObject.ApplyModifiedProperties();
        }

        void ImageProGUI()
        {
            ImageProDrawEditor.DrawImageMask("裁剪遮罩", ref m_ImageMaskPanelOpen, m_IsUseMask, m_FillPercent, m_Fill,
                m_TrisCont, m_Segements);

            if (GUI.changed)
            {
                EditorPrefs.SetBool("UGUIPro.m_ImageMaskPanelOpen", m_ImageMaskPanelOpen);
            }
        }
    }
}