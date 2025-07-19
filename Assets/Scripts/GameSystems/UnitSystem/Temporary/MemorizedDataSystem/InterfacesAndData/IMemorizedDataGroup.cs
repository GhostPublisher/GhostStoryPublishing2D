using System;
using System.Collections.Generic;

namespace GameSystems.UnitSystem
{
    public interface IMemorizedDataGroup
    {
        public Dictionary<Type, IDirectDetectionData> DirectDetectionDatas { get; }
        public Dictionary<Type, IMemorizedData> MemorizedDatas { get; }
    }

    public interface IMemorizedData { }
}