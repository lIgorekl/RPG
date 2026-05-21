using UnityEngine;
using App.Services;

namespace Presentation.AI
{
    public class StunState : IEnemyState
    {
        private BaseEnemyBehaviour _behaviour;
        private float _timer;

        public StunState(BaseEnemyBehaviour behaviour, float duration)
        {
            _behaviour = behaviour;
            _timer = duration;
        }

        public void Enter()
        {
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = true;
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.CreateDefaultState());
            }
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}