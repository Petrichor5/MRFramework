using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;

public static class TextExtends
{
    public static void SetText(this Text widget, string sContent)
    {
        widget.text = sContent;
    }
    
    public static void SetTemplate(this Text widget, params string[] strParams)
    {
        // 正则表达式查找格式为 {text} 的所有占位符
        var regex = new Regex(@"\{(\w+)\}");
        var sContent = widget.text;
        var matches = regex.Matches(sContent);

        // 确保参数数量与占位符数量匹配
        if (matches.Count != strParams.Length) return;

        // 将每个占位符替换为相应的参数值
        for (int i = 0; i < matches.Count; i++)
        {
            sContent = sContent.Replace(matches[i].Value, strParams[i]);
        }
        
        widget.text = sContent;
    }
    
    public static void SetText(this TextMeshProUGUI widget, string sContent)
    {
        widget.text = sContent;
    }
    
    public static void SetTemplate(this TextMeshProUGUI widget, params string[] strParams)
    {
        var regex = new Regex(@"\{(\w+)\}");
        var sContent = widget.text;
        var matches = regex.Matches(sContent);

        if (matches.Count != strParams.Length) return;

        for (int i = 0; i < matches.Count; i++)
        {
            sContent = sContent.Replace(matches[i].Value, strParams[i]);
        }
        
        widget.text = sContent;
    }
}
