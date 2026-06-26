using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.AI
{
    // Сервис случайного перемещения врагов
    // Используется для патрулирования территории в состоянии ожидания
    public class EnemyWanderService
    {
        // Пытается найти случайную точку на NavMesh и направить туда врага
        public void TryWander(
            NavMeshAgent agent,
            Transform self,
            float radius)
        {
            if (agent == null)
                return;

            // Делаем несколько попыток найти подходящую точку
            for (int i = 0; i < 5; i++)
            {
                // Генерируем случайную точку вокруг врага
                Vector3 randomDirection =
                    Random.insideUnitSphere * radius +
                    self.position;

                // Проверяем находится ли точка на NavMesh
                if (NavMesh.SamplePosition(
                        randomDirection,
                        out NavMeshHit hit,
                        radius,
                        NavMesh.AllAreas))
                {
                    // Если точка найдена, строим маршрут
                    agent.SetDestination(hit.position);

                    return;
                }
            }
        }
    }
}