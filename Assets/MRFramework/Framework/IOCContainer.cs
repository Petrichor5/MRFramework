using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    public class IOCContainer
    {
        private Dictionary<Type, object> m_Instances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (!m_Instances.ContainsKey(key))
            {
                m_Instances[key] = instance;
            }
            else
            {
                m_Instances.Add(key, instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (m_Instances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }
}
