using Core.Combat;
using Core.Stats;
using Gameplay.Stats;

namespace Gameplay.Characters
{
    // Базовый класс для объектов, которые могут получать урон.
    // Содержит систему здоровья и события изменения состояния.
    public abstract class DamageableEntity : IDamageable
    {
        protected IHealth Health;

        public bool IsDead => Health.IsDead;

        // События для UI, эффектов и других систем
        public event System.Action<Damage> DamageReceived;
        public event System.Action<int, int> HealthChanged;
        public event System.Action Died;

        protected DamageableEntity(int maxHealth)
        {
            Health = new Health(maxHealth);
        }

        // Получение урона
        public virtual void ReceiveDamage(Damage damage)
        {
            if (IsDead)
                return;

            Health.TakeDamage(damage.Value);

            // Уведомляем системы о изменении HP
            HealthChanged?.Invoke(Health.Current, Health.Max);
            DamageReceived?.Invoke(damage);

            OnDamageReceived(damage);

            // Проверяем смерть
            if (Health.IsDead)
            {
                OnDeath();
                Died?.Invoke();
            }
        }

        // Переопределяется в наследниках (игрок, враги)
        protected virtual void OnDamageReceived(Damage damage) { }

        protected virtual void OnDeath() { }
    }
}