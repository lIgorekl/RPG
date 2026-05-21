using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.AI
{
    public class EnemyMovementService
    {
        public void MoveTo(
            NavMeshAgent agent,
            Vector3 target)
        {
            if (agent == null)
                return;

            agent.SetDestination(target);
        }

        public void Stop(NavMeshAgent agent)
        {
            if (agent == null)
                return;

            agent.isStopped = true;
        }

        public void Resume(NavMeshAgent agent)
        {
            if (agent == null)
                return;

            agent.isStopped = false;
        }

        public void RotateTo(
            Transform self,
            Vector3 target,
            float speed = 10f)
        {
            Vector3 direction = target - self.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion lookRotation =
                Quaternion.LookRotation(direction);

            self.rotation = Quaternion.Slerp(
                self.rotation,
                lookRotation,
                speed * Time.deltaTime);
        }

        public void ResetPath(NavMeshAgent agent)
        {
            if (agent == null)
                return;

            agent.ResetPath();
        }

        public bool TryMoveToRandomDirection(
            NavMeshAgent agent,
            Transform self,
            Vector3 direction,
            float distance)
        {
            if (agent == null)
                return false;

            Vector3 target =
                self.position + direction * distance;

            if (NavMesh.SamplePosition(
                target,
                out NavMeshHit hit,
                2f,
                NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return true;
            }

            return false;
        }
    }
}