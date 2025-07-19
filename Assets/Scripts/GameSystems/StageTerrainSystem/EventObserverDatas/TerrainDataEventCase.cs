using System;
using System.Collections.Generic;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class InitialSetGeneratedTerrainDataEvent : IEventData
    {
        public int Width;
        public int Height;
    }

    [Serializable]
    public class AdditionalSetGeneratedTerrainDataEvent_Test : IEventData
    {
        public List<TerrainData> TerrainDatas;
    }

    [Serializable]
    public class ClearTrerrainData : IEventData { }
}