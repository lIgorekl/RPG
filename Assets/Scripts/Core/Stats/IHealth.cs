namespace Core.Stats
{
    // Контракт системы здоровья.
    // Используется игровыми сущностями (игрок, враги и т.д.).
    public interface IHealth
    {
        int Current { get; }
        int Max { get; }

        bool IsDead { get; }

        void TakeDamage(int value);
        void Heal(int value);
    }
}