using MRFramework.UGUIPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public enum ELocaleType
{
    Chinese = 0,
    English = 1,
}

public class LocalizationDemo : MonoBehaviour
{
    public LocalizedString LocalString;
    public TextMeshPro Text_Score;
    private int score;
    public ButtonPro Button_AddScore;

    private bool m_Active = false;
    public ButtonPro Button_Chinese;
    public ButtonPro Button_English;

    private void Start()
    {
        Button_Chinese.AddButtonClick(() =>
        {
            if (m_Active) return;
            StartCoroutine(SetLocale(ELocaleType.Chinese));
        });

        Button_English.AddButtonClick(() =>
        {
            if (m_Active) return;
            StartCoroutine(SetLocale(ELocaleType.English));
        });

        Button_AddScore.AddButtonClick(() =>
        {
            score++;
            LocalString.Arguments[0] = score;
            LocalString.RefreshString();
        });

        SmartString();
    }

    private void SmartString()
    {
        LocalString.Arguments = new object[] { score };
        LocalString.StringChanged += UpdateText;
    }

    private void UpdateText(string value)
    {
        Text_Score.text = value;
    }

    private IEnumerator SetLocale(ELocaleType type)
    {
        m_Active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)type];
        m_Active = false;
    }
}
