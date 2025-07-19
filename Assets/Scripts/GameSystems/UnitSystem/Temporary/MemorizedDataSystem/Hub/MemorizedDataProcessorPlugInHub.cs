using System;
using System.Collections.Generic;

using UnityEngine;
using Foundations.Architecture.EventObserver;

namespace GameSystems.UnitSystem
{
    public interface IMemorizedDataProcessorPlugIn
    {
        public void InitialSetting(IMemorizedDataGroup memorizedDataGroup);
        public void UpdateMemorizedData();
        public void UpdateMemorizedData(IEventData eventData);
        public void UpdateMemorizedData_TurnOver();
    }

    public class MemorizedDataProcessorPlugInHub : MonoBehaviour
    {
        [SerializeField] private GameObject InterfaceContainer;
        private Dictionary<Type, IMemorizedDataProcessorPlugIn> memorizedDataProcessorPlugIns;

        public void InitialSetting(IMemorizedDataGroup memorizedDataGroup)
        {
            this.memorizedDataProcessorPlugIns = new();

            foreach (var comp in this.InterfaceContainer.GetComponents<MonoBehaviour>())
            {
                if (comp is IMemorizedDataProcessorPlugIn plugIn)
                {
                    plugIn.InitialSetting(memorizedDataGroup);
                    this.memorizedDataProcessorPlugIns[plugIn.GetType()] = plugIn;
                }
            }
        }

        public void UpdateMemorizedDataAll()
        {
            foreach (var plugIn in this.memorizedDataProcessorPlugIns.Values)
            {
                plugIn.UpdateMemorizedData();
            }
        }

        public void UpdateMemorizedDataAll_TurnOver()
        {
            foreach (var plugIn in this.memorizedDataProcessorPlugIns.Values)
            {
                plugIn.UpdateMemorizedData_TurnOver();
            }
        }

        public void UpdateMemorizedData(Type memorizedDataPlugInType)
        {
            if (!this.memorizedDataProcessorPlugIns.TryGetValue(memorizedDataPlugInType, out var plugIn))
            {
                Debug.Log($"해당 플러그인 없음");
            }

            plugIn.UpdateMemorizedData();
        }

        public void UpdateMemorizedData(Type memorizedDataPlugInType, IEventData eventData)
        {
            if (!this.memorizedDataProcessorPlugIns.TryGetValue(memorizedDataPlugInType, out var plugIn))
            {
                Debug.Log($"해당 플러그인 없음");
            }

            plugIn.UpdateMemorizedData(eventData);
        }
    }
}