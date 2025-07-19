using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.HashSet
{
    /// <summary>
    /// 단일 HashSet<TValue> 기반의 데이터 컨테이너입니다.
    /// </summary>
    [Serializable]
    public class Data_HashSetContainer<TValue>
    {
        protected readonly HashSet<TValue> datas = new();

        /// <summary>전체 요소 반환</summary>
        public virtual HashSet<TValue> GetAll()
        {
            return this.datas;
        }

        /// <summary>요소 추가</summary>
        public virtual bool Add(TValue value)
        {
            return this.datas.Add(value); // 중복 방지
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