using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

 namespace MRFramework.UGUIPro
 {
	public class ButtonProDrawEditor  
	{
	    [MenuItem("GameObject/UI/UGUI Pro/Button Pro")]
	    public static void CreateButtonPro()
	    {
			GameObject button = new GameObject("Button Pro", typeof(RectTransform), typeof(Image), typeof(ButtonPro), typeof(CanvasGroup));

            RectTransform buttonProRectTrans = button.GetComponent<RectTransform>();
	        ResetInCanvasFor((RectTransform)buttonProRectTrans.transform);
	        
	        TextMeshPro text = new GameObject("TextMesh Pro", typeof(RectTransform), typeof(TextMeshPro)).GetComponent<TextMeshPro>();
	        text.transform.SetParent(buttonProRectTrans);
	        text.transform.localPosition = Vector3.zero;
	        text.transform.localScale = Vector3.one;
	        text.transform.rotation = Quaternion.identity;
	        text.color = Color.black;
	        text.text = "Button Pro";
	        text.fontSize = 24;
	        text.alignment = TMPro.TextAlignmentOptions.Midline;
	        text.raycastTarget = false;
	        
	        buttonProRectTrans.sizeDelta= text.rectTransform.sizeDelta = new Vector2(163,50);
	        buttonProRectTrans.localPosition = Vector3.zero;
	    }
	
	    public static void DrawDoubleClickGUI(string title, ref bool m_PanelOpen, SerializedProperty isuseDoubleClick , SerializedProperty clickInterval, SerializedProperty evetnts)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(isuseDoubleClick);
	            if (isuseDoubleClick.boolValue)
	            {
	                EditorGUILayout.PropertyField(clickInterval);
	                EditorGUILayout.PropertyField(evetnts);
	            }
	        }, title, ref m_PanelOpen, true);
	    }
	    public static void DrawLongPressGUI(string title, ref bool m_PanelOpen, SerializedProperty isuseLongPress, SerializedProperty duration, SerializedProperty evetnts)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(isuseLongPress);
	            if (isuseLongPress.boolValue)
	            {
	                EditorGUILayout.PropertyField(duration);
	                EditorGUILayout.PropertyField(evetnts);
	            }
	        }, title, ref m_PanelOpen, true);
	    }
		public static void DrawSignGUI(string title, ref bool m_PanelOpen, SerializedProperty isuseLongPress)
		{
			LayoutFrameBox(() =>
			{
				EditorGUILayout.PropertyField(isuseLongPress);
			}, title, ref m_PanelOpen, true);
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
