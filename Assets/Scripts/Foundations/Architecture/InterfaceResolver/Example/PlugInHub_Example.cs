using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundations.Architecture.InterfaceResolver
{
    public interface IPlugInHub
    {
        public void UpdatePlugInAll();
    }

    public interface IPlugIn
    {
        public void UpdatePlugIn();
    }

    [Serializable]
    public class PlugInHub_Example<T> : IPlugInHub where T : class, IPlugIn
    {
        [SerializeField] private GameObject PlugInContainerObject;
        private Dictionary<Type, T> plugIns;

        public PlugInHub_Example()
        {
            this.plugIns = new();

            this.AllocatePlugIn();
        }
        private void AllocatePlugIn()
        {
            var interfaceComponentResolver = InterfaceComponentResolver.Instance;

            if (interfaceComponentResolver.TryResolveTypeKeyDictionary<T>(this.PlugInContainerObject, out var plugInDic))
            {
                this.plugIns = plugInDic;
            }
            else
            {
                Debug.Log($"일치하는 {typeof(T).Name} 컴포넌트 없음.");
            }
        }

        public void UpdatePlugInAll()
        {
            foreach (var plugIn in plugIns.Values)
            {
                plugIn.UpdatePlugIn();
            }
        }
    }
}