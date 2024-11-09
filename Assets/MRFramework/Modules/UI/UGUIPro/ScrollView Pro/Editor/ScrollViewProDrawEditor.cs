using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;

namespace MRFramework.UGUIPro
{
    public class ScrollViewProDrawEditor
    {
        private const float  kWidth       = 160f;
        private const float  kThinHeight  = 20f;
        
        private static Vector2 s_ThinElementSize        = new Vector2(kWidth, kThinHeight);
        private static Color   s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color   s_PanelColor             = new Color(1f, 1f, 1f, 0.392f);
        
        private const string kStandardSpritePath       = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath     = "UI/Skin/Background.psd";
        private const string kMaskPath                 = "UI/Skin/UIMask.psd";
        
        static private DefaultControls.Resources s_StandardResources;

        static private DefaultControls.Resources GetStandardResources()
        {
            if (s_StandardResources.standard == null)
            {
                s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
                s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
                s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            }
            return s_StandardResources;
        }

        [MenuItem("GameObject/UI/UGUI Pro/ScrollView Pro (Horizontal)")]
        public static void CreateScrollViewProHorizontal()
        {
            OnCreate(EScrollType.Horizontal);
        }

        [MenuItem("GameObject/UI/UGUI Pro/ScrollView Pro (Vertical)")]
        public static void CreateScrollViewProVertical()
        {
            OnCreate(EScrollType.Vertical);
        }

        public static void OnCreate(EScrollType eScrollType)
        {
            var resources = GetStandardResources();
            
            GameObject root = CreateUIElementRoot("ScrollView Pro", new Vector2(200, 200), typeof(Image), typeof(ScrollViewPro), typeof(CanvasGroup));

            GameObject viewport = CreateUIObject("Viewport", root, typeof(Image), typeof(Mask));
            GameObject content = CreateUIObject("Content", viewport, typeof(RectTransform));
            content.AddComponent<GridLayoutGroup>();

            // Sub controls.
            GameObject hScrollbar = null;
            GameObject vScrollbar = null;
            switch (eScrollType)
            {
                case EScrollType.Horizontal:
                    hScrollbar = CreateScrollbar(resources);
                    hScrollbar.name = "Scrollbar Horizontal";
                    SetParentAndAlign(hScrollbar, root);
                    RectTransform hScrollbarRT = hScrollbar.GetComponent<RectTransform>();
                    hScrollbarRT.anchorMin = Vector2.zero;
                    hScrollbarRT.anchorMax = Vector2.right;
                    hScrollbarRT.pivot = Vector2.zero;
                    hScrollbarRT.sizeDelta = new Vector2(0, hScrollbarRT.sizeDelta.y);
                    break;
                case EScrollType.Vertical:
                    vScrollbar = CreateScrollbar(resources);
                    vScrollbar.name = "Scrollbar Vertical";
                    SetParentAndAlign(vScrollbar, root);
                    vScrollbar.GetComponent<Scrollbar>().SetDirection(Scrollbar.Direction.BottomToTop, true);
                    RectTransform vScrollbarRT = vScrollbar.GetComponent<RectTransform>();
                    vScrollbarRT.anchorMin = Vector2.right;
                    vScrollbarRT.anchorMax = Vector2.one;
                    vScrollbarRT.pivot = Vector2.one;
                    vScrollbarRT.sizeDelta = new Vector2(vScrollbarRT.sizeDelta.x, 0);
                    break;
            }

            // Setup RectTransforms.

            // Make viewport fill entire scroll view.
            RectTransform viewportRT = viewport.GetComponent<RectTransform>();
            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.sizeDelta = Vector2.zero;
            viewportRT.pivot = Vector2.up;

            // Make context match viewpoprt width and be somewhat taller.
            // This will show the vertical scrollbar and not the horizontal one.
            RectTransform contentRT = content.GetComponent<RectTransform>();
            switch (eScrollType)
            {
                case EScrollType.Horizontal:
                    contentRT.anchorMin = Vector2.zero;
                    contentRT.anchorMax = Vector2.up;
                    contentRT.sizeDelta = new Vector2(300, 0);
                    break;
                case EScrollType.Vertical:
                    contentRT.anchorMin = Vector2.up;
                    contentRT.anchorMax = Vector2.one;
                    contentRT.sizeDelta = new Vector2(0, 300);
                    break;
            }
            contentRT.pivot = Vector2.up;

            // Setup UI components.

            ScrollViewPro scrollRect = root.GetComponent<ScrollViewPro>();
            scrollRect.content = contentRT;
            scrollRect.viewport = viewportRT;
            if (hScrollbar)
                scrollRect.horizontalScrollbar = hScrollbar.GetComponent<Scrollbar>();
            if (vScrollbar)
                scrollRect.verticalScrollbar = vScrollbar.GetComponent<Scrollbar>();
            scrollRect.horizontalScrollbarVisibility = ScrollViewPro.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarVisibility = ScrollViewPro.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.horizontalScrollbarSpacing = -3;
            scrollRect.verticalScrollbarSpacing = -3;

            Image rootImage = root.GetComponent<Image>();
            rootImage.sprite = resources.background;
            rootImage.type = Image.Type.Sliced;
            rootImage.color = s_PanelColor;

            Mask viewportMask = viewport.GetComponent<Mask>();
            viewportMask.showMaskGraphic = false;

            Image viewportImage = viewport.GetComponent<Image>();
            viewportImage.sprite = resources.mask;
            viewportImage.type = Image.Type.Sliced;
            
            // Reset Position
            var rect = (RectTransform)root.transform;
            ResetInCanvasFor(rect);
            rect.localPosition = Vector3.zero;
        }
        
        public static GameObject CreateScrollbar(DefaultControls.Resources resources)
        {
            // Create GOs Hierarchy
            GameObject scrollbarRoot = CreateUIElementRoot("Scrollbar", s_ThinElementSize, typeof(Image), typeof(Scrollbar));

            GameObject sliderArea = CreateUIObject("Sliding Area", scrollbarRoot, typeof(RectTransform));
            GameObject handle = CreateUIObject("Handle", sliderArea, typeof(Image));

            Image bgImage = scrollbarRoot.GetComponent<Image>();
            bgImage.sprite = resources.background;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = s_DefaultSelectableColor;

            Image handleImage = handle.GetComponent<Image>();
            handleImage.sprite = resources.standard;
            handleImage.type = Image.Type.Sliced;
            handleImage.color = s_DefaultSelectableColor;

            RectTransform sliderAreaRect = sliderArea.GetComponent<RectTransform>();
            sliderAreaRect.sizeDelta = new Vector2(-20, -20);
            sliderAreaRect.anchorMin = Vector2.zero;
            sliderAreaRect.anchorMax = Vector2.one;

            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 20);

            Scrollbar scrollbar = scrollbarRoot.GetComponent<Scrollbar>();
            scrollbar.handleRect = handleRect;
            scrollbar.targetGraphic = handleImage;
            SetDefaultColorTransitionValues(scrollbar);

            return scrollbarRoot;
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
        
        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;
            
            Undo.SetTransformParent(child.transform, parent.transform, "");
        }
        
        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);
        }
        
        private static GameObject CreateUIElementRoot(string name, Vector2 size, params Type[] components)
        {
            GameObject child = new GameObject(name, components);
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }
        
        private static GameObject CreateUIObject(string name, GameObject parent, params Type[] components)
        {
            GameObject go = new GameObject(name, components);
            go.transform.SetParent(parent.transform);
            return go;
        }
    }
}