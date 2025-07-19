using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/Player/PlayerUnitResourceDataGroup", fileName = "PlayerUnitResourceDataGroup")]
    public class PlayerUnitResourceDataGroup : ScriptableObject
    {
        [SerializeField] private List<PlayerUnitResourceData> PlayerUnitImageResourceDatas;

        public bool TryGetPlayerUnitResourceData(int unitID, out PlayerUnitResourceData playerUnitResourceData)
        {
            playerUnitResourceData = null;

            foreach (var data in this.PlayerUnitImageResourceDatas)
            {
                if(data.UnitID == unitID)
                {
                    playerUnitResourceData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class PlayerUnitResourceData
    {
        [SerializeField] public int UnitID;
        [SerializeField] public GameObject UnitPrefab;
        [SerializeField] public Sprite UnitSpriteImage;
    }
}