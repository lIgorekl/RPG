using UnityEngine;

namespace Presentation.AI
{
    public class BossStunState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _timer;

        public BossStunState(
            BossStateMachine stateMachine,
            float duration)
        {
            _stateMachine = stateMachine;
            _timer = duration;
        }

        public void Enter()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Stop(
                behaviour.Agent);

            if (behaviour.EnemyView.Animator != null)
            {
                behaviour.EnemyView.Animator
                    .SetTrigger("Hurt");
            }
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
