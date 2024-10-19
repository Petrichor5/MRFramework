using System;
using System.Reflection;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 单例创建类
    /// </summary>
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class, ISingleton
        {
            var type = typeof(T);
            var monoBehaviourType = typeof(MonoBehaviour);

            // 创建mono单例
            if (monoBehaviourType.IsAssignableFrom(type))
            {
                return CreateMonoSingleton<T>();
            }
            // 创建普通单例
            else
            {
                var instance = CreateNonPublicConstructorObject<T>();
                instance.OnSingletonInit();
                return instance;
            }
        }

        private static T CreateNonPublicConstructorObject<T>() where T : class
        {
            var type = typeof(T);
            // 获取所有私有构造函数
            var constructorInfos = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            // 找到无参构造函数
            var ctor = Array.Find(constructorInfos, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                // 当这条触发时，检查一下单例类有没有私有化无参构造函数
                throw new Exception("没有找到私有无参构造函数" + type);
            }

            return ctor.Invoke(null) as T;
        }

        private static T CreateMonoSingleton<T>() where T : class, ISingleton
        {
            T instance;
            var type = typeof(T);

            // 判断T实例存在的条件是否满足
            if (!Application.isPlaying)
                return null;

            // 判断当前场景中是否存在T实例
            instance = UnityEngine.Object.FindObjectOfType(type) as T;
            if (instance != null)
            {
                instance.OnSingletonInit();
                return instance;
            }

#if UNITY_EDITOR
            //MemberInfo：获取有关成员属性的信息并提供对成员元数据的访问
            MemberInfo info = typeof(T);
            //获取T类型 自定义属性，并找到相关路径属性，利用该属性创建T实例
            var attributes = info.GetCustomAttributes(true);
            foreach (var atribute in attributes)
            {
                var defineAttri = atribute as MonoSingletonPathAttribute;
                if (defineAttri == null)
                {
                    continue;
                }

                instance = CreateComponentOnGameObject<T>(defineAttri.PathInHierarchy, true);
                break;
            }
#endif

            //创建同名Obj 并挂载相关脚本 组件
            if (instance == null)
            {
                var obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent(typeof(T)) as T;
                GameObject.DontDestroyOnLoad(obj);
            }

            instance?.OnSingletonInit();
            return instance;
        }

        /// <summary>
        /// 在GameObject上创建T组件（脚本）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">路径（应该就是Hierarchy下的树结构路径）</param>
        /// <param name="dontDestroy">不要销毁 标签</param>
        /// <returns></returns>
        private static T CreateComponentOnGameObject<T>(string path, bool dontDestroy) where T : class
        {
            var obj = FindGameObject(path, true, dontDestroy);
            if (obj == null)
            {
                obj = new GameObject("Singleton of " + typeof(T).Name);
                if (dontDestroy)
                {
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                }
            }

            return obj.AddComponent(typeof(T)) as T;
        }

        /// <summary>
        /// 查找Obj（一个嵌套查找Obj的过程）
        /// </summary>
        /// <param name="root">父节点</param>
        /// <param name="subPath">拆分后的路径节点</param>
        /// <param name="index">下标</param>
        /// <param name="build">true</param>
        /// <param name="dontDestroy">不要销毁 标签</param>
        /// <returns></returns>
        private static GameObject FindGameObject(GameObject root, string[] subPath, int index, bool build,
            bool dontDestroy)
        {
            GameObject client = null;

            if (root == null)
            {
                client = GameObject.Find(subPath[index]);
            }
            else
            {
                var child = root.transform.Find(subPath[index]);
                if (child != null)
                {
                    client = child.gameObject;
                }
            }

            if (client == null)
            {
                if (build)
                {
                    client = new GameObject(subPath[index]);
                    if (root != null)
                    {
                        client.transform.SetParent(root.transform);
                    }

                    if (dontDestroy && index == 0)
                    {
                        GameObject.DontDestroyOnLoad(client);
                    }
                }
            }

            if (client == null)
            {
                return null;
            }

            return ++index == subPath.Length ? client : FindGameObject(client, subPath, index, build, dontDestroy);
        }

        /// <summary>
        /// 查找Obj（对于路径 进行拆分）
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="build">true</param>
        /// <param name="dontDestroy">不要销毁 标签</param>
        /// <returns></returns>
        private static GameObject FindGameObject(string path, bool build, bool dontDestroy)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var subPath = path.Split('/');
            if (subPath == null || subPath.Length == 0)
            {
                return null;
            }

            return FindGameObject(null, subPath, 0, build, dontDestroy);
        }
    }
}
