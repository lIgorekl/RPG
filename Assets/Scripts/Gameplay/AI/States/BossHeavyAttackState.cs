using UnityEngine;

namespace Presentation.AI
{
    public class BossHeavyAttackState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _cooldown = 3f;
        private float _timer;

        public BossHeavyAttackState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            Debug.Log("ENTER HEAVY STATE");
            _timer = _cooldown;

            _behaviour.MovementService.Stop(_behaviour.Agent);
        }

        public void Update()
        {
            _timer -= Time.deltaTime * _behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                Debug.Log("HEAVY DAMAGE HIT");

                _behaviour.EnemyView.Attack(_behaviour.Player, 2.5f, true);

                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateBossRecover(_behaviour));
            }
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }
    }
}