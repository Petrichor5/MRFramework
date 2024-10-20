using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    [CustomEditor(typeof(ScrollViewPro), true)]
    public class ScrollViewProEditor : ScrollRectEditor
    {
        private SerializedProperty m_ItemPrefab;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ItemPrefab = serializedObject.FindProperty("ItemPrefab");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(m_ItemPrefab, new GUIContent("ItemPrefab"));
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}