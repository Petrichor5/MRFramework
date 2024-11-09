using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/SceneManager")]
    public class SceneManager : MonoSingleton<SceneManager>
    {
        private string m_CurrentScenePath; // 当前场景资源路径
        private string m_LastScenePath; // 上个场景资源路径
        private AsyncOperationHandle<SceneInstance> m_CurrentLoadHandle; // 当前场景资源句柄

        private void OnDestroy()
        {
            ClearSceneReferences();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scenePath">场景资源路径</param>
        /// <param name="beforeUnload">卸载上个场景之前回调</param>
        /// <param name="onSceneLoaded">加载场景成功回调</param>
        /// <param name="isShowProgress">是否显示进度条加载</param>
        public void LoadSceneAsync(string scenePath, Action beforeUnload = null, Action onSceneLoaded = null, bool isShowProgress = true)
        {
            if (isShowProgress)
            {
                UIManager.Instance.OpenPanel<WBP_GlobalUI_SceneProgressBar>((panel) =>
                {
                    StartCoroutine(LoadSceneCoroutine(scenePath, beforeUnload, onSceneLoaded, panel.UpdateProgress));
                });
            }
            else
            {
                StartCoroutine(LoadSceneCoroutine(scenePath, beforeUnload, onSceneLoaded, null));
            }
        }

        /// <summary>
        /// 返回上个场景
        /// </summary>
        /// <param name="beforeUnload">卸载上个场景之前回调</param>
        /// <param name="onSceneLoaded">加载场景成功回调</param>
        /// <param name="isShowProgress">是否显示进度条加载</param>
        public void ReturnLastSceneAsync(Action beforeUnload = null, Action onSceneLoaded = null, bool isShowProgress = true)
        {
            if (isShowProgress)
            {
                UIManager.Instance.OpenPanel<WBP_GlobalUI_SceneProgressBar>((panel) =>
                {
                    StartCoroutine(LoadSceneCoroutine(GetLastSceneName(), beforeUnload, onSceneLoaded, panel.UpdateProgress));
                });
            }
            else
            {
                StartCoroutine(LoadSceneCoroutine(GetLastSceneName(), beforeUnload, onSceneLoaded, null));
            }
        }

        private IEnumerator LoadSceneCoroutine(string scenePath, Action beforeUnload, Action onSceneLoaded, Action<float> progress)
        {
            m_LastScenePath = m_CurrentScenePath;
            var lastLoadHandle = m_CurrentLoadHandle;
            m_CurrentScenePath = scenePath;

            // 卸载当前场景前执行回调
            beforeUnload?.Invoke();

            // 1. 尝试加载新场景但不激活
            m_CurrentLoadHandle = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single, false);

            if (progress != null)
            {
                // 假进度条模拟
                float fakeProgress = 0f;
                while (!m_CurrentLoadHandle.IsDone)
                {
                    fakeProgress += Time.deltaTime * 0.2f; // 调整速度
                    fakeProgress = Mathf.Clamp01(fakeProgress);
                    progress?.Invoke(fakeProgress);
                    yield return null;
                }

                // 确保进度条达到100%
                while (fakeProgress < 1f)
                {
                    fakeProgress += Time.deltaTime * 0.8f;
                    fakeProgress = Mathf.Clamp01(fakeProgress);
                    progress?.Invoke(fakeProgress);
                    yield return null;
                }
            }
            else
            {
                while (!m_CurrentLoadHandle.IsDone)
                    yield return null;
            }

            // 2. 卸载当前场景
            if (!string.IsNullOrEmpty(m_LastScenePath) && m_CurrentLoadHandle.IsValid())
            {
                // 派发退出场景事件
                EventManager.Instance.TriggerEventListener(SceneEvent.ExitScene, lastLoadHandle.Result.Scene.name);

                var unloadHandle = Addressables.UnloadSceneAsync(lastLoadHandle);
                while (!unloadHandle.IsDone)
                    yield return null;
            }

            // 3. 激活新场景
            if (m_CurrentLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                AsyncOperation asyncOperation = m_CurrentLoadHandle.Result.ActivateAsync();
                asyncOperation.completed += (ao) =>
                {
                    UIManager.Instance.ClosePanel<WBP_GlobalUI_SceneProgressBar>();
                    onSceneLoaded?.Invoke();
                    // 派发进入场景事件
                    EventManager.Instance.TriggerEventListener(SceneEvent.EnterScene, m_CurrentLoadHandle.Result.Scene.name);
                };
            }
            else
            {
                Log.Error($"[SceneManager] 场景激活失败: {scenePath}");
            }
        }

        /// <summary>
        /// 获取上个场景的名称
        /// </summary>
        /// <returns></returns>
        public string GetLastSceneName()
        {
            return m_LastScenePath;
        }

        /// <summary>
        /// 清理和释放场景加载相关的引用和资源
        /// </summary>
        public void ClearSceneReferences()
        {
            m_LastScenePath = null;
            m_CurrentScenePath = null;
            if (m_CurrentLoadHandle.IsValid())
            {
                // 异步卸载场景 句柄自动释放引用
                Addressables.UnloadSceneAsync(m_CurrentLoadHandle);
            }
        }
    }
}
