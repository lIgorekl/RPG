using UnityEngine;

namespace Presentation.AI
{
    public class AttackState : IEnemyState
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        private float _cooldown = 1f;
        private float _timer;

        public AttackState(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = 0f;

            _stateMachine.EnemyBehaviour.EnterAttack();
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.EnemyBehaviour;

            if (behaviour.ShouldStopAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                behaviour.UpdateAttack();

                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            _stateMachine.EnemyBehaviour.ExitAttack();
        }
    }
}
