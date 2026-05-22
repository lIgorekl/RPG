using UnityEngine;

namespace Presentation.AI
{
    public class AttackState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;
        private readonly MeleeEnemyStateMachine _stateMachine;

        private float _cooldown = 1f;
        private float _timer;

        public AttackState(
            EnemyBehaviour behaviour,
            MeleeEnemyStateMachine stateMachine)
        {
            _behaviour = behaviour;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = 0f;

            _behaviour.EnterAttack();
        }

        public void Update()
        {
            if (_behaviour.ShouldStopAttack())
            {
                _stateMachine.EnterChase(_behaviour);
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _behaviour.UpdateAttack();

                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            _behaviour.ExitAttack();
        }
    }
}