using Core.Combat;
using Gameplay.Stats;

namespace Gameplay.Characters
{
    public abstract class BaseCharacter : DamageableEntity, IDamageDealer
    {
        public int CurrentHP => Health.Current;
        public int MaxHP => Health.Max;
        protected CharacterStats Stats;

        protected BaseCharacter(CharacterStats stats) : base(stats.MaxHP)
        {
            Stats = stats;
        }

        public Damage GetPhysicalDamage()
        {
            return new Damage(Stats.PhysicalDamage, DamageType.Physical);
        }

        public Damage GetMagicalDamage()
        {
            return new Damage(Stats.MagicalDamage, DamageType.Magical);
        }

        public virtual Damage GetDamage()
        {
            return GetPhysicalDamage();
        }
    }
}