using Config;
using MRFramework;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        int row = 1;
        var config = ConfigManager.Instance.GetConfigByRow<Config_Test_TestConfig>(row);
        
        int configID = config.Id;
        string name = config.Name;
        int storageCount = config.StorageCount;
        string path = config.PrefabPath;
        bool isCommonOre = config.IsCommonOre;

        Debug.Log(config.ToString());




        var configTable = ConfigManager.Instance.GetConfig<Config_Test_TestConfig>();
        foreach (var item in configTable.Values)
        {
            Debug.Log(item.ToString());
        }


        // Vector3
        Vector3 a = Vector3.one;
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        //Debug.Log(JsonConvert.SerializeObject(a, settings));

        string json = @"{""x"":1.0,""y"":1.0,""z"":1.0}";
        a = JsonConvert.DeserializeObject<Vector3>(json);
        Debug.Log(a);


        // 数组
        int[] arr1 = { 1, 2, };
        Debug.Log("Arr1" + " , " + JsonConvert.SerializeObject(arr1));

        string jsonArray1 = @"[1,2]";
        var array1 = JsonConvert.DeserializeObject<int[]>(jsonArray1);
        foreach (var item in array1)
        {
            Debug.Log(item);
        }


        string[] arr2 = { "你", "好", };
        Debug.Log("Arr2" + " , " + JsonConvert.SerializeObject(arr2));

        string jsonArray2 = @"[""你"",""好""]";
        var array2 = JsonConvert.DeserializeObject<int[]>(jsonArray2);
        foreach (var item in array2)
        {
            Debug.Log(item);
        }


        Test2[] arr3 = { new Test2(), new Test2(20), };
        Debug.Log("Arr3" + " , " + JsonConvert.SerializeObject(arr3));

        string jsonArray3 = @"[{""Name"":""名称"",""Age"":18},{""Name"":""名称"",""Age"":20}]";
        var array3 = JsonConvert.DeserializeObject<Test2[]>(jsonArray3);
        foreach (var item in array3)
        {
            Debug.Log(item.Age + " " + item.Name);
        }
    }
}
