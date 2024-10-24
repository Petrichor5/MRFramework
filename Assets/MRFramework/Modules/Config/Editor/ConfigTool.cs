using Excel;
using System.Data;
using System.IO;
using UnityEditor;
using MRFramework;
using UnityEngine;

public class ConfigTool
{
    [MenuItem("MRFramework/GenerateExcel 导表生成工具")]
    private static void GenerateExcelInfo()
    {
        GenerateEnumConfig();
        GenerateGlobalConfig();

        AssetDatabase.Refresh(); // 刷新Project窗口
    }

    /// <summary>
    /// 生成枚举导表数据
    /// </summary>
    private static void GenerateEnumConfig()
    {
        ConfigSettings.EnumExcelPath.CreateDirIfNotExists();
        DirectoryInfo dInfo = new DirectoryInfo(ConfigSettings.EnumExcelPath);
        FileInfo[] files = dInfo.GetFiles("*.*", SearchOption.AllDirectories);
        DataTableCollection tableCollection = null;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" && files[i].Extension != ".xls")
                continue;
            Debug.Log(files[i].Name);
            GenerateEnum(tableCollection, files[i]);
        }
    }
    
    /// <summary>
    /// 生成枚举数据
    /// </summary>
    private static void GenerateEnum(DataTableCollection tableCollection, FileInfo file)
    {
        using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            tableCollection = excelReader.AsDataSet().Tables;
            fs.Close();
        }
        
        foreach (DataTable table in tableCollection)
        {
            // 生成枚举文件名，包含路径前缀（如果有的话）
            string jsonFileName = ConfigUtil.GetEnumFileName(file.FullName, table.TableName);
            
            // 生成枚举类
            string enumSavePath = ConfigUtil.GetEnumOutputPath(file.FullName);
            ConfigFactory.GenerateExcelEnum(table, jsonFileName, enumSavePath);
        }
    }

    /// <summary>
    /// 生成导表数据
    /// </summary>
    private static void GenerateGlobalConfig()
    {
        ConfigSettings.GlobalExcelPath.CreateDirIfNotExists();
        
        // 记在指定路径中的所有Excel文件 用于生成对应的3个文件
        DirectoryInfo dInfo = new DirectoryInfo(ConfigSettings.GlobalExcelPath);
        
        // 得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
        FileInfo[] files = dInfo.GetFiles("*.*", SearchOption.AllDirectories);
        
        // 数据表容器
        DataTableCollection tableCollection = null;
        
        for (int i = 0; i < files.Length; i++)
        {
            // 如果不是excel文件就不要处理了
            if (files[i].Extension != ".xlsx" && files[i].Extension != ".xls")
                continue;

            GenerateConfig(tableCollection, files[i]);
        }
    }

    /// <summary>
    /// 生成导表
    /// </summary>
    private static void GenerateConfig(DataTableCollection tableCollection, FileInfo file)
    {
        // 打开一个Excel文件得到其中的所有表的数据 ( 如果报错！可能是因为没有关闭Excel表，请关闭所有打开的Excel表! )
        using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            tableCollection = excelReader.AsDataSet().Tables;
            fs.Close();
        }

        // 遍历文件中的所有表的信息
        foreach (DataTable table in tableCollection)
        {
            // 生成JSON文件名，包含路径前缀（如果有的话）
            string jsonFileName = ConfigUtil.GetJsonFileName(file.FullName, table.TableName);
                
            // 生成容器类
            string containerSavePath = ConfigUtil.GetContainerOutputPath(file.FullName);
            ConfigFactory.GenerateExcelContainer(table, jsonFileName, containerSavePath);
                
            // 生成JSON数据
            string jsonSavePath = ConfigUtil.GetJsonOutputPath(file.FullName);
            ConfigFactory.GenerateExcelJson(table, jsonFileName, jsonSavePath);
        }
    }
}
