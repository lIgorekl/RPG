using UnityEngine;

namespace Presentation.AI
{
    public class BossAttackState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _cooldown = 1.5f;
        private float _timer;

        public BossAttackState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = _cooldown;

            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            if (!behaviour.ShouldAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            _timer -=
                Time.deltaTime *
                behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                behaviour.EnemyView.Attack(
                    behaviour.Player,
                    1f);

                _timer = _cooldown;
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
