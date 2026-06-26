using Gameplay.Stats;
using Core.Combat;

namespace Gameplay.Characters
{
    // Доменная сущность игрока
    // Хранит игровые характеристики и состояние игрока
    // Наследуется от BaseCharacter, поэтому получает здоровье,
    // систему получения урона и базовые параметры атаки
    public class PlayerEntity : BaseCharacter
    {
        // Создает игровую сущность игрока
        // В конструктор передаются характеристики персонажа
        // (здоровье, физический и магический урон)
        public PlayerEntity(CharacterStats stats)
            : base(stats)
        {
        }
    }
}