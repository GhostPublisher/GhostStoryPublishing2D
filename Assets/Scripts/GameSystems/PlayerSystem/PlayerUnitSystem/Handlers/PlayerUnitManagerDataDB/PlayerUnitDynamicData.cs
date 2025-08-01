using System;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    [Serializable]
    public class PlayerUnitDynamicData
    {
        [SerializeField] public int UniqueID;

        [SerializeField] public int HPCost_Current;
        [SerializeField] public int BehaviourCost_Current;
    }

}