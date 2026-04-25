using UnityEngine;

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
                if (_behaviour is EnemyBehaviour melee)
                {
                    _behaviour.StateMachine.ChangeState(new IdleState(melee));
                }
                else if (_behaviour is RangedEnemyBehaviour ranged)
                {
                    _behaviour.StateMachine.ChangeState(new RangedIdleState(ranged));
                }
            }
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}