using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public enum SaveType
{
    PersistentData,
    StreamingAssetsPath,
}

public static class SaveUtil
{
    #region Json

    /// <summary>
    /// 序列化：存储 Json 数据
    /// </summary>
    /// <param name="data">存储对象</param>
    /// <param name="dir">目录</param>
    /// <param name="fileName">Json 文件名</param>
    /// <param name="saveType">存储类型</param>
    public static void SaveData2Json(object data, string dir, string fileName, SaveType saveType)
    {
        string folderPath = GetSavePath(dir, saveType);
        folderPath.CreateDirIfNotExists();

        string path = Path.Combine(folderPath, fileName + ".json");
        string jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// 反序列化：读取 Json 文件
    /// </summary>
    /// <param name="dir">目录</param>
    /// <param name="fileName">Json 文件名</param>
    /// <param name="saveType">存储类型</param>
    public static T LoadDataFromJson<T>(string dir, string fileName, SaveType saveType) where T : new()
    {
        string folderPath = GetSavePath(dir, saveType);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("不存在该路径, Path：" + folderPath);
            return default;
        }

        string path = Path.Combine(folderPath, fileName + ".json");
        if (!File.Exists(path))
        {
            Debug.LogError("没有找到 Json 文件, Path：" + path);
            return default;
        }

        string jsonStr = File.ReadAllText(path);
        T data = JsonConvert.DeserializeObject<T>(jsonStr);
        return data;
    }

    private static string GetSavePath(string dir, SaveType saveType)
    {
        // PersistentDataPath 游戏打包后该文件夹在任何平台都可读可写
        string rootPath = string.Empty;
        switch (saveType)
        {
            case SaveType.PersistentData:
                rootPath = Application.persistentDataPath;
                break;
            case SaveType.StreamingAssetsPath:
                rootPath = Application.streamingAssetsPath;
                break;
        }
        return Path.Combine(rootPath, dir);
    }

    #endregion

    #region PlayerPrefs

    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">数据对象</param>
    /// <param name="keyName">数据对象的唯一 Key</param>
    public static void SavePlayerPrefsData(object data, string keyName)
    {
        Type dataType = data.GetType();
        // 得到所有的字段
        FieldInfo[] fieldInfos = dataType.GetFields();

        // 自定义一个 key 存储的规则：keyName_数据类型名_字段类型名_字段名
        string saveKeyName;
        FieldInfo fieldInfo;
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            fieldInfo = fieldInfos[i];
            saveKeyName = string.Format($"{keyName}_{dataType.Name}_{fieldInfo.FieldType.Name}_{fieldInfo.Name}");
            SaveValue(fieldInfo.GetValue(data), saveKeyName);
        }

        PlayerPrefs.Save();
    }

    // 存储单个数据
    private static void SaveValue(object value, string keyName)
    {
        Type fieldType = value.GetType();

        if (fieldType == typeof(int))
        {
            PlayerPrefs.SetInt(keyName, (int)value);
        }

        else if (fieldType == typeof(float))
        {
            PlayerPrefs.SetFloat(keyName, (float)value);
        }

        else if (fieldType == typeof(string))
        {
            PlayerPrefs.SetString(keyName, value.ToString());
        }

        else if (fieldType == typeof(bool))
        {
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
        }

        // List 类型
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            IList list = value as IList;
            // 存储 List 长度
            if (list != null)
            {
                PlayerPrefs.SetInt(keyName, list.Count);
                // 存储 list 数据，定义一个 index 确保 keyName 唯一，不被覆盖
                int index = 0;
                foreach (object obj in list)
                {
                    // 递归：通过上面的 if 判断 obj 的数据类型
                    SaveValue(obj, keyName + index);
                    index++;
                }
            }
        }

        // Dictionary 类型
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            IDictionary dictionary = value as IDictionary;
            // 存储字典长度
            if (dictionary != null)
            {
                PlayerPrefs.SetInt(keyName, dictionary.Count);
                // 存储字典数据，定义一个 index 确保 keyName 唯一，不被覆盖
                int index = 0;
                foreach (object key in dictionary.Keys)
                {
                    // 递归：通过上面的 if 判断 obj 的数据类型
                    SaveValue(key, keyName + "_key_" + index);
                    SaveValue(dictionary[key], keyName + "_value_" + index);
                    index++;
                }
            }
        }

        // 自定义类型
        else
        {
            // 不用递归
            // 把这个自定义类型又反回去 SaveData
            // 然后 SaveData 会获取自定义类型的字段 生成keyName
            // 然后调用 SaveValue 通过上面的if判断 存储自定义类型的字段
            SavePlayerPrefsData(value, keyName);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="dataType">读取的数据类型Type</param>
    /// <param name="keyName">数据对象的唯一 Key</param>
    public static object LoadPlayerPrefsData(Type dataType, string keyName)
    {
        object data = Activator.CreateInstance(dataType);
        FieldInfo[] fieldInfos = dataType.GetFields();

        // 自定义一个 key 存储的规则：keyName_数据类型名_字段类型名_字段名
        string loadKeyName;
        FieldInfo fieldInfo;
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            fieldInfo = fieldInfos[i];
            loadKeyName = string.Format($"{keyName}_{dataType.Name}_{fieldInfo.FieldType.Name}_{fieldInfo.Name}");
            fieldInfo.SetValue(data, LoadValue(fieldInfo.FieldType, loadKeyName));
        }

        return data;
    }

    // 读取单个数据
    private static object LoadValue(Type fieldType, string keyName)
    {
        if (fieldType == typeof(int))
        {
            return PlayerPrefs.GetInt(keyName, 0);
        }

        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName, 0);
        }

        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyName, "");
        }

        else if (fieldType == typeof(bool))
        {
            return PlayerPrefs.GetInt(keyName, 0) == 1;
        }

        // List 类型
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            Type[] types = fieldType.GetGenericArguments();
            // 得到长度
            int count = PlayerPrefs.GetInt(keyName);
            // 实例化 Lisy 对象，进行赋值，返回出去
            IList list = Activator.CreateInstance(fieldType) as IList;
            for (int i = 0; i < count; i++)
            {
                if (list != null) list.Add(LoadValue(types[0], keyName + i));
            }

            return list;
        }

        // Dictionary 类型
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            Type[] types = fieldType.GetGenericArguments();
            // 得到长度
            int count = PlayerPrefs.GetInt(keyName);
            // 实例化 Dictionary 对象
            IDictionary dictionary = Activator.CreateInstance(fieldType) as IDictionary;
            object key;
            object value;
            for (int i = 0; i < count; i++)
            {
                key = LoadValue(types[0], keyName + "_key_" + i);
                value = LoadValue(types[1], keyName + "_value_" + i);
                if (dictionary != null) dictionary.Add(key, value);
            }

            return dictionary;
        }

        // 自定义类型
        else
        {
            // SaveValue 同理 
            return LoadPlayerPrefsData(fieldType, keyName);
        }
    }

    #endregion
}
