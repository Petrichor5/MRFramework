using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    [CustomEditor(typeof(ScrollViewPro), true)]
    public class ScrollViewProEditor : ScrollRectEditor
    {
        private SerializedProperty m_ItemPrefab;
        private SerializedProperty m_ScrollType;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ItemPrefab = serializedObject.FindProperty("ItemPrefab");
            m_ScrollType = serializedObject.FindProperty("ScrollType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(m_ItemPrefab, new GUIContent("ItemPrefab"));
            EditorGUILayout.PropertyField(m_ScrollType, new GUIContent("ScrollType"));
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}