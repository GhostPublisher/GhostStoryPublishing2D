using System;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.UnitSystem
{
    public interface EnemyIInteractionRangeProcessorPlugIn
    {
        public void InitialSetting(EnemyUnitManagerData generatedEnemyUnitData);
        public void UpdateInteractionRange(Vector2Int currentPosition);
    }

    public class EnemyInteractionRangeProcessorHub : MonoBehaviour
    {
        [SerializeField] private GameObject InterfaceContainer;
        private Dictionary<Type, EnemyIInteractionRangeProcessorPlugIn> interactionRangeProcessorPlugIns;

        public void InitialSetting(EnemyUnitManagerData generatedEnemyUnitData)
        {
            this.interactionRangeProcessorPlugIns = new();

            foreach (var comp in this.InterfaceContainer.GetComponents<MonoBehaviour>())
            {
                if (comp is EnemyIInteractionRangeProcessorPlugIn plugIn)
                {
                    plugIn.InitialSetting(generatedEnemyUnitData);
                    this.interactionRangeProcessorPlugIns[plugIn.GetType()] = plugIn;
                }
            }
        }

        public void UpdateInteractionRangeAll(Vector2Int currentPosition)
        {
            foreach(var plugIn in this.interactionRangeProcessorPlugIns.Values)
            {
                plugIn.UpdateInteractionRange(currentPosition);
            }
        }

        public void UpdateInteractionRange(Type interactionType, Vector2Int currentPosition)
        {
            if (!this.interactionRangeProcessorPlugIns.TryGetValue(interactionType, out var plugIn))
            {
                Debug.Log($"해당 플러그인 없음");
            }

            plugIn.UpdateInteractionRange(currentPosition);
        }

    }
}