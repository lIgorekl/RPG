using UnityEngine;

namespace Presentation.AI
{
    public class BossAttackState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _cooldown = 1.5f;
        private float _timer;

        public BossAttackState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            Debug.Log("ENTER NORMAL ATTACK");
            _timer = _cooldown;

            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = true;
        }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance > _behaviour.AttackRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new BossChaseState(_behaviour));
                return;
            }

            _timer -= Time.deltaTime * _behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                _behaviour.EnemyView.Attack(_behaviour.Player, 1f);
                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}