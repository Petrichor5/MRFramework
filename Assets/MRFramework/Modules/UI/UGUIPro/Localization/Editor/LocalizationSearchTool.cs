using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MRFramework.UGUIPro
{
    /// <summary>
    /// 多语言文本查找工具
    /// </summary>
    public class LocalizationSearchTool
    {
        // key：文本内容 value：本地化key
        private static Dictionary<string, string> m_SearchDic = new Dictionary<string, string>(); // 用字典收集查找到的文本，以及过滤重复的文本
        private static int m_Index;

        [MenuItem("MRFramework/扫描预制件文本控件")]
        public static void ScanPrefabsForUIText()
        {
            string folderPath = "Assets/AssetPackage/UI"; // 设置UI文件夹路径
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath }); // 指定查找预制体

            foreach (string prefabPath in prefabPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab != null)
                {
                    InitKey();

                    TextPro[] texts = prefab.GetComponentsInChildren<TextPro>(true); // true 表示包含禁用的对象
                    foreach (TextPro text in texts)
                    {
                        string key = GenLocalizeKey(prefab.name);
                        if (!m_SearchDic.ContainsKey(text.text))
                        {
                            m_SearchDic.Add(text.text, key);
                        }

                        text.LocalizationTextExtend.UseLocalization = true;
                        text.LocalizationTextExtend.Key = key;

                        Debug.Log($"面板：{prefab.name} | 文本控件：{text.name} | 内容：{text.text} | Key：{key}");
                    }

                    TextMeshPro[] textMeshs = prefab.GetComponentsInChildren<TextMeshPro>(true); // true 表示包含禁用的对象
                    foreach (TextMeshPro text in textMeshs)
                    {
                        string key = GenLocalizeKey(prefab.name);
                        if (!m_SearchDic.ContainsKey(text.text))
                        {
                            m_SearchDic.Add(text.text, key);
                        }

                        text.LocalizationTextExtend.UseLocalization = true;
                        text.LocalizationTextExtend.Key = key;

                        Debug.Log($"面板：{prefab.name} | 文本控件：{text.name} | 内容：{text.text} | Key：{key}");
                    }

                    // 标记对象为已修改, 确保修改生效
                    EditorUtility.SetDirty(prefab);
                }
            }

            PrintSearchDic();
            ExportToExcel();
        }

        private static void InitKey()
        {
            m_Index = 1;
        }

        private static string GenLocalizeKey(string prefabName)
        {
            return prefabName + "_" + m_Index;
        }

        private static void ExportToExcel()
        {
            string filePath = "Assets/LocalizationData.xlsx";
            FileInfo file = new FileInfo(filePath);

            if (file.Exists)
            {
                file.Delete(); // 删除已存在的文件，重新创建
            }

            // 创建工作簿
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Localization");

            // 创建表头
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("本地化Key");
            headerRow.CreateCell(1).SetCellValue("文本内容");

            // 填充数据
            int rowIndex = 1;
            foreach (var item in m_SearchDic)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(item.Value);
                row.CreateCell(1).SetCellValue(item.Key);
            }

            // 保存文件
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            Debug.Log($"Excel 文件已导出到 {filePath}");
        }

        private static void PrintSearchDic()
        {
            Debug.Log("查找到的文本 ====================>");
            foreach (var item in m_SearchDic)
            {
                Debug.Log($"内容：{item.Key} | Key：{item.Value}");
            }
        }
    }
}
