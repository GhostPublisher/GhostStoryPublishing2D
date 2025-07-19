using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.HashSet
{
    /// <summary>
    /// ���� HashSet<TValue> ����� ������ �����̳��Դϴ�.
    /// </summary>
    [Serializable]
    public class Data_HashSetContainer<TValue>
    {
        protected readonly HashSet<TValue> datas = new();

        /// <summary>��ü ��� ��ȯ</summary>
        public virtual HashSet<TValue> GetAll()
        {
            return this.datas;
        }

        /// <summary>��� �߰�</summary>
        public virtual bool Add(TValue value)
        {
            return this.datas.Add(value); // �ߺ� ����
        }

        /// <summary>��� ����</summary>
        public virtual bool Remove(TValue value)
        {
            return this.datas.Remove(value);
        }

        /// <summary>����</summary>
        public virtual void Clear()
        {
            this.datas.Clear();
        }

        /// <summary>���� ����</summary>
        public virtual bool Contains(TValue value)
        {
            return this.datas.Contains(value);
        }

        /// <summary>���� ��ȯ</summary>
        public virtual int Count()
        {
            return this.datas.Count;
        }
    }
}