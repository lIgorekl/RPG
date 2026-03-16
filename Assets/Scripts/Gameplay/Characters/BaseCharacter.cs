using Core.Combat;
using Gameplay.Stats;

namespace Gameplay.Characters
{
    // Базовый класс всех игровых персонажей (игрок, враги).
    // Содержит здоровье и базовые параметры урона.
    public abstract class BaseCharacter : DamageableEntity, IDamageDealer
    {
        protected CharacterStats Stats;

        public int CurrentHP => Health.Current;
        public int MaxHP => Health.Max;

        protected BaseCharacter(CharacterStats stats) : base(stats.MaxHP)
        {
            Stats = stats;
        }

        // Физический урон персонажа
        public Damage GetPhysicalDamage()
        {
            return new Damage(Stats.PhysicalDamage, DamageType.Physical);
        }

        // Магический урон персонажа
        public Damage GetMagicalDamage()
        {
            return new Damage(Stats.MagicalDamage, DamageType.Magical);
        }

        // Базовый тип урона (по умолчанию физический)
        public virtual Damage GetDamage()
        {
            return GetPhysicalDamage();
        }
    }
}