using System.Text;

public static class TimeUtil
{
    private static StringBuilder m_ResultStr = new StringBuilder();

    /// <summary>
    /// 秒转时分秒格式 其中时分秒可以自己传
    /// </summary>
    /// <param name="s">秒数</param>
    /// <param name="egZero">是否忽略0</param>
    /// <param name="isKeepLen">是否保留至少2位</param>
    /// <param name="hourStr">小时的拼接字符</param>
    /// <param name="minuteStr">分钟的拼接字符</param>
    /// <param name="secondStr">秒的拼接字符</param>
    /// <returns></returns>
    public static string SecondToHMS(int s, bool egZero = false, bool isKeepLen = false, string hourStr = "时",
        string minuteStr = "分", string secondStr = "秒")
    {
        //时间不会有负数 所以我们如果发现是负数直接归0
        if (s < 0)
            s = 0;
        //计算小时
        int hour = s / 3600;
        //计算分钟
        //除去小时后的剩余秒
        int second = s % 3600;
        //剩余秒转为分钟数
        int minute = second / 60;
        //计算秒
        second = s % 60;
        //拼接
        m_ResultStr.Clear();
        //如果小时不为0 或者 不忽略0 
        if (hour != 0 || !egZero)
        {
            m_ResultStr.Append(isKeepLen ? StringUtil.GetNumStr(hour, 2) : hour); //具体几个小时
            m_ResultStr.Append(hourStr);
        }

        //如果分钟不为0 或者 不忽略0 或者 小时不为0
        if (minute != 0 || !egZero || hour != 0)
        {
            m_ResultStr.Append(isKeepLen ? StringUtil.GetNumStr(minute, 2) : minute); //具体几分钟
            m_ResultStr.Append(minuteStr);
        }

        //如果秒不为0 或者 不忽略0 或者 小时和分钟不为0
        if (second != 0 || !egZero || hour != 0 || minute != 0)
        {
            m_ResultStr.Append(isKeepLen ? StringUtil.GetNumStr(second, 2) : second); //具体多少秒
            m_ResultStr.Append(secondStr);
        }

        //如果传入的参数是0秒时
        if (m_ResultStr.Length == 0)
        {
            m_ResultStr.Append(0);
            m_ResultStr.Append(secondStr);
        }

        return m_ResultStr.ToString();
    }

    /// <summary>
    /// 秒转00:00:00格式
    /// </summary>
    /// <param name="s"></param>
    /// <param name="egZero"></param>
    /// <returns></returns>
    public static string SecondToHMS2(int s, bool egZero = false)
    {
        return SecondToHMS(s, egZero, true, ":", ":", "");
    }
}