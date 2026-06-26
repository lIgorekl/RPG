using Core.Stats;

namespace Gameplay.Stats
{
    // Реализация системы здоровья персонажа
    // Хранит текущее HP и обрабатывает получение урона и лечение
    public class Health : IHealth
    {
        // Текущее количество здоровья
        public int Current { get; private set; }

        // Максимальное количество здоровья
        public int Max { get; private set; }

        // Проверяет мертв ли персонаж
        public bool IsDead => Current <= 0;

        public Health(int maxHealth)
        {
            // При создании текущее здоровье равно максимальному
            Max = maxHealth;
            Current = maxHealth;
        }

        // Наносит урон персонажу
        public void TakeDamage(int value)
        {
            // Мертвый персонаж больше не получает урон
            if (IsDead)
                return;

            Current -= value;

            // Не допускаем отрицательное количество HP
            if (Current < 0)
                Current = 0;
        }

        // Восстанавливает здоровье
        public void Heal(int value)
        {
            // Мертвого персонажа нельзя лечить
            if (IsDead)
                return;

            Current += value;

            // Ограничиваем здоровье максимальным значением
            if (Current > Max)
                Current = Max;
        }
    }
}