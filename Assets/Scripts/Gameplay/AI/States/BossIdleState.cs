using UnityEngine;

namespace Presentation.AI
{
    public class BossIdleState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _wanderTimer;
        private float _wanderDelay = 3f;

        public BossIdleState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            if (behaviour.ShouldDetectPlayer())
            {
                _stateMachine.EnterChase();
                return;
            }

            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0f)
            {
                behaviour.WanderService.TryWander(
                    behaviour.Agent,
                    behaviour.Self,
                    6f);

                _wanderTimer = _wanderDelay;
            }
        }

        public void Exit() { }
    }
}
