using UnityEngine;

namespace Presentation.AI
{
    public class AttackState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;

        private float _cooldown = 1f;
        private float _timer;

        public AttackState(
            EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
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
                ((MeleeEnemyStateMachine)_behaviour.StateMachine)
                    .EnterChase(_behaviour);

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