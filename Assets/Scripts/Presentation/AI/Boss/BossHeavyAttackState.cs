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

            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = true;
        }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            _timer -= Time.deltaTime * _behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                Debug.Log("HEAVY DAMAGE HIT");

                _behaviour.EnemyView.Attack(_behaviour.Player, 2.5f, true);

                _behaviour.StateMachine.ChangeState(
                    new BossRecoverState(_behaviour));
            }

            _timer -= Time.deltaTime * _behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                // можно усилить урон позже
                _behaviour.EnemyView.Attack(_behaviour.Player, 2.5f, true);

                _behaviour.StateMachine.ChangeState(
                    new BossRecoverState(_behaviour));
            }
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}