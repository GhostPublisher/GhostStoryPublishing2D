using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.Dictionary
{
    /// <summary>
    /// 키(Type)와 값(List<TValue>)의 매핑을 제공하는 딕셔너리
    /// </summary>
    [Serializable]
    public class KeyValueListDictionary<TKey, TValue> where TKey : notnull
    {
        protected readonly Dictionary<TKey, List<TValue>> container = new();

        /// <summary>리스트 전체 가져오기</summary>
        public List<TValue> Get(TKey key)
        {
            this.container.TryGetValue(key, out var value);

            // value가 null이면 new List<TValue>() 반환.
            // null이 아니면 value 반환.
            return value ?? new List<TValue>();
        }

        /// <summary>리스트 안전하게 가져오기</summary>
        public bool TryGet(TKey key, out List<TValue> list)
        {
            return this.container.TryGetValue(key, out list);
        }

        /// <summary>리스트 전체 교체</summary>
        public void Set(TKey key, List<TValue> values)
        {
            this.container[key] = values;
        }

        /// <summary>값 하나 추가</summary>
        public void Add(TKey key, TValue value)
        {
            if (this.container.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
        }

        /// <summary>리스트에서 특정 값 제거</summary>
        public bool Remove(TKey key, TValue value)
        {
            return this.container.TryGetValue(key, out var list) && list.Remove(value);
        }

        /// <summary>Key 전체 제거</summary>
        public bool RemoveKey(TKey key)
        {
            return this.container.Remove(key);
        }

        /// <summary>Key 존재 여부</summary>
        public bool ContainsKey(TKey key)
        {
            return this.container.ContainsKey(key);
        }
    }
}