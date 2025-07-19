using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EffectSpriteDataDBHandler : IStaticReferenceHandler
    {
        private EffectSpriteDataScriptableObject EffectSpriteDataScriptableObject;

        public EffectSpriteDataDBHandler()
        {
            this.LoadScriptableObject();
        }
        private void LoadScriptableObject()
        {
            // EffectSpriteDataScriptableObject
            this.EffectSpriteDataScriptableObject = Resources.Load<EffectSpriteDataScriptableObject>("ScriptableObject/Effect/EffectSpriteDataScriptableObject");

            if (this.EffectSpriteDataScriptableObject == null)
            {
                Debug.LogError("[EffectSpriteDataScriptableObject] EffectSpriteDataScriptableObject.asset을 찾을 수 없습니다. 경로 또는 파일 확인 필요.");
            }
        }

        public bool TryGetEffectSpriteData(int effectID, out EffectSpriteData effectSpriteData)
        {
            effectSpriteData = null;
            if (this.EffectSpriteDataScriptableObject == null) return false;

            return this.EffectSpriteDataScriptableObject.TryGetEffectSpriteData(effectID, out effectSpriteData);
        }
    }
}