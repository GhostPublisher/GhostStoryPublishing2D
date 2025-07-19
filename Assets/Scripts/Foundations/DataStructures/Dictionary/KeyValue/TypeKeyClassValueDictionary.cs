using System;

namespace Foundations.DataStructures.Dictionary
{
    public interface IGetOrCreateProvider<TValue>
    {
        public T GetOrCreate<T>() where T : TValue, new();
    }

    /// <summary>
    /// Type을 키로 사용하는 DictionaryContainer입니다.
    /// 타입 기반 조회 및 등록 메서드를 제공합니다.
    /// </summary>
    public class TypeKeyClassValueDictionary<TValue> : KeyValueDictionary<Type, TValue>, IGetOrCreateProvider<TValue>
        where TValue : class
    {
        /// <summary>
        /// typeof(T) 키로 등록된 데이터를 조회합니다, 없으면 null 반환.
        /// </summary>
        public T Get<T>() where T : class, TValue
        {
            return base.Get(typeof(T)) as T;
        }
        /// <summary>값을 안전하게 가져오기. 있으면 true 반환</summary>
        public bool TryGet<T>(out T value) where T : class, TValue
        {
            if (base.TryGet(typeof(T), out var rawValue) && rawValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = null;
            return false;
        }
        /// <summary>
        /// T 타입의 데이터가 존재하면 반환하고, 없으면 생성하여 등록 후 반환합니다.
        /// </summary>
        public T GetOrCreate<T>() where T : TValue, new()
        {
            Type key = typeof(T);

            if (!this.TryGet(key, out var value))
            {
                value = new T();
                this.Set(key, value);
            }

            return (T)value;
        }
        /// <summary>
        /// typeof(T) 키를 기준으로 데이터를 등록합니다.
        /// </summary>
        public void Set<T>(T instance) where T : TValue
        {
            base.Set(typeof(T), instance);
        }

        /// <summary>
        /// typeof(T) 키에 해당하는 데이터를 제거합니다.
        /// </summary>
        public void RemoveKey<T>() where T : TValue
        {
            base.RemoveKey(typeof(T));
        }

        /// <summary>
        /// typeof(T) 키로 데이터가 등록되어 있는지 확인합니다.
        /// </summary>
        public bool ContainsKey<T>() where T : TValue
        {
            return base.ContainsKey(typeof(T));
        }
    }
}