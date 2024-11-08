using Config;
using MRFramework;
using UnityEngine;

public class Test
{
    public Vector3 v3 = new Vector3(1, 2, 3);
}

public class Test2
{
    public string Name = "名称";
    public int Age = 18;

    public Test2()
    {

    }

    public Test2(int age)
    {
        Age = age;
    }
}

public class ConfigDemo : MonoBehaviour
{
    private void Start()
    {
        // 按行获取配置表
        int row = 1;
        var config = ConfigManager.Instance.GetConfigByRow<Config_Test_TestConfig>(row);

        // 目前支持的类型：
        int configID = config.Id;
        string name = config.Name;
        int storageCount = config.StorageCount;
        string path = config.PrefabPath;
        bool isCommonOre = config.IsCommonOre;
        int[] ints = config.IntArrayTest;
        float[] floats = config.FloatArrayTest;
        string[] strings = config.StringArrayTest;
        Vector2 buildPos1 = config.BuildPos1;
        Vector3 buildPos2 = config.BuildPos2;


        // 内置一键打印
        Debug.Log(config.ToString());


        // 获取整个配置表
        var configTable = ConfigManager.Instance.GetConfig<Config_Test_TestConfig>();
        foreach (var item in configTable.Values)
        {
            Debug.Log(item.ToString());
        }
    }
}
