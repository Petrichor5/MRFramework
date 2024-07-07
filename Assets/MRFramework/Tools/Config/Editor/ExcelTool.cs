using System;
using Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using MRFramework;
using Newtonsoft.Json;

public class ExcelTool
{
    [MenuItem("MRFramework/GenerateExcel 导表生成工具")]
    private static void GenerateExcelInfo()
    {
        ConfigSettings.EXCEL_PATH.CreateDirIfNotExists();
        
        // 记在指定路径中的所有Excel文件 用于生成对应的3个文件
        DirectoryInfo dInfo = new DirectoryInfo(ConfigSettings.EXCEL_PATH);
        
        // 得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
        FileInfo[] files = dInfo.GetFiles("*.*", SearchOption.AllDirectories);
        
        // 数据表容器
        DataTableCollection tableCollection;
        
        Dictionary<string, object> registryDic = new Dictionary<string, object>();
        
        for (int i = 0; i < files.Length; i++)
        {
            // 如果不是excel文件就不要处理了
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;
            // 打开一个Excel文件得到其中的所有表的数据 ( 如果报错！可能是因为没有关闭Excel表，请关闭所有打开的Excel表! )
            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tableCollection = excelReader.AsDataSet().Tables;
                fs.Close();
            }

            // 遍历文件中的所有表的信息
            foreach (DataTable table in tableCollection)
            {
                int keyIndex = GetKeyIndex(table); // 得到主键索引
                DataRow rowType = GetVariableTypeRow(table); // 字段类型行
                
                // 生成JSON文件名，包含路径前缀（如果有的话）
                string jsonFileName = GetJsonFileName(files[i].FullName, table.TableName);
                registryDic.Add(jsonFileName, rowType[keyIndex]);
                
                // 生成容器类
                GenerateExcelContainer(table, jsonFileName);
                // 生成JSON数据
                GenerateExcelJson(table, jsonFileName);
            }
        }
        
