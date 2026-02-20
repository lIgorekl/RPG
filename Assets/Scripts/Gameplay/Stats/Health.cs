using Core.Stats;

namespace Gameplay.Stats
{
    public class Health : IHealth
    {
        public int Current { get; private set; }
        public int Max { get; private set; }
        public bool IsDead => Current <= 0;

        public Health(int maxHealth)
        {
            Max = maxHealth;
            Current = maxHealth;
        }

        public void TakeDamage(int value)
        {
            if (IsDead) return;

            Current -= value;
            if (Current < 0)
                Current = 0;
        }

        public void Heal(int value)
        {
            if (IsDead) return;

            Current += value;
            if (Current > Max)
                Current = Max;
        }
    }
}