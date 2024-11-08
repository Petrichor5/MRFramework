using UnityEngine;
using MRFramework.UGUIPro;

public class LocalizationDemo : MonoBehaviour
{
    async void Start()
    {
        //执行可等待异步本地多语言配置加载
        await LocalizationManager.Instance.InitLanguageConfig();
    }

    public async void OnChineseButtonClick()
    {
       await LocalizationManager.Instance.SwitchLanguage(LanguageType.Chinese);
    }

    public async void OnEnglishButtonClick()
    {
        await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);
    }
}
