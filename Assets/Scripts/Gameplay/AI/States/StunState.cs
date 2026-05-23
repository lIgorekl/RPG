using UnityEngine;

namespace Presentation.AI
{
    public class StunState : IEnemyState
    {
        private readonly EnemyStateMachineBase _stateMachine;

        private BaseCombatEnemyBehaviour Behaviour =>
            (BaseCombatEnemyBehaviour)_stateMachine.Behaviour;

        private float _timer;

        public StunState(
            EnemyStateMachineBase stateMachine,
            float duration)
        {
            _stateMachine = stateMachine;
            _timer = duration;
        }

        public void Enter()
        {
            Behaviour.EnterStun();
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _stateMachine.EnterDefaultState();
            }
        }

        public void Exit()
        {
            Behaviour.ExitStun();
        }
    }
}
