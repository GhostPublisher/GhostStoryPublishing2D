using Foundations.Patterns.Singleton;

using UnityEngine;

namespace Foundations.Architecture.ReferencesHandler
{
    public class LazyReferenceHandlerManager : Singleton<LazyReferenceHandlerManager>
    {
        private LazyReferenceHandlerGroup<IStaticReferenceHandler> StaticLazyReferenceHandlerGroup;
        private LazyReferenceHandlerGroup<IDynamicReferenceHandler> DynamicLazyReferenceHandlerGroup;
        private LazyReferenceHandlerGroup<IUtilityReferenceHandler> UtilityLazyReferenceHandlerGroup;

        /// <summary>
        /// 외부 생성 차단용 기본 생성자.
        /// </summary>
        protected LazyReferenceHandlerManager() { }

        // Singleton에서 호출함.
        protected override void OnInitialize()
        {
            this.StaticLazyReferenceHandlerGroup = new();
            this.DynamicLazyReferenceHandlerGroup = new();
            this.UtilityLazyReferenceHandlerGroup = new();
        }

        public T GetStaticDataHandler<T>() where T : IStaticReferenceHandler, new()
        {
            return StaticLazyReferenceHandlerGroup.GetOrCreate<T>();
        }

        public T GetDynamicDataHandler<T>() where T : IDynamicReferenceHandler, new()
        {
            return DynamicLazyReferenceHandlerGroup.GetOrCreate<T>();
        }

        public T GetUtilityHandler<T>() where T : IUtilityReferenceHandler, new()
        {
            return UtilityLazyReferenceHandlerGroup.GetOrCreate<T>();
        }

        public T GetUtilityComponentHandler<T>() where T : MonoBehaviour, IUtilityReferenceHandler
        {
            return UtilityLazyReferenceHandlerGroup.GetOrAddMonoBehaviour<T>();
        }
    }

    public interface IStaticReferenceHandler { }
    public interface IDynamicReferenceHandler { }
    public interface IUtilityReferenceHandler { }
}