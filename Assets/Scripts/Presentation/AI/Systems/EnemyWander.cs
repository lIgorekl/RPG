using UnityEngine;
using UnityEngine.AI;

namespace Presentation.AI
{
    // Система случайного перемещения врага.
    // Работает только если игрок находится вне радиуса обнаружения.
    public class EnemyWander : MonoBehaviour
    {
        [SerializeField] private float wanderRadius = 5f;
        [SerializeField] private float waitTime = 2f;

        private NavMeshAgent _agent;
        private float _waitTimer;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            SetNewDestination();
        }

        private void Update()
        {
            var enemyView = GetComponent<Presentation.Scene.BaseEnemyView>();

            // Если враг мёртв или оглушён — не двигаемся
            if (enemyView != null && (enemyView.IsDead || enemyView.IsStunned))
                return;

            var behaviour = GetComponent<IEnemyBehaviour>();
            if (behaviour == null)
                return;

            Transform player = behaviour.Player;
            float detectionRadius = behaviour.DetectionRadius;

            float distance = Vector3.Distance(
                transform.position,
                player.position);

            // Если игрок рядом — прекращаем блуждание
            if (distance <= detectionRadius)
                return;

            if (_agent.pathPending)
                return;

            if (_agent.remainingDistance <= 0.2f)
            {
                _waitTimer -= Time.deltaTime;

                if (_waitTimer <= 0f)
                    SetNewDestination();
            }
        }

        // Выбирает новую случайную точку на NavMesh
        private void SetNewDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, 1))
            {
                _agent.SetDestination(hit.position);
            }

            _waitTimer = waitTime;
        }
    }
}