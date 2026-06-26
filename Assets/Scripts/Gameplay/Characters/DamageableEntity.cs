using Core.Combat;
using Core.Stats;
using Gameplay.Stats;

namespace Gameplay.Characters
{
    // Базовый класс для объектов, которые могут получать урон
    // Хранит здоровье и события, связанные с получением урона и смертью
    public abstract class DamageableEntity : IDamageable
    {
        // Система здоровья объекта
        protected IHealth Health;

        public bool IsDead => Health.IsDead;

        // События для UI и игровых систем
        public event System.Action<Damage> DamageReceived;
        public event System.Action<int, int> HealthChanged;
        public event System.Action Died;

        protected DamageableEntity(int maxHealth)
        {
            Health = new Health(maxHealth);
        }

        // Обрабатывает получение урона
        public virtual void ReceiveDamage(Damage damage)
        {
            // Мертвый объект больше не получает урон
            if (IsDead)
                return;

            // Уменьшаем здоровье на величину урона
            Health.TakeDamage(damage.Value);

            // Уведомляем подписчиков об изменении здоровья
            HealthChanged?.Invoke(Health.Current, Health.Max);

            // Уведомляем о факте получения урона
            DamageReceived?.Invoke(damage);

            // Дополнительная логика наследников
            OnDamageReceived(damage);

            // Проверяем смерть после получения урона
            if (Health.IsDead)
            {
                OnDeath();

                // Уведомляем подписчиков о смерти объекта
                Died?.Invoke();
            }
        }

        // Переопределяется для дополнительной логики получения урона
        protected virtual void OnDamageReceived(Damage damage) { }

        // Переопределяется для дополнительной логики смерти
        protected virtual void OnDeath() { }

        // Принудительно уведомляет подписчиков об изменении здоровья
        protected void NotifyHealthChanged()
        {
            HealthChanged?.Invoke(Health.Current, Health.Max);
        }
    }
}