using System.Collections.Generic;

namespace Foundations.Architecture.ReferencesHandler
{
    public class LazyHandler_DataExample : IStaticReferenceHandler
    {
        public string Name;
    }

    public class LazyHandler_DataGroupExample : IStaticReferenceHandler
    {
        public List<LazyHandler_DataExample> lists;
    }

    public class HandlerUsing_Example
    {
        private LazyReferenceHandlerManager LazyReferenceHandlerManager;

        private LazyHandler_DataExample LazyHandler_DataExample;
        private LazyHandler_DataGroupExample LazyHandler_DataGroupExample;

        public HandlerUsing_Example()
        {
            this.LazyReferenceHandlerManager = LazyReferenceHandlerManager.Instance;
        }

        private void GetHandler()
        {
            this.LazyHandler_DataExample = this.LazyReferenceHandlerManager.GetStaticDataHandler<LazyHandler_DataExample>();
            this.LazyHandler_DataGroupExample = this.LazyReferenceHandlerManager.GetStaticDataHandler<LazyHandler_DataGroupExample>();
        }
    }
}