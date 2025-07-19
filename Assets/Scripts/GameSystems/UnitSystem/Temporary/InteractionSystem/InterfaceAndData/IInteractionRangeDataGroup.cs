using System;
using System.Collections.Generic;

namespace GameSystems.UnitSystem
{
    public interface IInteractionRangeDataGroup
    {
        public Dictionary<Type, IInteractionRangeData> InteractionRangeDatas { get; }
    }

    public interface IInteractionRangeData { }
}