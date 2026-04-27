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
                if (_behaviour is EnemyBehaviour melee2)
                {
                    _behaviour.StateMachine.ChangeState(new IdleState(melee2));
                }
                else if (_behaviour is RangedEnemyBehaviour ranged)
                {
                    _behaviour.StateMachine.ChangeState(new RangedIdleState(ranged));
                }
                else if (_behaviour is BossBehaviour boss)
                {
                    _behaviour.StateMachine.ChangeState(new BossIdleState(boss));
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