using UnityEngine;

namespace Presentation.AI
{
    public class BossRecoverState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _timer;

        public BossRecoverState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = 3f;

            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _stateMachine.EnterChase();
            }
        }

        public void Exit()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Resume(
                behaviour.Agent);
        }
    }
}
