using UnityEngine;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// MonoBehaviour 기반의 싱글톤 클래스.
    /// 씬 내에 단 하나만 존재해야 하며, 중복 시 자동 제거 또는 보존 처리가 가능함.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        /// <summary>
        /// 싱글 인스턴스 접근 프로퍼티
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // 씬에 존재하는 인스턴스를 찾거나,
                    instance = FindObjectOfType<T>();

                    // 없으면 경고 표시 (자동 생성은 하지 않음)
                    if (instance == null)
                        Debug.LogError($"[MonoSingleton<{typeof(T).Name}>] Instance not found in scene.");
                }

                return instance;
            }
        }

        /// <summary>
        /// 싱글톤 인스턴스 할당 및 중복 방지
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                OnInitialize();
            }
            else if (instance != this)
            {
                Debug.LogWarning($"[MonoSingleton<{typeof(T).Name}>] Duplicate detected, destroying: {gameObject.name}");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 초기화 훅 (선택적으로 override 가능)
        /// </summary>
        protected virtual void OnInitialize() { }
    }
}