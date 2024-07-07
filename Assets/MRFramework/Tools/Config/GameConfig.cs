using System.Collections;
using System.Collections.Generic;
using System.IO;
using MRFramework;
using Newtonsoft.Json;
using UnityEngine;

public static class GameConfig
{
    private static readonly Dictionary<string, IDictionary> mConfigCache = new Dictionary<string, IDictionary>();

    /// <summary>
    /// 按行获取导表数据
    /// </summary>
    /// <param name="key">行（主键行）</param>
    /// <param name="dontShowError">不显示报错</param>
    /// <typeparam name="TTable">导表</typeparam>
    /// <typeparam name="TKey">导表主键</typeparam>
    /// <returns></returns>
    public static TTable GetConfigRow<TTable, TKey>(TKey key, bool dontShowError = false)
    {
        var config = GetConfig<TTable, TKey>(dontShowError);

        if (config != null && config.TryGetValue(key, out TTable rowData))
        {
            return rowData;
        }

        if (!dontShowError)
        {
            var configName = typeof(TTable).ToString();
            Debug.LogError($"Row with key {key} not found in config {configName}");
        }

        return default;
    }
    
    /// <summary>
    /// 获取导表数据
    /// </summary>
    /// <param name="dontShowError">不显示报错</param>
    /// <typeparam name="TTable">导表</typeparam>
    /// <typeparam name="TKey">导表主键</typeparam>
    /// <returns></returns>
    public static Dictionary<TKey, TTable> GetConfig<TTable, TKey>(bool dontShowError = false)
    {
        var configName = typeof(TTable).ToString();
        var config = GetConfigByName<Dictionary<TKey, TTable>>(configName, dontShowError);

        if (config != null)
        {
            return config;
        }

        if (!dontShowError) Debug.LogError($"Config {configName} is not of type Dictionary<int, {typeof(TTable)}>");
  
        return null;
    }
    
    private static T GetConfigByName<T>(string configName, bool dontShowError = false)
    {
        if (mConfigCache.TryGetValue(configName, out IDictionary configData))
        {
            return (T)configData;
        }

        string jsonFilePath = Path.Combine(ConfigSettings.DATA_JSON_PATH, configName + ".json");
        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            var configType = ConfigRegistry.GetConfigType(configName);

            if (configType != null)
            {
                var config = JsonConvert.DeserializeObject(jsonContent, configType);
                mConfigCache[configName] = config as IDictionary;
                return (T)config;
            }
            else
            {
                if (!dontShowError) Debug.LogError($"Config type not found for {configName}");
            }
        }
        else
        {
            if (!dontShowError) Debug.LogError($"Config file not found for {configName}");
        }

        return default;
    }
}