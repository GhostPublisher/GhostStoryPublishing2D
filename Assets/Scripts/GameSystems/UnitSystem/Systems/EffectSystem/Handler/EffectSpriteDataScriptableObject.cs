using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/Effect/EffectSpriteDataScriptableObject", fileName = "EffectSpriteDataScriptableObject")]
    public class EffectSpriteDataScriptableObject : ScriptableObject
    {
        [SerializeField ] private List<EffectSpriteData> EffectSpriteDatas;

        public bool TryGetEffectSpriteData(int EffectID, out EffectSpriteData effectSpriteData)
        {
            effectSpriteData = null;
            if (this.EffectSpriteDatas == null) return false; 

            foreach(var data in this.EffectSpriteDatas)
            {
                if (data.EffectID == EffectID)
                {
                    effectSpriteData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class EffectSpriteData
    {
        public int EffectID;
        public EffectPositionMarker EffectPositionMarker;
        public Sprite[] EffectFrames;
        public int FrameInterval;
    }
}