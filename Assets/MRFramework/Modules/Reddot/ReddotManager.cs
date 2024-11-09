using Config;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class ReddotEventHandler
{
    private Action<bool> m_OnEvent;

    public void Register(Action<bool> onEvent)
    {
        m_OnEvent += onEvent;
    }

    public void UnRegister(Action<bool> onEvent)
    {
        m_OnEvent -= onEvent;
    }

    public void Clear()
    {
        m_OnEvent = null;
    }

    public void Trigger(bool flag)
    {
        m_OnEvent?.Invoke(flag);
    }
}

[MonoSingletonPath("MRFramework/ReddotManager")]
public class ReddotManager : MonoSingleton<ReddotManager>
{
    private Dictionary<string, ReddotSubManagerBase> m_ReddotSubManagerDic;
    private Dictionary<EReddot, Dictionary<string, bool>> m_ReddotTree;
    private Dictionary<string, ReddotEventHandler> m_ReddotEventDic;

    public override void OnSingletonInit()
    {
        m_ReddotSubManagerDic = new Dictionary<string, ReddotSubManagerBase>();
        m_ReddotTree = new Dictionary<EReddot, Dictionary<string, bool>>();
        m_ReddotEventDic = new Dictionary<string, ReddotEventHandler>();
    }

    /// <summary>
    /// 注册红点子管理器
    /// </summary>
    public void RegisterReddotManager<T>() where T : ReddotSubManagerBase, new()
    {
        T subMgr = new T();
        string name = typeof(T).Name;

        if (!m_ReddotSubManagerDic.ContainsKey(name))
        {
            subMgr.OnRegister();
            m_ReddotSubManagerDic.Add(name, subMgr);
        }
        else
        {
            Log.Error($"[ReddotManager] 注册失败，红点子管理器不能重复注册！Name = {name}");
        }
    }

    /// <summary>
    /// 注销红点子管理器
    /// </summary>
    public void UnRegisterReddotManager<T>() where T : ReddotSubManagerBase, new()
    {
        string name = typeof(T).Name;

        if (m_ReddotSubManagerDic.ContainsKey(name))
        {
            var subMgr = m_ReddotSubManagerDic[name];
            subMgr.OnUnRegister();
            m_ReddotSubManagerDic.Remove(name);
        }
        else
        {
            Log.Error($"[ReddotManager] 注销失败，没有找到该红点子管理器！Name = {name}");
        }
    }

    /// <summary>
    /// 注册红点子管理器
    /// </summary>
    public T GetReddotManager<T>() where T : ReddotSubManagerBase
    {
        string name = typeof(T).Name;
        m_ReddotSubManagerDic.TryGetValue(name, out var subMgr);
        return subMgr.ConvertTo<T>();
    }

    /// <summary>
    /// 添加红点节点
    /// </summary>
    public void AddReddotNode(EReddot eReddot, string node = null)
    {
        string key = GetKeyName(eReddot, node);
        if (m_ReddotTree.ContainsKey(eReddot))
        {
            if (!m_ReddotTree[eReddot].ContainsKey(key))
                m_ReddotTree[eReddot][key] = false;
        }
        else
        {
            // 创建红点树
            
            Dictionary<string, bool> nodes = MRFramework.DictionaryPool<string, bool>.Get();
            nodes.Add(node, false);
            m_ReddotTree.Add(eReddot, nodes);
        }
    }

    /// <summary>
    /// 移除红点节点
    /// </summary>
    public void RemoveReddotNode(EReddot eReddot, string node = null)
    {
        string key = GetKeyName(eReddot, node);
        if (m_ReddotTree.ContainsKey(eReddot))
        {
            if (m_ReddotTree[eReddot].ContainsKey(key))
            {
                m_ReddotTree[eReddot].Remove(key);
                // 移除整颗红点树
                if (m_ReddotTree[eReddot].Count <= 0)
                {
                    MRFramework.DictionaryPool<string, bool>.Release(m_ReddotTree[eReddot]);
                    m_ReddotTree.Remove(eReddot);
                }
            }
            return;
        }
        Log.Warning($"[ReddotManager] 没有找到该红点节点！Root = {eReddot}, Node = {node}");
    }

