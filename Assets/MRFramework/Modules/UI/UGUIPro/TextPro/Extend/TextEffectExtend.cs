using UnityEngine;

namespace MRFramework.UGUIPro
{
    [System.Serializable]
    public class TextEffectExtend
    {
        [SerializeField] private bool m_UseTextEffect;

        public bool UseTextEffect
        {
            get { return m_UseTextEffect; }
            set { m_UseTextEffect = value; }
        }

        public bool UseOutLine
        {
            get { return m_EnableOutLine; }
        }

        [SerializeField] private float m_LerpValue = 0;
        [SerializeField] private bool m_OpenShaderOutLine = true;
        [SerializeField] private TextProOutLine m_OutlineEx;
        [SerializeField] private bool m_EnableOutLine = false;
        [SerializeField] private float m_OutLineWidth = 1;
        [SerializeField] private GradientType m_GradientType = GradientType.OneColor;
        [SerializeField] private Color32 m_TopColor = Color.white;
        [SerializeField] private Color32 m_MiddleColor = Color.white;
        [SerializeField] private Color32 m_BottomColor = Color.white;
        [SerializeField] private Color32 m_OutLineColor = Color.yellow;
        [SerializeField] private Camera m_Camera;

        [SerializeField, Range(0, 1)]
        private float m_Alpha = 1;

        [Range(0.1f, 0.9f)] [SerializeField]
        private float m_ColorOffset = 0.5f;

        [SerializeField] public TextEffect m_TextEffect;

        public GradientType GradientType => m_GradientType;
        public float LerpValue => m_LerpValue;
        public bool OpenShaderOutLine => m_OpenShaderOutLine;
        public TextProOutLine OutlineEx => m_OutlineEx;
        public float OutLineWidth => m_OutLineWidth;
        public Color32 TopColor => m_TopColor;
        public Color32 MiddleColor => m_MiddleColor;
        public Color32 BottomColor => m_BottomColor;
        public Color32 OutLineColor => m_OutLineColor;
        public float Alpha => m_Alpha;
        public float ColorOffset => m_ColorOffset;
        
        public void SaveSerializeData(TextPro TextPro)
        {
            m_TextEffect = TextPro.GetComponent<TextEffect>();
            if (m_TextEffect == null)
            {
                int insid = TextPro.GetInstanceID();

                TextPro[] textProAry = Transform.FindObjectsOfType<TextPro>();
                for (int i = 0; i < textProAry.Length; i++)
                {
                    if (textProAry[i].GetInstanceID() == insid)
                    {
                        m_TextEffect = textProAry[i].gameObject.AddComponent<TextEffect>();
                        m_TextEffect.hideFlags = HideFlags.HideInInspector;
                        break;
                    }
                }
            }

            if (m_Camera == null)
            {
                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                if (mainCamera != null)
                {
                    m_Camera = mainCamera.GetComponent<Camera>();
                }
                else
                {
                    m_Camera = Transform.FindObjectOfType<Camera>();
                }
            }

            if (m_Camera == null)
            {
                Debug.LogError("not find Main Camera in Scenes");
            }
        }

        public void SetAlpha(float alpha)
        {
            m_TextEffect.SetAlpah(alpha);
        }
    }
}