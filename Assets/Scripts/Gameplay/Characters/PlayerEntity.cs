using Gameplay.Stats;

namespace Gameplay.Characters
{
    public class PlayerEntity : BaseCharacter
    {
        public PlayerEntity(CharacterStats stats) : base(stats)
        {
        }

        protected override void OnDamageReceived(Core.Combat.Damage damage)
        {
            // Здесь позже можно добавить реакцию на урон
            // Например: события, эффекты, звук и т.п.
        }

        protected override void OnDeath()
        {
            // Логика смерти игрока (позже будет Game Over)
        }
    }
}