using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemySpawnSystem;
using GameSystems.EnemySystem.EnemyAISequenceSystem;

namespace GameSystems.EnemySystem
{
    public class EnemySystemManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawnController EnemySpawnController;

        [SerializeField] private EnemyAISequencer EnemyAISequencer;


    }
}
