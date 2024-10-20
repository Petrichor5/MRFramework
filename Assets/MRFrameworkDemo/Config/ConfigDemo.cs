using Config;
using MRFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
