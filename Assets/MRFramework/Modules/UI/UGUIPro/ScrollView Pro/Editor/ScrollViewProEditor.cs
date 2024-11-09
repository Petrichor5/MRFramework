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
        private SerializedProperty m_ItemScaleX;
        private SerializedProperty m_ItemScaleY;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ItemPrefab = serializedObject.FindProperty("ItemPrefab");
            m_ScrollType = serializedObject.FindProperty("ScrollType");
            m_ItemScaleX = serializedObject.FindProperty("ItemScaleX");
            m_ItemScaleY = serializedObject.FindProperty("ItemScaleY");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(m_ItemPrefab, new GUIContent("ItemPrefab"));
            EditorGUILayout.PropertyField(m_ScrollType, new GUIContent("ScrollType"));
            EditorGUILayout.PropertyField(m_ItemScaleX, new GUIContent("ItemScaleX"));
            EditorGUILayout.PropertyField(m_ItemScaleY, new GUIContent("ItemScaleY"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}