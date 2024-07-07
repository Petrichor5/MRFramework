using Newtonsoft.Json;

/// <summary>
/// ProtoBuff 转为 Json 字符串，并进行打印
/// </summary>
public class ProtoBuffConvert
{
    public static void ToJson<T>(T proto)
    {
        Log.Info(JsonConvert.SerializeObject(proto));
    }
}
