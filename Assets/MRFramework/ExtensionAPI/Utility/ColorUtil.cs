using UnityEngine;

public class ColorUtil
{
    /// <summary>
    /// 将十六进制颜色字符串转换为 Color 对象
    /// 例如：#3BCBFF
    /// </summary>
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", ""); // 替换 0x 前缀
        hex = hex.Replace("#", ""); // 替换 # 前缀

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, 255);
    }
}
