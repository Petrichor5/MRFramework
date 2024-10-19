using MRFramework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneStartGameEditor
{
    [MenuItem("MRFramework/设置游戏启动场景/默认场景启动", priority = 1)]
    private static void DefaultSceneStartGame()
    {
        string path = SceneSettings.GameStartScenePath; // 默认场景路径
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        Debug.Log($"默认场景进入游戏: {path}");
    }
    
    [MenuItem("MRFramework/设置游戏启动场景/当前场景启动", priority = 2)]
    private static void CurSceneStartGame()
    {
        EditorSceneManager.playModeStartScene = null;
        Debug.Log($"当前场景进入游戏");
    }
}
