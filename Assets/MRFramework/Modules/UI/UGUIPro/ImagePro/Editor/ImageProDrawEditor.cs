using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    public class ImageProDrawEditor
    {
        [MenuItem("GameObject/UI/UGUI Pro/Image Pro")]
        public static void CreateImagePro()
        {
            GameObject root = new GameObject("Image Pro", typeof(RectTransform), typeof(ImagePro), typeof(CanvasGroup));
            ResetInCanvasFor((RectTransform)root.transform);

            root.transform.localPosition = Vector3.zero;
        }

        [MenuItem("GameObject/UI/UGUI Pro/FilletImage")]
        public static void CreateFilletImage()
        {
            GameObject root = new GameObject("Fillet Image", typeof(RectTransform), typeof(FilletImage));
            ResetInCanvasFor((RectTransform)root.transform);
            root.transform.localPosition = Vector3.zero;
        }

        public static T EditorAssetLaod<T>(string path, bool isAssetbundle) where T : Object
        {
            return isAssetbundle ? AssetDatabase.LoadAssetAtPath<T>(path) : Resources.Load<T>(path);
        }

        public static void DrawImageMask(string title, ref bool m_PanelOpen, SerializedProperty useMask,
            SerializedProperty fillPercent, SerializedProperty fill, SerializedProperty triscont,
            SerializedProperty segements)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(useMask);
                if (useMask.boolValue)
                {
                    EditorGUILayout.PropertyField(fillPercent);
                    EditorGUILayout.PropertyField(fill);
                    EditorGUILayout.PropertyField(triscont);
                    EditorGUILayout.PropertyField(segements);
                }
            }, title, ref m_PanelOpen, true);
        }

        public static void SimpleUseGUI(string title, ref bool m_PanelOpen, float space, SerializedProperty useThis,
            params SerializedProperty[] sps)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(useThis);
                if (useThis.boolValue)
                {
                    foreach (var s in sps)
                    {
                        if (s != null)
                        {
                            EditorGUILayout.PropertyField(s);
                        }
                    }
                }
            }, title, ref m_PanelOpen, true);
        }

        public static void SendWillRenderCanvases()
        {
            GameObject obj = Selection.activeTransform.gameObject;
            obj.SetActive(false);
            obj.SetActive(true);
        }

        private static void ResetInCanvasFor(RectTransform root)
        {
            root.SetParent(Selection.activeTransform);
            if (!InCanvas(root))
            {
                Transform canvasTF = GetCreateCanvas();
                root.SetParent(canvasTF);
            }

            if (!Transform.FindObjectOfType<UnityEngine.EventSystems.EventSystem>())
            {
                GameObject eg = new GameObject("EventSystem");
                eg.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eg.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            root.localScale = Vector3.one;
            root.localPosition = new Vector3(root.localPosition.x, root.localPosition.y, 0f);
            Selection.activeGameObject = root.gameObject;
        }

        private static bool InCanvas(Transform tf)
        {
            while (tf.parent)
            {
                tf = tf.parent;
                if (tf.GetComponent<Canvas>())
                {
                    return true;
                }
            }

            return false;
        }

        private static Transform GetCreateCanvas()
        {
            Canvas c = Object.FindObjectOfType<Canvas>();
            if (c)
            {
                return c.transform;
            }
            else
            {
                GameObject g = new GameObject("Canvas");
                c = g.AddComponent<Canvas>();
                c.renderMode = RenderMode.ScreenSpaceOverlay;
                g.AddComponent<CanvasScaler>();
                g.AddComponent<GraphicRaycaster>();
                return g.transform;
            }
        }

        private static void LayoutFrameBox(System.Action action, string label, ref bool open, bool box = false)
        {
            bool _open = open;
            LayoutVertical(() =>
            {
                _open = GUILayout.Toggle(
                    _open,
                    label,
                    GUI.skin.GetStyle("foldout"),
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(18)
                );
                if (_open)
                {
                    action();
                }
            }, box);
            open = _open;
        }
        
        private static void LayoutVertical(System.Action action, bool box = false)
        {
            if (box)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(6, 6, 2, 2)
                };
                GUILayout.BeginVertical(style);
            }
            else
            {
                GUILayout.BeginVertical();
            }

            action();
            GUILayout.EndVertical();
        }
    }
}