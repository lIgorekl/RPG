using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.AI
{
    public class EnemyWanderService
    {
        public void TryWander(
            NavMeshAgent agent,
            Transform self,
            float radius)
        {
            if (agent == null)
                return;

            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection =
                    Random.insideUnitSphere * radius +
                    self.position;

                if (NavMesh.SamplePosition(
                    randomDirection,
                    out NavMeshHit hit,
                    radius,
                    NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }
    }
}