        // 生成映射关系类
        GenerateConfigRegistry(registryDic);
    }

    /// <summary>
    /// 生成Excel表对应的数据容器类
    /// </summary>
    private static void GenerateExcelContainer(DataTable table, string jsonFileName)
    {
        ConfigSettings.DATA_CONTAINER_PATH.CreateDirIfNotExists();
        ConfigSettings.DATA_CONTAINER_PATH.EmptyDirIfExists();
        
        int keyIndex = GetKeyIndex(table); // 得到主键索引
        DataRow rowName = GetVariableNameRow(table); // 字段名行
        DataRow rowType = GetVariableTypeRow(table); // 字段类型行

        StringBuilder sb = new StringBuilder();
        
        sb.AppendLine("/*******************************");
        sb.AppendLine(" * Desc: 以下代码为导表工具自动化生成，请不要手动更改。");
        sb.AppendLine(" * Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.AppendLine(" *******************************/ ");
        sb.AppendLine();
        
        sb.AppendLine("using System.Text;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine("namespace MRFramework.Config"); // 包裹在框架命名空间里，防止被外部无意中调用
        sb.AppendLine("{");
        sb.AppendLine("\tpublic class " + jsonFileName + "_Container");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tpublic Dictionary<{rowType[keyIndex]}, {jsonFileName}> DataDic = " +
                      $"new Dictionary<{rowType[keyIndex]}, {jsonFileName}>();");
        sb.AppendLine();
        sb.AppendLine("\t\tpublic override string ToString()");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\tStringBuilder sb = new StringBuilder();");
        sb.AppendLine($"\t\t\tsb.AppendLine(\"{jsonFileName}: \");");
        sb.AppendLine("\t\t\tforeach (var item in DataDic)");
        sb.AppendLine("\t\t\t{");
        sb.AppendLine("\t\t\t\tsb.AppendLine(item.Key.ToString() + \": \" + item.Value.ToString());");
        sb.AppendLine("\t\t\t}");
        sb.AppendLine("\t\t\treturn sb.ToString();");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        sb.AppendLine("}");
        sb.AppendLine();
        
        sb.AppendLine("public class " + jsonFileName);
        sb.AppendLine("{");
        // 变量进行字符串拼接，只处理非空白列
        for (int i = 0; i < table.Columns.Count; i++)
        {
            string columnName = rowName[i].ToString().Trim();
            string columnType = rowType[i].ToString().Trim();

            if (!string.IsNullOrEmpty(columnName) && !string.IsNullOrEmpty(columnType))
            {
                sb.AppendLine($"\tpublic {columnType} {columnName};");
            }
        }
        sb.AppendLine();
        sb.AppendLine("\tpublic override string ToString()");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tStringBuilder sb = new StringBuilder();");
        for (int i = 0; i < table.Columns.Count; i++)
        {
            string columnName = rowName[i].ToString().Trim();
            if (!string.IsNullOrEmpty(columnName))
            {
                sb.AppendLine($"\t\tsb.AppendLine(\"{columnName}: \" + {columnName}.ToString());");
            }
        }
        sb.AppendLine("\t\treturn sb.ToString();");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        File.WriteAllText(ConfigSettings.DATA_CONTAINER_PATH + jsonFileName + ".cs", sb.ToString());
        
        AssetDatabase.Refresh(); // 刷新Project窗口
    }

    /// <summary>
    /// 生成Excel表对应的JSON数据
    /// </summary>
    private static void GenerateExcelJson(DataTable table, string jsonFileName)
    {
        ConfigSettings.DATA_JSON_PATH.CreateDirIfNotExists();
        ConfigSettings.DATA_JSON_PATH.EmptyDirIfExists();

        // 创建一个Dictionary来存储表数据
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        // 遍历所有内容的行
        DataRow row;
        // 得到类型行 根据类型来决定应该如何写入数据
        DataRow rowType = GetVariableTypeRow(table);
        DataRow rowName = GetVariableNameRow(table);

        for (int i = ConfigSettings.BEGIN_INDEX; i < table.Rows.Count; i++)
        {
            row = table.Rows[i];
            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < table.Columns.Count; j++)
            {
                string columnName = rowName[j].ToString();
                switch (rowType[j].ToString())
                {
                    case "int":
                        rowData[columnName] = int.Parse(row[j].ToString());
                        break;
                    case "float":
                        rowData[columnName] = float.Parse(row[j].ToString());
                        break;
                    case "bool":
                        rowData[columnName] = bool.Parse(row[j].ToString());
                        break;
                    case "string":
                        rowData[columnName] = row[j].ToString();
                        break;
                }
            }
            dataDictionary.Add(row[GetKeyIndex(table)].ToString(), rowData);
        }

        // 序列化字典为JSON字符串
        string json = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented);

        // 将JSON字符串写入文件
        File.WriteAllText(Path.Combine(ConfigSettings.DATA_JSON_PATH, jsonFileName + ".json"), json);

        // 刷新Project窗口
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// 生成配置映射关系类
    /// </summary>
    private static void GenerateConfigRegistry(Dictionary<string, object> registryDic)
    {
        StringBuilder sb = new StringBuilder();
        
        sb.AppendLine("/*******************************");
        sb.AppendLine(" * Desc: 以下代码为导表工具自动化生成，请不要手动更改。");
        sb.AppendLine(" * Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.AppendLine(" *******************************/ ");
        sb.AppendLine();
        
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine("public static class ConfigRegistry");
        sb.AppendLine("{");
        sb.AppendLine("\tprivate static readonly Dictionary<string, Type> mConfigTypeMap = new Dictionary<string, Type>();");
        sb.AppendLine();
        sb.AppendLine("\tpublic static Type GetConfigType(string configName)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tif (mConfigTypeMap.TryGetValue(configName, out var type))");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\treturn type;");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t\tthrow new ArgumentException($\"Config type not found for {{configName}}\", nameof(configName));");
        sb.AppendLine("\t}");
        sb.AppendLine();
        sb.AppendLine("\tprivate static void RegisterConfig(string configName, Type type)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tmConfigTypeMap.TryAdd(configName, type);");
        sb.AppendLine("\t}");
        sb.AppendLine();
        
        sb.AppendLine("\tstatic ConfigRegistry()");
        sb.AppendLine("\t{");
        
        foreach (var item in registryDic)
        {
            sb.AppendLine($"\t\tRegisterConfig(\"{item.Key}\", typeof(Dictionary<{item.Value}, {item.Key}>));");
        }
        
        sb.AppendLine("\t}");
        
        sb.AppendLine("}");

        File.WriteAllText(ConfigSettings.DATA_CONFIGREGISTRY_PATH + "ConfigRegistry.cs", sb.ToString());

        AssetDatabase.Refresh(); // 刷新Project窗口
    }

    /// <summary>
    /// 获取Josn文件名称
    /// </summary>
    /// <param name="excelFilePath">Excel文件路径</param>
    /// <param name="tableName">表名称</param>
    /// <returns></returns>
    private static string GetJsonFileName(string excelFilePath, string tableName)
    {
        // 生成JSON文件名，包含路径前缀（如果有的话）
        string jsonFileName;
        string relativePath = Path.GetDirectoryName(excelFilePath);

        if (relativePath == ConfigSettings.EXCEL_PATH)
        {
            // 文件在根目录时，使用表名作为文件名
            jsonFileName = $"{tableName}";
        }
        else
        {
            // 文件在子目录时，使用相对路径作为前缀
            relativePath = relativePath?.Substring(ConfigSettings.EXCEL_PATH.Length + 1).Replace(Path.DirectorySeparatorChar, '_').Replace(Path.AltDirectorySeparatorChar, '_');
            jsonFileName = $"{relativePath}_{tableName}";
        }

        return jsonFileName;
    }

    /// <summary>
    /// 获取变量名所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[0];
    }

    /// <summary>
    /// 获取变量类型所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[1];
    }

    /// <summary>
    /// 获取主键索引
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static int GetKeyIndex(DataTable table)
    {
        DataRow row = table.Rows[2];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
                return i;
        }
        return 0;
    }
}
