using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.Dictionary
{
    /// <summary>
    /// �Ϲ����� Key-Value ��� Map ����� (Key: TKey, Value: TValue)
    /// </summary>
    [Serializable]
    public class KeyValueDictionary<TKey, TValue> where TKey : notnull
    {
        protected readonly Dictionary<TKey, TValue> container = new Dictionary<TKey, TValue>();

        /// <summary>�� ��������. ������ �⺻��</summary>
        public virtual TValue Get(TKey key)
        {
            this.container.TryGetValue(key, out var value);
            return value;
        }
        /// <summary>���� �����ϰ� ��������. ������ true ��ȯ</summary>
        public virtual bool TryGet(TKey key, out TValue value)
        {
            return this.container.TryGetValue(key, out value);
        }

        /// <summary>�� �߰� �Ǵ� ����</summary>
        public virtual void Set(TKey key, TValue value)
        {
            this.container[key] = value;
        }

        /// <summary>�� ����</summary>
        public virtual bool RemoveKey(TKey key)
        {
            return this.container.Remove(key);
        }

        /// <summary>�ش� Key ���� ����</summary>
        public virtual bool ContainsKey(TKey key)
        {
            return this.container.ContainsKey(key);
        }
    }
}