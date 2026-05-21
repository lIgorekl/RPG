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

            _behaviour.MovementService.Stop(_behaviour.Agent);
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateBossChase(_behaviour));
            }
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }
    }
}