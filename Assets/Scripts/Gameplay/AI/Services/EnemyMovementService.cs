using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.AI
{
    // Сервис перемещения врагов
    // Содержит общие методы для движения, поворота и работы с NavMeshAgent
    public class EnemyMovementService
    {
        // Перемещает врага к указанной точке
        public void MoveTo(
            NavMeshAgent agent,
            Vector3 target)
        {
            if (agent == null)
                return;

            agent.SetDestination(target);
        }

        // Полностью останавливает движение врага
        public void Stop(NavMeshAgent agent)
        {
            if (agent == null)
                return;

            agent.isStopped = true;
        }

        // Возобновляет движение врага
        public void Resume(NavMeshAgent agent)
        {
            if (agent == null)
                return;

            agent.isStopped = false;
        }

        // Плавно поворачивает объект к цели
        public void RotateTo(
            Transform self,
            Vector3 target,
            float speed = 10f)
        {
            Vector3 direction = target - self.position;

            // Игнорируем разницу по высоте
            direction.y = 0f;

            // Если цель слишком близко, поворот не нужен
            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion lookRotation =
                Quaternion.LookRotation(direction);

            // Выполняем плавный поворот
            self.rotation = Quaternion.Slerp(
                self.rotation,
                lookRotation,
                speed * Time.deltaTime);
        }

        // Сбрасывает текущий маршрут агента
        public void ResetPath(NavMeshAgent agent)
        {
            if (agent == null)
                return;

            agent.ResetPath();
        }

        // Пытается переместить врага в случайном направлении
        public bool TryMoveToRandomDirection(
            NavMeshAgent agent,
            Transform self,
            Vector3 direction,
            float distance)
        {
            if (agent == null)
                return false;

            // Вычисляем предполагаемую точку назначения
            Vector3 target =
                self.position + direction * distance;

            // Проверяем находится ли точка на NavMesh
            if (NavMesh.SamplePosition(
                target,
                out NavMeshHit hit,
                2f,
                NavMesh.AllAreas))
            {
                // Если точка подходит, строим маршрут
                agent.SetDestination(hit.position);

                return true;
            }

            return false;
        }

        // Поддерживает нужную дистанцию до цели
        // Используется дальними врагами
        public void MaintainDistance(
            NavMeshAgent agent,
            Transform self,
            Transform target,
            float minDistance,
            float maxDistance,
            float moveSpeed)
        {
            if (agent == null)
                return;

            // Вычисляем расстояние до цели
            float distance =
                Vector3.Distance(
                    self.position,
                    target.position);

            // Устанавливаем скорость движения
            agent.speed = moveSpeed;

            // Если игрок подошел слишком близко
            if (distance < minDistance)
            {
                // Вычисляем направление от игрока
                Vector3 direction =
                    (self.position - target.position).normalized;

                // Строим точку для отступления
                Vector3 destination =
                    self.position + direction * minDistance;

                agent.SetDestination(destination);

                return;
            }

            // Если игрок слишком далеко
            if (distance > maxDistance)
            {
                // Подходим ближе к игроку
                agent.SetDestination(target.position);

                return;
            }

            // Если дистанция подходит,
            // останавливаем движение
            agent.ResetPath();
        }
    }
}