using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    [CustomEditor(typeof(ScrollListPro), true)]
    public class ScrollListProEditor : ScrollRectEditor
    {
        private SerializedProperty m_ItemPrefab;
        private SerializedProperty m_ScrollType;
        private SerializedProperty m_ItemWidth;
        private SerializedProperty m_ItemHeight;
        private SerializedProperty m_ItemScaleX;
        private SerializedProperty m_ItemScaleY;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ItemPrefab = serializedObject.FindProperty("ItemPrefab");
            m_ScrollType = serializedObject.FindProperty("ScrollType");
            m_ItemWidth = serializedObject.FindProperty("ItemWidth");
            m_ItemHeight = serializedObject.FindProperty("ItemHeight");
            m_ItemScaleX = serializedObject.FindProperty("ItemScaleX");
            m_ItemScaleY = serializedObject.FindProperty("ItemScaleY");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_ItemPrefab, new GUIContent("ItemPrefab"));
            EditorGUILayout.PropertyField(m_ScrollType, new GUIContent("ScrollType"));
            EditorGUILayout.PropertyField(m_ItemWidth, new GUIContent("ItemWidth"));
            EditorGUILayout.PropertyField(m_ItemHeight, new GUIContent("ItemHeight"));
            EditorGUILayout.PropertyField(m_ItemScaleX, new GUIContent("ItemScaleX"));
            EditorGUILayout.PropertyField(m_ItemScaleY, new GUIContent("ItemScaleY"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}