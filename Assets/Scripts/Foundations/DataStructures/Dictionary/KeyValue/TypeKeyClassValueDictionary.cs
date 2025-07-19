using System;

namespace Foundations.DataStructures.Dictionary
{
    public interface IGetOrCreateProvider<TValue>
    {
        public T GetOrCreate<T>() where T : TValue, new();
    }

    /// <summary>
    /// Type�� Ű�� ����ϴ� DictionaryContainer�Դϴ�.
    /// Ÿ�� ��� ��ȸ �� ��� �޼��带 �����մϴ�.
    /// </summary>
    public class TypeKeyClassValueDictionary<TValue> : KeyValueDictionary<Type, TValue>, IGetOrCreateProvider<TValue>
        where TValue : class
    {
        /// <summary>
        /// typeof(T) Ű�� ��ϵ� �����͸� ��ȸ�մϴ�, ������ null ��ȯ.
        /// </summary>
        public T Get<T>() where T : class, TValue
        {
            return base.Get(typeof(T)) as T;
        }
        /// <summary>���� �����ϰ� ��������. ������ true ��ȯ</summary>
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
        /// T Ÿ���� �����Ͱ� �����ϸ� ��ȯ�ϰ�, ������ �����Ͽ� ��� �� ��ȯ�մϴ�.
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
        /// typeof(T) Ű�� �������� �����͸� ����մϴ�.
        /// </summary>
        public void Set<T>(T instance) where T : TValue
        {
            base.Set(typeof(T), instance);
        }

        /// <summary>
        /// typeof(T) Ű�� �ش��ϴ� �����͸� �����մϴ�.
        /// </summary>
        public void RemoveKey<T>() where T : TValue
        {
            base.RemoveKey(typeof(T));
        }

        /// <summary>
        /// typeof(T) Ű�� �����Ͱ� ��ϵǾ� �ִ��� Ȯ���մϴ�.
        /// </summary>
        public bool ContainsKey<T>() where T : TValue
        {
            return base.ContainsKey(typeof(T));
        }
    }
}