namespace Gameplay.Stats
{
    // Класс, хранящий базовые характеристики персонажа
    // Используется при создании игрока и врагов
    public class CharacterStats
    {
        // Максимальное здоровье персонажа
        public int MaxHP { get; private set; }

        // Базовый физический урон
        public int PhysicalDamage { get; private set; }

        // Базовый магический урон
        public int MagicalDamage { get; private set; }

        // Создает набор характеристик персонажа
        public CharacterStats(
            int maxHP,
            int physicalDamage,
            int magicalDamage)
        {
            MaxHP = maxHP;
            PhysicalDamage = physicalDamage;
            MagicalDamage = magicalDamage;
        }
    }
}