    /// <summary>
    /// 获取红点的状态
    /// </summary>
    public bool GetReddotState(EReddot eReddot, string node = null)
    {
        string key = GetKeyName(eReddot, node);
        if (m_ReddotTree.ContainsKey(eReddot))
        {
            if (m_ReddotTree[eReddot].ContainsKey(key))
                return m_ReddotTree[eReddot][key];
        }
        Log.Error($"[ReddotManager] 没有找到该红点节点！Root = {eReddot}, Node = {node}");
        return false;
    }

    /// <summary>
    /// 设置红点状态
    /// </summary>
    public void SetReddotState(EReddot eReddot, string node, bool state)
    {
        if (m_ReddotTree.ContainsKey(eReddot))
        {
            string key = GetKeyName(eReddot, node);
            if (m_ReddotTree[eReddot].ContainsKey(key))
            {
                m_ReddotTree[eReddot][key] = state;
                TriggerEventListener(key, state);
            }
            else
            {
                Log.Error($"[ReddotManager] 没有找到该红点节点！Root = {eReddot}, Node = {node}");
            }
            return;
        }

        Log.Error($"[ReddotManager] 没有找到该红点树！Root = {eReddot}");
    }

    /// <summary>
    /// 刷新根节点状态
    /// </summary>
    public void RefreshRootReddotState(EReddot eReddot)
    {
        if (m_ReddotTree.ContainsKey(eReddot))
        {
            bool flag = false;

            // 查找子节点是否有显示的状态
            foreach (var state in m_ReddotTree[eReddot].Values)
            {
                if (state)
                {
                    flag = true;
                    break;
                }
            }

            // 刷新根节点状态
            string key = ReddotTool.MKReddotKey(eReddot);
            TriggerEventListener(key, flag);

            return;
        }

        Log.Error($"[ReddotManager] 没有找到该红点树！Root = {eReddot}");
    }

    /// <summary>
    /// 重置整颗红点树状态为False
    /// </summary>
    public void ResetReddotState(EReddot eReddot)
    {
        if (m_ReddotTree.ContainsKey(eReddot))
        {
            // 刷新子节点状态
            Dictionary<string, bool> nodeDic = m_ReddotTree[eReddot];
            string[] nodes = nodeDic.Keys.ToArray();
            for (int i = 0; i < nodes.Length; i++)
            {
                nodeDic[nodes[i]] = false;
                TriggerEventListener(nodes[i], false);
            }

            // 刷新根节点状态
            TriggerEventListener(eReddot.ToString(), false);

            return;
        }

        Log.Error($"[ReddotManager] 没有找到该红点树！Root = {eReddot}");
    }

    /// <summary>
    /// 添加红点事件
    /// </summary>
    public void AddEventListener(string key, Action<bool> onEvent)
    {
        if (onEvent == null)
        {
            Log.Error($"[ReddotManager] 不能添加空的事件函数！EventKey = {key}");
            return;
        }

        ReddotEventHandler eventHandler;
        if (m_ReddotEventDic.TryGetValue(key, out ReddotEventHandler e))
        {
            eventHandler = e;
        }
        else
        {
            eventHandler = new ReddotEventHandler();
            m_ReddotEventDic.Add(key, eventHandler);
        }
        eventHandler.Register(onEvent);
    }

    /// <summary>
    /// 移除红点事件
    /// </summary>
    public void RemoveEventListener(string key, Action<bool> onEvent)
    {
        if (m_ReddotEventDic.TryGetValue(key, out ReddotEventHandler eventHandler))
        {
            eventHandler?.UnRegister(onEvent);
        }
        else
        {
            Log.Warning($"[ReddotManager] 没有找到该红点事件！EventKey = {key}");
        }
    }

    /// <summary>
    /// 触发红点事件
    /// </summary>
    public void TriggerEventListener(string key, bool state)
    {
        if (m_ReddotEventDic.TryGetValue(key, out ReddotEventHandler eventHandler))
        {
            eventHandler.Trigger(state);
            return;
        }

        Log.Error($"[ReddotManager] 没有找到该红点事件！EventKey = {key}");
    }

    /// <summary>
    /// 构建红点的Key
    /// </summary>
    private string GetKeyName(EReddot eReddot, string node)
    {
        string key;
        if (!string.IsNullOrEmpty(node))
            key = ReddotTool.MKReddotKey(eReddot, node);
        else
            key = eReddot.ToString();
        return key;
    }
}
