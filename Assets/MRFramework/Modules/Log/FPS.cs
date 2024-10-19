using UnityEngine;

[MonoSingletonPath("MRFramework/Log/FPS")]
public class FPS : MonoSingleton<FPS>
{
    private float m_DeltaTime = 0.0f;

    private GUIStyle m_Style;

    void Awake()
    {
        m_Style = new GUIStyle();
        m_Style.alignment = TextAnchor.UpperLeft;
        m_Style.normal.background = null;
        m_Style.fontSize = 35; // 字体大小可以根据屏幕比例动态调整
        m_Style.normal.textColor = Color.red;
    }

    void Update()
    {
        m_DeltaTime += (Time.deltaTime - m_DeltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // 适应屏幕的字体大小，可以根据屏幕高度调整
        m_Style.fontSize = (int)(Screen.height * 0.04f); // 根据屏幕高度的4%调整字体大小

        // 适应屏幕的FPS显示框
        float width = Screen.width * 0.2f;  // 使用屏幕宽度的20%作为FPS显示框的宽度
        float height = Screen.height * 0.05f;  // 使用屏幕高度的5%作为FPS显示框的高度
        Rect rect = new Rect(10, 10, width, height);

        // 计算FPS
        float fps = 1.0f / m_DeltaTime;
        string text = string.Format(" FPS: {0:N0} ", fps);
        GUI.Label(rect, text, m_Style);
    }
}