using System;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyAISequenceSystem
{
    // Enemy Turn의 시작 시, Enemy AI 순서 재 할당.
    [Serializable]
    public class AllocateEnemyAISequence : IEventData { }

    // Enemy AI 실행 및 다음 Enemy AI 실행.
    [Serializable]
    public class ExecuteEnemyAI : IEventData { }

    // 새로운 Turn이 시작되었을 떄, 각 Enemy들의 EnemyAISequence완 연관된 데이터 초기화
    [Serializable]
    public class OperateNewTurnSetting : IEventData { }

    // 불특정 이유로 인해 종료된 EnemyAI 흐름에 재 진입.
    [Serializable]
    public class CurrentOperatedEnemy_OperationContinue : IEventData { }
}