using UnityEngine;

namespace Presentation.AI
{
    public class BossChaseState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        public BossChaseState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance > _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new BossIdleState(_behaviour));
                return;
            }

            if (distance <= _behaviour.AttackRadius)
            {
                // 50% шанс сильной атаки
                if (Random.value > 0.5f)
                {
                    Debug.Log("HEAVY ATTACK");

                    _behaviour.StateMachine.ChangeState(
                        new BossHeavyAttackState(_behaviour));
                }
                else
                {
                    Debug.Log("NORMAL ATTACK");

                    _behaviour.StateMachine.ChangeState(
                        new BossAttackState(_behaviour));
                }
                return;
            }

            Vector3 direction =
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                _behaviour.Self.rotation =
                    Quaternion.Slerp(
                        _behaviour.Self.rotation,
                        lookRotation,
                        10f * Time.deltaTime);
            }

            _behaviour.Agent.SetDestination(_behaviour.Player.position);
        }

        public void Exit() { }
    }
}