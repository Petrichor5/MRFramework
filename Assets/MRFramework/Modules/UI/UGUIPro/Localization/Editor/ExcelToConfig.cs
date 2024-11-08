using Excel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using MRFramework.UGUIPro;

public class ExcelToConfig
{
    /// <summary>
    /// 解析表名 （在实际开发中可能某列会存在不解析的情况）
    /// </summary>
    private static List<string> m_ParseKeyList = new List<string>() { "Chinese", "English", "Thai" };

    [MenuItem("MRFramework/生成Excel多语言配置")]
    static void GeneratorLoaclization()
    {
        string path = LocalizationSettings.ExcelLaodPath;
        path.CreateDirIfNotExists();
        Debug.Log("Excel 文件路径: " + path + LocalizationSettings.ExcelFileName);
        DataSet resultSet = LaodExcel(path + LocalizationSettings.ExcelFileName);
        GenerateLocalizationCfg(resultSet);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="excelFile">Excel file.</param> 
    public static DataSet LaodExcel(string excelFile)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        return mExcelReader.AsDataSet();
    }

    /// <summary>
    /// 生成多语言配置
    /// </summary>
    public static void GenerateLocalizationCfg(DataSet resultSet)
    {
        //判断Excel文件中是否存在数据表
        if (resultSet.Tables.Count < 1)
            return;
        //默认读取第一个数据表
        DataTable mSheet = resultSet.Tables[0];
        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表以保存全部数据
        Dictionary<string, List<LocalizationData>> localizationDataListDic =
            new Dictionary<string, List<LocalizationData>>();
        for (int i = 1; i < colCount; i++)
        {
            List<LocalizationData> dataList = new List<LocalizationData>();

            string cloName = mSheet.Rows[0][i].ToString();
            int startIndex = cloName.LastIndexOf(@"（");
            if (startIndex > 0)
                cloName = cloName.Substring(0, startIndex - 1);

            if (cloName != "" && m_ParseKeyList.Contains(cloName))
            {
                localizationDataListDic.Add(cloName, dataList);
            }
        }

        //读取数据
        for (int i = 1; i < rowCount; i++)
        {
            for (int j = 1; j < colCount; j++)
            {
                //创建实例
                LocalizationData data = new LocalizationData();

                data.Key = mSheet.Rows[i][colCount - 1].ToString();
                data.value = mSheet.Rows[i][j].ToString();
                // Debug.Log("key:" + data.Key);
                if (!string.IsNullOrEmpty(data.value) && mSheet.Rows[0][j].ToString() != "" &&
                    !string.IsNullOrEmpty(data.Key))
                {
                    string cloName = mSheet.Rows[0][j].ToString();
                    int startIndex = cloName.LastIndexOf(@"（");
                    if (startIndex > 0)
                        cloName = cloName.Substring(0, startIndex - 1);
                    if (localizationDataListDic.ContainsKey(cloName))
                        localizationDataListDic[cloName].Add(data); //添加至列表
                }
            }
        }

        foreach (var item in localizationDataListDic)
        {
            // 生成Json字符串
            string json = JsonConvert.SerializeObject(item.Value, Newtonsoft.Json.Formatting.Indented);
            int startIndex = item.Key.LastIndexOf("（");
            string itemKey = item.Key;

            if (startIndex > 0)
            {
                itemKey = item.Key.Substring(0, startIndex);
            }

            // 创建单独语言文件路径
            var savePath = Path.Combine(LocalizationSettings.OutPutPath);
            Directory.CreateDirectory(savePath); // 确保目录存在

            json = json.Replace(@"\\n", @"\n");

            // 写入文件
            string filePath = Path.Combine(savePath, $"{itemKey}.txt");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    textWriter.Write(json);
                }
            }

            Debug.Log("写入完成 Path：" + filePath);
        }

        AssetDatabase.Refresh();
    }
}