using System;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// 제네릭 기반의 상속용 싱글톤 클래스.
    /// private 생성자도 지원하며, 최초 접근 시 OnInitialize() 호출.
    /// </summary>
    public abstract class Singleton<T> where T : class
    {
        /// <summary>
        /// Lazy<T>를 사용하여 thread-safe하며, 처음 Instance에 접근할 때만 생성된다.
        /// </summary>
        private static readonly Lazy<T> lazyInstance = new(() =>
        {
            // private 매개변수 없는 생성자 호출
            T instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true)!;

            // 초기화 메서드가 있다면 호출
            if (instance is Singleton<T> singleton)
            {
                singleton.OnInitialize();
            }

            return instance;
        });

        /// <summary>
        /// 외부에서 접근 가능한 싱글 인스턴스.
        /// </summary>
        public static T Instance => lazyInstance.Value;

        /// <summary>
        /// 외부 생성 차단용 기본 생성자.
        /// 반드시 private 또는 protected로 선언해야 함.
        /// </summary>
        protected Singleton() { }

        /// <summary>
        /// 인스턴스 생성 직후 1회 호출되는 초기화 메서드.
        /// 상속 클래스에서 override 가능.
        /// </summary>
        protected virtual void OnInitialize() { }
    }
}