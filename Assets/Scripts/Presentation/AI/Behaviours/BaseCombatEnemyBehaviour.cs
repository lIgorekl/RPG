using UnityEngine;

namespace Presentation.AI
{
    // Базовый класс для боевых врагов
    // Содержит общую логику преследования, бегства, оглушения и патрулирования
    public abstract class BaseCombatEnemyBehaviour
        : BaseEnemyBehaviour
    {
        // Проверяет должен ли враг перейти в состояние бегства
        public bool ShouldFlee()
        {
            var entity = EnemyView.GetEntity();

            // Вычисляем процент оставшегося здоровья
            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            // При здоровье ниже 30% враг начинает убегать
            return hpPercent < 0.3f;
        }

        // Поворачивает врага в сторону игрока
        protected void RotateToPlayer()
        {
            Vector3 direction =
                Player.position - Self.position;

            // Игнорируем разницу по высоте
            direction.y = 0f;

            // Если цель слишком близко, поворот не нужен
            if (direction.sqrMagnitude < 0.01f)
                return;

            Self.rotation =
                Quaternion.LookRotation(direction);
        }

        // Логика состояния ожидания и патрулирования
        public void UpdateIdle(
            ref float wanderTimer,
            float wanderDelay)
        {
            // Уменьшаем таймер до следующего перемещения
            wanderTimer -= Time.deltaTime;

            // Когда таймер закончился, выбираем новую точку патруля
            if (wanderTimer <= 0f)
            {
                WanderService.TryWander(
                    Agent,
                    Self,
                    5f);

                // Перезапускаем таймер
                wanderTimer = wanderDelay;
            }
        }

        // Проверяет находится ли игрок в зоне обнаружения
        protected bool IsTargetDetected()
        {
            return CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        // Подготавливает врага к бегству
        public void EnterFlee()
        {
            // Сбрасываем текущий маршрут
            MovementService.ResetPath(Agent);

            // Разрешаем движение
            MovementService.Resume(Agent);
        }

        // Подготавливает врага к оглушению
        public void EnterStun()
        {
            // Полностью останавливаем движение
            MovementService.Stop(Agent);
        }

        // Завершает состояние оглушения
        public void ExitStun()
        {
            // Возвращаем возможность двигаться
            MovementService.Resume(Agent);
        }

        // Переводит врага в состояние оглушения
        // Переопределяется в конкретных типах врагов
        public virtual void EnterStunState(float duration)
        {
        }
    }
}