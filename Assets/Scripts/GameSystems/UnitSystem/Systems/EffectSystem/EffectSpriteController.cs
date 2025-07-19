using System.Collections;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    // 이건 SpriteRenderer만 갖는 GameOBject로 표현.
    public class EffectSpriteController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer EffectSpriteRenderer;
        
        public IEnumerator PlayEffect(Vector3 effectPosition, Sprite[] effectFrames, int FrameInterval)
        {
            this.gameObject.transform.position = effectPosition;

            foreach (var frame in effectFrames)
            {
                EffectSpriteRenderer.sprite = frame;

                for (int i = 0; i < FrameInterval; i++)
                    yield return null; // 프레임 기준 대기
            }

            // 이펙트 종료 처리 (파괴 또는 Pool 반환)
            Destroy(gameObject);
        }
    }
}