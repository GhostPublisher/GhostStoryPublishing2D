using System;
using System.Collections.Generic;

using UnityEngine;

namespace Foundations.Architecture.ReferencesHandler
{
    public class LazyReferenceHandlerGroup<HandlerType> where HandlerType : class
    {
        private Dictionary<Type, HandlerType> handlerContainer;
        private GameObject monoHandlerRootObject;

        public LazyReferenceHandlerGroup(string monoRootObjectName = "UtilityHandlers")
        {
            this.handlerContainer = new();

            this.monoHandlerRootObject = GameObject.Find(monoRootObjectName);
            if (this.monoHandlerRootObject == null)
            {
                this.monoHandlerRootObject = new GameObject(monoRootObjectName);
                UnityEngine.Object.DontDestroyOnLoad(this.monoHandlerRootObject);
            }
        }

        public T GetOrCreate<T>() where T : HandlerType, new()
        {
            Type key = typeof(T);

            if (!this.handlerContainer.TryGetValue(key, out var value))
            {
                value = new T();
                this.handlerContainer[key] = value;
            }

            return (T)value;
        }

        public T GetOrAddMonoBehaviour<T>() where T : MonoBehaviour, HandlerType
        {
            Type key = typeof(T);

            if (!handlerContainer.TryGetValue(key, out var value))
            {
                value = monoHandlerRootObject.GetComponent<T>();
                if (value == null)
                    value = monoHandlerRootObject.AddComponent<T>();

                handlerContainer[key] = value;
            }

            return (T)value;
        }
    }
}