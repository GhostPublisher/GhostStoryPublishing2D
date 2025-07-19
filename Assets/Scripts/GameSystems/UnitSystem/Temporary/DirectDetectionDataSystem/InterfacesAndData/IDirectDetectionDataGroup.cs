using System;
using System.Collections.Generic;

namespace GameSystems.UnitSystem
{
    public interface IDirectDetectionDataGroup
    {
        public Dictionary<Type, IInteractionRangeData> InteractionRangeDatas { get; }
        public Dictionary<Type, IDirectDetectionData> DirectDetectionDatas { get; }
    }

    public interface IDirectDetectionData { }
}