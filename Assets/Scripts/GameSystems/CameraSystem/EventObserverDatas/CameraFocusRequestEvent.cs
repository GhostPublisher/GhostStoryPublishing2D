using System;
using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.CameraSystem
{
    [Serializable]
    public class CameraFocusRequestEvent : IEventData
    {
        public Vector2Int FocusGridPosition;
    }
}