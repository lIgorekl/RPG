using UnityEngine;

namespace Presentation.AI
{
    public class BossAttackState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _cooldown = 1.5f;
        private float _timer;

        public BossAttackState(
            BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = _cooldown;

            _behaviour.MovementService.Stop(
                _behaviour.Agent);
        }

        public void Update()
        {
            _behaviour.TickAttack(
                ref _timer,
                _cooldown);
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(
                _behaviour.Agent);
        }
    }
}