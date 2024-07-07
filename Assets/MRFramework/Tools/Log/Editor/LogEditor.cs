using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class LogEditor
{
    [MenuItem("MRFramework/日志系统/打开日志系统")]
    public static void LoadReport()
    {
        ScriptingDefineSymbols.AddScriptingDefineSymbol("OPEN_LOG");
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        AssetDatabase.Refresh();
        Debug.Log("[Log] 打开日志系统");
    }

    [MenuItem("MRFramework/日志系统/关闭日志系统")]
    public static void CloseReport()
    {
        ScriptingDefineSymbols.RemoveScriptingDefineSymbol("OPEN_LOG");
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        AssetDatabase.Refresh();
        Debug.Log("[Log] 关闭日志系统");
    }
}