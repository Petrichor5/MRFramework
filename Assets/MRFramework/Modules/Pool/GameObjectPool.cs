using System;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// GameObject对象池
    /// </summary>
    public class GameObjectPool<T> : Pool<T>, IDisposable where T : MonoBehaviour, new()
    {
        private GameObject m_ParentGameObject;
        private string m_ResPath;
        private Action<T> m_ResetMethod;

        /// <summary>
        /// 创建GameObject对象池
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="resetMethod">重置函数</param>
        public GameObjectPool(string resPath, Action<T> resetMethod = null)
        {
            m_ParentGameObject = new GameObject(typeof(T).Name);
            GameObject.DontDestroyOnLoad(m_ParentGameObject);
            m_ResPath = resPath;
            m_ResetMethod = resetMethod;
        }

        public void Allocate(Action<T> callback)
        {
            if (m_CacheStack.Count == 0)
            {
                AssetManager.Instance.InstantiateAsync(m_ResPath, (go) =>
                {
                    GameObject gameObject = go;
                    callback?.Invoke(gameObject.GetComponent<T>());
                });
            }
            else
            {
                T t = m_CacheStack.Pop();
                t.gameObject.SetActive(true);
                callback?.Invoke(t);
            }
        }

        public bool Recycle(GameObject go)
        {
            T t = go.GetComponent<T>();
            m_ResetMethod?.Invoke(t);
            go.transform.SetParent(m_ParentGameObject.transform);
            go.SetActive(false);
            m_CacheStack.Push(t);
            return true;
        }

        public override bool Recycle(T t)
        {
            m_ResetMethod?.Invoke(t);
            t.transform.SetParent(m_ParentGameObject.transform);
            t.gameObject.SetActive(false);
            m_CacheStack.Push(t);
            return true;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void ClearPool()
        {
            foreach (T t in m_CacheStack)
            {
                AssetManager.Instance.ReleaseInstance(t.gameObject);
            }
            m_CacheStack.Clear();
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        public void Dispose()
        {
            ClearPool();

            GameObject.Destroy(m_ParentGameObject);
            m_ParentGameObject = null;

            m_ResetMethod = null;
            m_ResPath = null;
        }
    }
}
