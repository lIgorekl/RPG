using UnityEngine;
using UnityEngine.AI;

namespace Presentation.AI
{
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
            var melee = GetComponent<EnemyBehaviour>();
            var ranged = GetComponent<RangedEnemyBehaviour>();

            Transform player = null;
            float detectionRadius = 0f;

            if (melee != null)
            {
                player = melee.Player;
                detectionRadius = melee.DetectionRadius;
            }
            else if (ranged != null)
            {
                player = ranged.Player;
                detectionRadius = ranged.DetectionRadius;
            }
            else
            {
                return;
            }

            float distance = Vector3.Distance(
                transform.position,
                player.position);

            // Если игрок рядом — не бродим
            if (distance <= detectionRadius)
                return;

            if (_agent.pathPending)
                return;

            if (_agent.remainingDistance <= 0.2f)
            {
                _waitTimer -= Time.deltaTime;

                if (_waitTimer <= 0f)
                {
                    SetNewDestination();
                }
            }
        }

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