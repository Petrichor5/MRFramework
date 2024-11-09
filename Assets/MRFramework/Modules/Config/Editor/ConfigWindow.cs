using Excel;
using MRFramework;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ConfigWindow : EditorWindow
{
    private List<string> m_ExcelFilePathList = new List<string>();

    [MenuItem("MRFramework/导表生成工具")]
    public static void ShowWindow()
    {
        var window = GetWindow<ConfigWindow>("导表生成工具");
        window.minSize = new Vector2(300, 300);
    }

    private void OnGUI()
    {
        GUILayout.Label("将 Excel 文件拖拽到以下区域", EditorStyles.boldLabel);

        // 创建拖拽区域
        Rect dropArea = GUILayoutUtility.GetRect(0, 100, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "拖拽文件到这里");

        // 检测拖拽事件
        Event evt = Event.current;
        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            if (dropArea.Contains(evt.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (var path in DragAndDrop.paths)
                    {
                        if (path.EndsWith(".xlsx") || path.EndsWith(".xls"))
                        {
                            if (!m_ExcelFilePathList.Contains(path))
                            {
                                m_ExcelFilePathList.Add(path);
                                Debug.Log("添加 Excel 文件路径: " + path);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("请拖拽 Excel 文件！");
                        }
                    }
                }
                Event.current.Use();
            }
        }

        // 显示已拖拽的文件列表
        if (m_ExcelFilePathList.Count > 0)
        {
            GUILayout.Label("已拖拽的文件：", EditorStyles.boldLabel);
            foreach (var file in m_ExcelFilePathList)
            {
                GUILayout.Label(file);
            }
        }

        if (GUILayout.Button("生成导表"))
        {
            GenerateConfigFiles();
            m_ExcelFilePathList.Clear();
            AssetDatabase.Refresh(); // 刷新Project窗口
            Debug.Log("已清空文件列表");
        }

        if (GUILayout.Button("清空文件列表"))
        {
            m_ExcelFilePathList.Clear();
            Debug.Log("已清空文件列表");
        }
    }

    private void GenerateConfigFiles()
    {
        foreach (var filePath in m_ExcelFilePathList)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.DirectoryName.Contains("Global"))
            {
                GenerateGlobalConfig(file);
            }
            else if (file.DirectoryName.Contains("Enum"))
            {
                GenerateEnumConfig(file);
            }
        }
    }

    private void GenerateGlobalConfig(FileInfo file)
    {
        try
        {
            using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                var tableCollection = excelReader.AsDataSet().Tables;

                foreach (DataTable table in tableCollection)
                {
                    string jsonFileName = ConfigTool.GetJsonFileName(file.FullName, table.TableName);
                    string containerSavePath = ConfigTool.GetContainerOutputPath(file.FullName);
                    ConfigFactory.GenerateExcelContainer(table, jsonFileName, containerSavePath);

                    string jsonSavePath = ConfigTool.GetJsonOutputPath(file.FullName);
                    ConfigFactory.GenerateExcelJson(table, jsonFileName, jsonSavePath);
                }

                excelReader.Close();
            }
            Debug.Log("成功生成配置文件：" + file.Name);
        }
        catch (IOException e)
        {
            Debug.LogError($"处理文件 {file.Name} 时出错: {e.Message}");
        }
    }

    private void GenerateEnumConfig(FileInfo file)
    {
        try
        {
            using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                var tableCollection = excelReader.AsDataSet().Tables;

                foreach (DataTable table in tableCollection)
                {
                    string jsonFileName = ConfigTool.GetEnumFileName(file.FullName, table.TableName);
                    string enumSavePath = ConfigTool.GetEnumOutputPath(file.FullName);
                    ConfigFactory.GenerateExcelEnum(table, jsonFileName, enumSavePath);
                }

                excelReader.Close();
            }
            Debug.Log("成功生成枚举配置文件：" + file.Name);
        }
        catch (IOException e)
        {
            Debug.LogError($"处理文件 {file.Name} 时出错: {e.Message}");
        }
    }
}
