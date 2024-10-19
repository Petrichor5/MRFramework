using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MRFramework.UGUIPro
{
    [Serializable]
    public class LocalizationData
    {
        //[JsonConverter(typeof(StringEnumConverter))]
        //public LanguageType languageType;
        public string Key;
        public string value;
    }

    public class LocalizationDataConfig
    {
        /// <summary>
        /// 是否异步加载中
        /// </summary>
        private bool IsConfigLoading = false;

        /// <summary>
        /// 加载对应语言配置
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public async Task<List<LocalizationData>> LoadConfig(LanguageType languageType)
        {
            if (IsConfigLoading)
                return null;
            IsConfigLoading = true;

            string[] languageNames = Enum.GetNames(typeof(LanguageType));
            string fileName = languageNames[(int)languageType];
            string configPath = LocalizationSettings.ResPath + fileName + ".txt";
            TextAsset textAsset = LoadTextAsset(configPath);
            if (textAsset != null && textAsset.text != null)
            {
                string json = textAsset.text;
                List<LocalizationData> localizationDatalist = null;
                await Task.Run(() =>
                {
                    localizationDatalist = JsonConvert.DeserializeObject<List<LocalizationData>>(json);
                });
                IsConfigLoading = false;
                return localizationDatalist;
            }

            IsConfigLoading = false;
            return null;
        }

        /// <summary>
        /// 加载对应语言配置，通过Editor模式
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public List<LocalizationData> LoadConfigEditor(LanguageType languageType)
        {
            if (IsConfigLoading)
                return null;

            IsConfigLoading = true;
            string[] languageNames = Enum.GetNames(typeof(LanguageType));
            string fileName = languageNames[(int)languageType];
            string configPath = LocalizationSettings.ResPath + fileName + ".txt";
            TextAsset textAsset = LoadTextAsset(configPath);
            if (textAsset != null && textAsset.text != null)
            {
                string json = textAsset.text;
                List<LocalizationData> localizationDatalist;
                localizationDatalist = JsonConvert.DeserializeObject<List<LocalizationData>>(json);
                IsConfigLoading = false;
                return localizationDatalist;
            }

            IsConfigLoading = false;
            return null;
        }

        private TextAsset LoadTextAsset(string path)
        {
            AsyncOperationHandle<TextAsset> aoh = Addressables.LoadAssetAsync<TextAsset>(path);
            aoh.WaitForCompletion();
            TextAsset textAsset = aoh.Result;
            Addressables.Release(aoh);
            return textAsset;
        }
    }
}