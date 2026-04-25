using UnityEngine;

namespace Presentation.AI
{
    // Состояние ожидания.
    // Враг ничего не делает, пока игрок не войдёт в радиус обнаружения.
    public class IdleState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;

        private float _wanderTimer;
        private float _wanderDelay = 2f;

        public IdleState(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // переход в агрессию
            if (distance <= _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new ChaseState(_behaviour));
                return;
            }

            // ЛОГИКА БЛУЖДАНИЯ
            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 5f;
                randomDirection += _behaviour.Self.position;

                if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out UnityEngine.AI.NavMeshHit hit, 5f, 1))
                {
                    _behaviour.Agent.SetDestination(hit.position);
                }

                _wanderTimer = _wanderDelay;
            }
        }

        public void Exit() { }
    }
}