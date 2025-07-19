using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Patterns.Singleton;

namespace Foundations.Architecture.InterfaceResolver
{
    [Serializable]
    public class InterfaceComponentResolver : Singleton<InterfaceComponentResolver>
    {
        // 단일 PlugIn 
        public bool TryResolveSingle<T>(GameObject container, out T plugIn) where T : class
        {
            plugIn = container.GetComponent<T>();
            return plugIn != null;
        }

        // 다중 PlugIn (같은 인터페이스를 여러 개가 구현)
        public bool TryResolveList<T>(GameObject container, out List<T> plugIns) where T : class
        {
            plugIns = new List<T>(container.GetComponents<T>());
            return plugIns.Count > 0;
        }

        // 다중 PlugIn (같은 인터페이스를 여러 개가 구현)
        public bool TryResolveHashSet<T>(GameObject container, out HashSet<T> plugIns) where T : class
        {
            plugIns = new HashSet<T>();
            bool found = false;

            foreach (var comp in container.GetComponents<MonoBehaviour>())
            {
                if (comp is T plugIn)
                {
                    if (plugIns.Add(plugIn))  // 중복이면 추가되지 않음
                        found = true;
                }
            }

            return found;
        }

        // Dictionary by Type
        public bool TryResolveTypeKeyDictionary<T>(GameObject container, out Dictionary<Type, T> plugIns) where T : class
        {
            plugIns = new Dictionary<Type, T>();
            bool found = false;

            foreach (var comp in container.GetComponents<MonoBehaviour>())
            {
                if (comp is T plugIn)
                {
                    plugIns[plugIn.GetType()] = plugIn;
                    found = true;
                }
            }

            return found;
        }
    }
}