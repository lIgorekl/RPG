using Core.Combat;
using Gameplay.Stats;

namespace Gameplay.Characters
{
    // Базовый класс игровых персонажей
    // Хранит здоровье и параметры физического и магического урона
    public abstract class BaseCharacter : DamageableEntity, IDamageDealer
    {
        // Характеристики персонажа
        protected CharacterStats Stats;

        // Текущее и максимальное здоровье
        public int CurrentHP => Health.Current;
        public int MaxHP => Health.Max;

        protected BaseCharacter(CharacterStats stats) : base(stats.MaxHP)
        {
            Stats = stats;
        }

        // Возвращает физический урон персонажа
        public Damage GetPhysicalDamage()
        {
            return new Damage(Stats.PhysicalDamage, DamageType.Physical);
        }

        // Возвращает магический урон персонажа
        public Damage GetMagicalDamage()
        {
            return new Damage(Stats.MagicalDamage, DamageType.Magical);
        }

        // Возвращает основной тип урона персонажа
        // По умолчанию используется физический урон
        public virtual Damage GetDamage()
        {
            return GetPhysicalDamage();
        }

        // Устанавливает текущее здоровье персонажа
        public void SetHP(int value)
        {
            // Создаем новый объект здоровья
            Health = new Health(MaxHP);

            // Вычисляем разницу между максимальным и нужным здоровьем
            int delta = value - MaxHP;

            // Если здоровье должно быть меньше максимального,
            // искусственно наносим недостающий урон
            if (delta < 0)
                Health.TakeDamage(-delta);

            // Уведомляем подписчиков об изменении здоровья
            NotifyHealthChanged();
        }
    }
}