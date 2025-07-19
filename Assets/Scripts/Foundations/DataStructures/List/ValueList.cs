using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.List
{
    /// <summary>
    /// 단일 List<TValue> 기반의 데이터 컨테이너입니다.
    /// </summary>
    [Serializable]
    public class ValueList<TValue>
    {
        protected readonly List<TValue> datas = new();

        /// <summary>전체 요소 반환</summary>
        public virtual List<TValue> GetAll()
        {
            return this.datas;
        }

        /// <summary>요소 추가</summary>
        public virtual void Add(TValue value)
        {
            this.datas.Add(value);
        }

        /// <summary>요소 제거</summary>
        public virtual bool Remove(TValue value)
        {
            return this.datas.Remove(value);
        }

        /// <summary>비우기</summary>
        public virtual void Clear()
        {
            this.datas.Clear();
        }

        /// <summary>포함 여부</summary>
        public virtual bool Contains(TValue value)
        {
            return this.datas.Contains(value);
        }

        /// <summary>개수 반환</summary>
        public virtual int Count()
        {
            return this.datas.Count;
        }
    }
}