using Config;

public class ReddotUtil
{
    public static string MKReddotKey(EReddot eReddot, string content = null)
    {
        string key;
        if (!string.IsNullOrEmpty(content))
            key = string.Format(eReddot.ToString(), "_", content);
        else
            key = eReddot.ToString();
        return key;
    }
}
