using UnityEngine;

namespace Presentation.AI
{
    public class RangedIdleState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;
        private readonly RangedEnemyStateMachine _stateMachine;

        private float _wanderTimer;
        private float _wanderDelay = 2f;

        public RangedIdleState(
            RangedEnemyBehaviour behaviour,
            RangedEnemyStateMachine stateMachine)
        {
            _behaviour = behaviour;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            if (_behaviour.ShouldMaintainDistance())
            {
                _stateMachine.EnterMaintainDistance(_behaviour);
                return;
            }

            if (_behaviour.ShouldFlee())
            {
                if (_behaviour.CombatEvaluator.IsTargetDetected(
                    _behaviour.Self,
                    _behaviour.Player,
                    _behaviour.DetectionRadius))
                {
                    _stateMachine.EnterFlee(_behaviour);
                    return;
                }
            }

            _behaviour.UpdateIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit() { }
    }
}