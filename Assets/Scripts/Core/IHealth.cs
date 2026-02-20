namespace Core.Stats
{
    public interface IHealth
    {
        int Current { get; }
        int Max { get; }

        void TakeDamage(int value);
        void Heal(int value);
        bool IsDead { get; }
    }
}