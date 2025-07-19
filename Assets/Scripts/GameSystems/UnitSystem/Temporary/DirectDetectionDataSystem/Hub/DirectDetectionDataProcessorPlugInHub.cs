using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.UnitSystem
{
    public interface IDirectDetectionDataProcessorPlugIn
    {
        public void InitialSetting(IDirectDetectionDataGroup directDetectionDataGroup);
        public void UpdateDirectDetectionData();
    }

    public class DirectDetectionDataProcessorPlugInHub : MonoBehaviour
    {
        [SerializeField] private GameObject InterfaceContainer;
        private Dictionary<Type, IDirectDetectionDataProcessorPlugIn> directDetectionDataProcessorPlugIns;

        public void InitialSetting(IDirectDetectionDataGroup directDetectionDataGroup)
        {
            this.directDetectionDataProcessorPlugIns = new();

            foreach (var comp in this.InterfaceContainer.GetComponents<MonoBehaviour>())
            {
                if (comp is IDirectDetectionDataProcessorPlugIn plugIn)
                {
                    plugIn.InitialSetting(directDetectionDataGroup);
                    this.directDetectionDataProcessorPlugIns[plugIn.GetType()] = plugIn;
                }
            }
        }

        public void UpdateDirectDetectionDataAll()
        {
            foreach (var plugIn in this.directDetectionDataProcessorPlugIns.Values)
            {
                plugIn.UpdateDirectDetectionData();
            }
        }

        public void UpdateDirectDetectionData(Type directDetectedDataProcessorPlugInType)
        {
            if (!this.directDetectionDataProcessorPlugIns.TryGetValue(directDetectedDataProcessorPlugInType, out var plugIn))
            {
                Debug.Log($"해당 플러그인 없음");
            }

            plugIn.UpdateDirectDetectionData();
        }
    }
}