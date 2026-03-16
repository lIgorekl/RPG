using Gameplay.Stats;
using Core.Combat;

namespace Gameplay.Characters
{
    // Доменная сущность игрока.
    // Здесь будет размещаться логика, специфичная для игрока.
    public class PlayerEntity : BaseCharacter
    {
        public PlayerEntity(CharacterStats stats) : base(stats)
        {
        }
    }
}