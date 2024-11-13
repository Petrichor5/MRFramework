using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MRFramework
{
    public class ConfigFactory
    {
        /// <summary>
        /// 生成Excel表对应的数据容器类
        /// </summary>
        public static void GenerateExcelContainer(DataTable table, string jsonFileName, string outputPath)
        {
            outputPath.CreateDirIfNotExists();

            DataRow rowName = ConfigTool.GetVariableNameRow(table); // 字段名行
            DataRow rowType = ConfigTool.GetVariableTypeRow(table); // 字段类型行

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("/*******************************");
            sb.AppendLine(" * Desc: 以下代码为导表工具自动化生成，请不要手动更改。");
            sb.AppendLine(" * Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(" *******************************/ ");
            sb.AppendLine();
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            
            sb.AppendLine("namespace Config");
            sb.AppendLine("{");

            sb.AppendLine("\tpublic class " + jsonFileName);
            sb.AppendLine("\t{");
            // 变量进行字符串拼接，只处理非空白列
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string columnName = rowName[i].ToString().Trim();
                string columnType = rowType[i].ToString().Trim();

                if (!string.IsNullOrEmpty(columnName) && !string.IsNullOrEmpty(columnType))
                {
                    sb.AppendLine($"\t\tpublic {columnType} {columnName};");
                }
            }

            sb.AppendLine();
            sb.AppendLine("\t\tpublic override string ToString()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tStringBuilder sb = new StringBuilder();");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string columnName = rowName[i].ToString().Trim();
                if (!string.IsNullOrEmpty(columnName))
                {
                    sb.AppendLine($"\t\t\tsb.AppendLine(\"{columnName}: \" + {columnName}.ToString());");
                }
            }

            sb.AppendLine("\t\t\treturn sb.ToString();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            
            sb.AppendLine("}");

            string fullPath = Path.Combine(outputPath, jsonFileName + ".cs");
            File.WriteAllText(fullPath, sb.ToString());
        }

        /// <summary>
        /// 生成Excel表对应的JSON数据
        /// </summary>
        public static void GenerateExcelJson(DataTable table, string jsonFileName, string outputPath)
        {
            outputPath.CreateDirIfNotExists();

            // 创建一个Dictionary来存储表数据
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

            // 遍历所有内容的行
            DataRow row;
            // 得到类型行 根据类型来决定应该如何写入数据
            DataRow rowType = ConfigTool.GetVariableTypeRow(table);
            DataRow rowName = ConfigTool.GetVariableNameRow(table);

            for (int i = ConfigSettings.BeginIndex; i < table.Rows.Count; i++)
            {
                row = table.Rows[i];
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (row[j] is DBNull) continue;

                    string columnName = rowName[j].ToString();
                    string content = row[j].ToString();
                    switch (rowType[j].ToString())
                    {
                        case "int":
                            rowData[columnName] = int.Parse(content);
                            break;
                        case "float":
                            rowData[columnName] = float.Parse(content);
                            break;
                        case "bool":
                            rowData[columnName] = bool.Parse(content);
                            break;
                        case "string":
                            rowData[columnName] = content;
                            break;
                        case "int[]":
                            rowData[columnName] = new JArray(ConfigTool.GetList<int>(content, ','));
                            break;
                        case "float[]":
                            rowData[columnName] = new JArray(ConfigTool.GetList<float>(content, ','));
                            break;
                        case "string[]":
                            rowData[columnName] = new JArray(ConfigTool.GetList<string>(content, ','));
                            break;
                        case "List<int>":
                            rowData[columnName] = new JArray(ConfigTool.GetList<int>(content, ','));
                            break;
                        case "List<float>":
                            rowData[columnName] = new JArray(ConfigTool.GetList<float>(content, ','));
                            break;
                        case "List<string>":
                            rowData[columnName] = new JArray(ConfigTool.GetList<string>(content, ','));
                            break;
                        case "Vector2":
                            rowData[columnName] = ConfigTool.GetVectorValue(content, ',');
                            break;
                        case "Vector3":
                            rowData[columnName] = ConfigTool.GetVectorValue(content, ',');
                            break;
                        default:
                            if (!string.IsNullOrEmpty(columnName))
                                rowData[columnName] = content;
                            break;
                    }
                }

                if (rowData.Count > 0)
                {
                    var index = ConfigTool.GetKeyIndex(table);
                    dataDictionary.Add(row[index].ToString(), rowData);
                }
            }

            // 序列化字典为JSON字符串
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented, settings);

            string fullPath = Path.Combine(outputPath, jsonFileName + ".json");
            // 将JSON字符串写入文件
            File.WriteAllText(fullPath, json);
        }

        /// <summary>
        /// 生成Excel表对应的枚举数据
        /// </summary>
        public static void GenerateExcelEnum(DataTable table, string fileName, string outputPath)
        {
            outputPath.CreateDirIfNotExists();

            DataRow rowType = ConfigTool.GetEnumTypeRow(table); // 枚举字段类型行
            
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("/*******************************");
            sb.AppendLine(" * Desc: 以下代码为导表工具自动化生成，请不要手动更改。");
            sb.AppendLine(" * Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(" *******************************/ ");
            sb.AppendLine();
            
            sb.AppendLine("namespace Config");
            sb.AppendLine("{");
            
            sb.AppendLine("\tpublic enum " + fileName);
            sb.AppendLine("\t{");
            
            // 遍历所有内容的行
            DataRow row;
            for (int i = ConfigSettings.EnumBeginIndex; i < table.Rows.Count; i++)
            {
                row = table.Rows[i];
                string name = "";
                string value = "";
                string desc = "";
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (row[j] is DBNull) continue;
                    
                    switch (rowType[j].ToString())
                    {
                        case "Name":
                            name = row[j].ToString();
                            break;
                        case "Value":
                            value = row[j].ToString();
                            break;
                        case "Desc":
                            desc = row[j].ToString();
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    sb.AppendLine($"\t\t/// <summary>");
                    sb.AppendLine($"\t\t/// {desc}");
                    sb.AppendLine($"\t\t/// <summary>");
                    sb.AppendLine($"\t\t{name} = {value},");
                }
            }
            
            sb.AppendLine("\t}");
            
            sb.AppendLine("}");
            
            string fullPath = Path.Combine(outputPath, fileName + ".cs");
            File.WriteAllText(fullPath, sb.ToString());
        }
    }
}