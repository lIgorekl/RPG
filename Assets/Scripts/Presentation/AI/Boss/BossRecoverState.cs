using UnityEngine;

namespace Presentation.AI
{
    public class BossRecoverState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _duration = 3f;
        private float _timer;

        public BossRecoverState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            Debug.Log("BOSS RECOVER");

            _timer = _duration;

            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = true;
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _behaviour.StateMachine.ChangeState(
                    new BossChaseState(_behaviour));
            }
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}