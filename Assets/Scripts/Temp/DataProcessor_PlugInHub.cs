using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.InterfaceResolver;

namespace GameSystems.UnitSystem
{
    public interface IDataProcessorPlugInHub<TData>
    {
        public void InitialSetting(TData data);
        public void UpdateData<T>(Vector2Int currentPosition) where T : class;
        public void UpdateDataAll(Vector2Int currentPosition);
    }

    public interface IDataProcessorPlugIn<TData>
    {
        public void UpdateData(Vector2Int currentPosition);
        public void InitialSetting(TData data);
    }

    [Serializable]
    public class DataProcessor_PlugInHub<TPlugIn, TData> : IDataProcessorPlugInHub<TData> where TPlugIn : class, IDataProcessorPlugIn<TData>
    {
        [SerializeField] private GameObject plugInContainerObject;
        private Dictionary<Type, TPlugIn> dataProcessorPlugIns;

        public void Init()
        {
            this.AllocatePlugInAll();
        }

        private void AllocatePlugInAll()
        {
            var resolver = InterfaceComponentResolver.Instance;

            if(this.plugInContainerObject == null)
            {
//                Debug.Log($"명시된 plugInContainerObject가 없음.");
                return;
            }

            if (resolver.TryResolveTypeKeyDictionary<TPlugIn>(this.plugInContainerObject, out var plugInDic))
            {
                this.dataProcessorPlugIns = plugInDic;
            }
            else
            {
                Debug.Log($"일치하는 {typeof(TPlugIn).Name} 컴포넌트 없음.");
                this.dataProcessorPlugIns = new(); // 안전하게 비워두기
            }
        }

        public void InitialSetting(TData data)
        {
            foreach (var plugIn in this.dataProcessorPlugIns.Values)
            {
                plugIn.InitialSetting(data);
            }
        }

        public void UpdateDataAll(Vector2Int currentPosition)
        {
            foreach (var plugIn in this.dataProcessorPlugIns.Values)
            {
                plugIn.UpdateData(currentPosition);
            }
        }

        public void UpdateData<T>(Vector2Int currentPosition) where T : class
        {
            Type plugInType = typeof(T);

            if (this.dataProcessorPlugIns.TryGetValue(plugInType, out var value))
            {
                value.UpdateData(currentPosition);
            }
            else
            {
                Debug.Log($"일치하는 {typeof(T).Name} PlugIn이 없음.");
            }
        }
    }
}