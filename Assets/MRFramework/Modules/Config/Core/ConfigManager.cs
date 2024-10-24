using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/ConfigManager")]
    public class ConfigManager : MonoSingleton<ConfigManager>
    {
        private ConfigManager() { }

        // 缓存导表容器类，防止重复加载
        private readonly Dictionary<string, IDictionary> m_ConfigCache = new Dictionary<string, IDictionary>();

        /// <summary>
        /// 按行获取配置表
        /// </summary>
        /// <typeparam name="TData">数据结构类类名</typeparam>
        /// <param name="row">第几行</param>
        /// <param name="dontShowError">不输出报错</param>
        public TData GetConfigByRow<TData>(int row, bool dontShowError = false) where TData : class
        {
            var config = GetConfig<TData>(dontShowError);
            if (config != null)
            {
                return config[row];
            }
            return default;
        }

        /// <summary>
        /// 获取配置表
        /// </summary>
        /// <typeparam name="TData">数据结构类类名</typeparam>
        /// <param name="dontShowError">不输出报错</param>
        public IDictionary<int, TData> GetConfig<TData>(bool dontShowError = false) where TData : class
        {
            // 获取导表名称
            string configName = typeof(TData).Name;

            // 尝试从缓存字典里获取
            if (m_ConfigCache.TryGetValue(configName, out IDictionary configData))
            {
                return (IDictionary<int, TData>)configData;
            }

            IDictionary<int, TData> data = null;
            // 获取Json文件保存路径
            string jsonFilePath = GetJsonFilePath(configName);
            TextAsset textAsset = AssetManager.Instance.LoadAsset<TextAsset>(jsonFilePath);
            if (textAsset)
            {
                string jsonContent = textAsset.text;
                // 创建容器对象
                Type containerType = typeof(IDictionary<int, TData>);
                // 反序列化Json
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                var config = JsonConvert.DeserializeObject(jsonContent, containerType, settings);
                m_ConfigCache[configName] = config as IDictionary;
                data = (IDictionary<int, TData>)config;
            }
            else
            {
                if (!dontShowError) Log.Error($"没有找到导表文件: {jsonFilePath}");
            }

            AssetManager.Instance.ReleaseAsset<TextAsset>(jsonFilePath);
            return data;
        }

        private string GetJsonFilePath(string configName)
        {
            string rootPath = ConfigSettings.JsonLoadPath;
            // string name = configName.Replace("Config_", "");
            string[] strs = StringUtil.SplitStr(configName, 7);
            string foldName = strs[1];
            string filePath = Path.Combine(foldName, string.Format("{0}.json", configName));
            string jsonFilePath = Path.Combine(rootPath, filePath);
            string path = jsonFilePath.Replace("\\", "/");
            return path;
        }
    }
}
