using MRFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDemo : MonoBehaviour
{
    private void Start()
    {
        Log.Initalize();
        DontDestroyOnLoad(gameObject);

        MRF.EventMgr.AddEventListener<string>(SceneEvent.EnterScene, EnterScene)
            .AutoUnRegister(this.gameObject);

        MRF.EventMgr.AddEventListener<string>(SceneEvent.ExitScene, ExitScene)
            .AutoUnRegister(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MRF.SceneMgr.LoadSceneAsync("Test/Scene_Test_Main.unity",
                () =>
                {
                    Log.Info("准备开始卸载上个场景");
                },
                () =>
                {
                    Log.Info("加载下个场景结束");
                });
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 不显示加载进度条（默认开启）
            MRF.SceneMgr.LoadSceneAsync("Common/Scene_Common_GameStart.unity", null, null, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 返回上个场景（前提是上个场景是用SceneMgr加载出来的）
            MRF.SceneMgr.ReturnLastSceneAsync();
        }
    }

    private void EnterScene(string sceneName)
    {
        Log.Info("触发进入场景事件 " + sceneName);
    }

    private void ExitScene(string sceneName)
    {
        Log.Info("触发退出场景事件 " + sceneName);
    }
}
