using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitEffectController : MonoBehaviour, IEnemyUnitEffectController
    {
        private EffectSpriteDataDBHandler EffectSpriteDataDBHandler;

        [SerializeField] private List<EffectPositionMarkerData> EffectPositionMarkerDatas;

        [SerializeField] private GameObject EffectControllerPrefab;

        private EnemyUnitManagerData myEnemyUnitManagerData;

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.EffectSpriteDataDBHandler = HandlerManager.GetStaticDataHandler<EffectSpriteDataDBHandler>();

            this.myEnemyUnitManagerData = enemyUnitManagerData;
        }

        // Effect Prefab을 생성 후 위치값과 애니메이션으로 진행할 sprite[]를 전달.
        public IEnumerator OperateEffect(int effectID)
        {
            // EffectID에 대응되는 SO 값이 없을 때 발생. 오류
            if (!this.EffectSpriteDataDBHandler.TryGetEffectSpriteData(effectID, out var effectSpriteData)) yield break;

            EffectSpriteController effectSpriteController = Instantiate(EffectControllerPrefab).GetComponent<EffectSpriteController>();
            Vector3 effectPosition = this.GetEffectPosition(effectSpriteData.EffectPositionMarker);

            yield return StartCoroutine(effectSpriteController.PlayEffect(effectPosition, effectSpriteData.EffectFrames, effectSpriteData.FrameInterval));
        }

        // Unit마다 다른 Effect위치를 지정.
        private Vector3 GetEffectPosition(EffectPositionMarker effectPositionMarker)
        {
            foreach(var data in this.EffectPositionMarkerDatas)
            {
                if (data.EffectPositionMarker == effectPositionMarker)
                    return this.myEnemyUnitManagerData.EnemyUnitTransform.position + data.EffectPosition;
            }

            return Vector3.zero;
        }
    }
  
    [Serializable]
    public class EffectPositionMarkerData
    {
        public EffectPositionMarker EffectPositionMarker;
        public Vector3 EffectPosition;
    }

    [Serializable]
    public enum EffectPositionMarker
    {
        AboveHead,
        Head,
        Chest,
        Legs,
        UnderFoot
    }
}