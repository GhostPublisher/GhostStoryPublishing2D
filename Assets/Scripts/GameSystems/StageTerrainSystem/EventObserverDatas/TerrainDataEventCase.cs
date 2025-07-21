using System;
using System.Collections.Generic;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class InitialSetGeneratedTerrainDataEvent : IEventData
    {
        public int StageID;
    }

    [Serializable]
    public class InitialSetGeneratedTerrainDataEvent_Test : IEventData
    {
        public int Width;
        public int Height;
        public List<TerrainData> TerrainDatas;
    }

    [Serializable]
    public class ClearTrerrainData : IEventData { }
}