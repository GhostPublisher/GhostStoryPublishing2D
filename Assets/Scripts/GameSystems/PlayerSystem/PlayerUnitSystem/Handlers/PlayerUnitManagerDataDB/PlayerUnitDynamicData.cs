using System;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    [Serializable]
    public class PlayerUnitDynamicData
    {
        [SerializeField] public int UniqueID;

        [SerializeField] public int CurrentMoveCost;
        [SerializeField] public int CurrentSkillCost;
        [SerializeField] public int CurrentHPCost;
    }

}