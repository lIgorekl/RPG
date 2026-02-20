using Core.Combat;
using Core.Stats;
using Gameplay.Stats;

namespace Gameplay.Characters
{
    public abstract class DamageableEntity : IDamageable
    {
        public event System.Action<int, int> HealthChanged;
        public event System.Action Died;
        protected IHealth Health;

        public bool IsDead => Health.IsDead;

        protected DamageableEntity(int maxHealth)
        {
            Health = new Health(maxHealth);
        }

        public virtual void ReceiveDamage(Damage damage)
        {
            if (IsDead) return;

            Health.TakeDamage(damage.Value);
            HealthChanged?.Invoke(Health.Current, Health.Max);
            OnDamageReceived(damage);

            if (Health.IsDead)
            {
                OnDeath();
                Died?.Invoke();
            }
        }

        protected virtual void OnDamageReceived(Damage damage) { }

        protected virtual void OnDeath() { }
    }
}