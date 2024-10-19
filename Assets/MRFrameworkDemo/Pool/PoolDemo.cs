using System.Collections.Generic;
using MRFramework;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoolDemo : MonoBehaviour
{
    string gameObjectKey = "Assets/MRFrameworkDemo/Pool/GameObjectItem.prefab";
    private GameObjectPool<GameObjectItem> m_GameObjectPool;
    private List<GameObject> m_GameObjectList;
    
    private void Start()
    {
        m_GameObjectPool = new GameObjectPool<GameObjectItem>(gameObjectKey, OnResetGameObject);
        m_GameObjectList = new List<GameObject>();
    }

    public void Reset()
    {
        if (m_GameObjectPool != null)
            m_GameObjectPool.Dispose();
        m_GameObjectPool = new GameObjectPool<GameObjectItem>(gameObjectKey, OnResetGameObject);

        if (m_GameObjectList != null)
            m_GameObjectList.Clear();
        m_GameObjectList = new List<GameObject>();
    }

    #region GameObject Pool
    
    // 重置回收对象
    private void OnResetGameObject(GameObjectItem item)
    {
        item.Reset();
    }
    
    // 获取对象
    public void GetGameObject()
    {
        if (m_GameObjectPool == null) return;
        m_GameObjectPool.Allocate((item) =>
        {
            float x = Random.Range(-8, 8);
            float y = Random.Range(-4, 4);
            item.gameObject.transform.position = new Vector3(x, y);
            m_GameObjectList.Add(item.gameObject);
        });
    }
    
    // 回收对象
    public void ReturnGameObject()
    {
        if (m_GameObjectPool == null) return;
        if (m_GameObjectList.Count > 0)
        {
            int index = m_GameObjectList.Count - 1;
            m_GameObjectPool.Recycle(m_GameObjectList[m_GameObjectList.Count - 1]);
            m_GameObjectList.RemoveAt(index);
        }
    }

    // 清空对象池
    public void ClearGameObjectPool()
    {
        if (m_GameObjectPool == null) return;
        m_GameObjectPool.ClearPool();
        m_GameObjectList.Clear();
    }
    
    // 销毁对象池
    public void DisposeGameObjectPool()
    {
        if (m_GameObjectPool == null) return;
        m_GameObjectPool.Dispose();
        m_GameObjectList.Clear();
        m_GameObjectPool = null;
        m_GameObjectList = null;
    }

    #endregion
}
