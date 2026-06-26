namespace Core.Stats
{
    // Интерфейс системы здоровья
    // Определяет общий набор свойств и методов для работы с HP
    public interface IHealth
    {
        // Текущее количество здоровья
        int Current { get; }

        // Максимальное количество здоровья
        int Max { get; }

        // Проверяет мертв ли объект
        bool IsDead { get; }

        // Наносит урон
        void TakeDamage(int value);

        // Восстанавливает здоровье
        void Heal(int value);
    }
}