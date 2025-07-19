using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.List
{
    /// <summary>
    /// ���� List<TValue> ����� ������ �����̳��Դϴ�.
    /// </summary>
    [Serializable]
    public class ValueList<TValue>
    {
        protected readonly List<TValue> datas = new();

        /// <summary>��ü ��� ��ȯ</summary>
        public virtual List<TValue> GetAll()
        {
            return this.datas;
        }

        /// <summary>��� �߰�</summary>
        public virtual void Add(TValue value)
        {
            this.datas.Add(value);
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