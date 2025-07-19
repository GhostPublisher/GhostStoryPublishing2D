using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.Dictionary
{
    /// <summary>
    /// 일반적인 Key-Value 기반 Map 저장소 (Key: TKey, Value: TValue)
    /// </summary>
    [Serializable]
    public class KeyValueDictionary<TKey, TValue> where TKey : notnull
    {
        protected readonly Dictionary<TKey, TValue> container = new Dictionary<TKey, TValue>();

        /// <summary>값 가져오기. 없으면 기본값</summary>
        public virtual TValue Get(TKey key)
        {
            this.container.TryGetValue(key, out var value);
            return value;
        }
        /// <summary>값을 안전하게 가져오기. 있으면 true 반환</summary>
        public virtual bool TryGet(TKey key, out TValue value)
        {
            return this.container.TryGetValue(key, out value);
        }

        /// <summary>값 추가 또는 수정</summary>
        public virtual void Set(TKey key, TValue value)
        {
            this.container[key] = value;
        }

        /// <summary>값 제거</summary>
        public virtual bool RemoveKey(TKey key)
        {
            return this.container.Remove(key);
        }

        /// <summary>해당 Key 포함 여부</summary>
        public virtual bool ContainsKey(TKey key)
        {
            return this.container.ContainsKey(key);
        }
    }
}