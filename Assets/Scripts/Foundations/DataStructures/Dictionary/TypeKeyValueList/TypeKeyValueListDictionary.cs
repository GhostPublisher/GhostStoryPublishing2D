using System;
using System.Collections.Generic;

namespace Foundations.DataStructures.Dictionary
{
    public class TypeKeyValueListDictionary<TValue> : KeyValueListDictionary<Type, TValue> where TValue : class
    {
        /// <summary>리스트 전체 가져오기</summary>
        public List<TValue> GetList<T>() where T : class
        {
            return base.Get(typeof(T));
        }
        /// <summary>값을 안전하게 가져오기. 있으면 true 반환</summary>
        public bool TryGetList<T>(out List<TValue> list) where T : class
        {
            if(base.TryGet(typeof(T), out var rawList) && rawList != null)  
            {
                list = rawList;
                return true;
            }

            list = null;
            return false;
        }

        /// <summary>리스트 전체 교체</summary>
        public void SetList<T>() where T : class
        {
            base.Set(typeof(T), new List<TValue>());
        }
        /// <summary>값 하나 추가</summary>
        public void Add<T>(TValue value) where T : class
        {
            base.Add(typeof(T), value);
        }
    }
}