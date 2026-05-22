using UnityEngine;

namespace Presentation.AI
{
    public class RangedAttackState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;

        private float _cooldown = 2f;
        private float _timer;

        public RangedAttackState(
            RangedEnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = 0f;

            _behaviour.EnterRangedAttack();
        }

        public void Update()
        {
            if (_behaviour.ShouldStopRangedAttack())
            {
                ((RangedEnemyStateMachine)_behaviour.StateMachine)
                    .EnterMaintainDistance(_behaviour);

                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _behaviour.UpdateRangedAttack();

                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            _behaviour.ExitRangedAttack();
        }
    }
}