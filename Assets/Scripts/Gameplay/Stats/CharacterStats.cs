namespace Gameplay.Stats
{
    // Хранит базовые характеристики персонажа.
    // Используется игроком и врагами.
    public class CharacterStats
    {
        public int MaxHP { get; private set; }
        public int PhysicalDamage { get; private set; }
        public int MagicalDamage { get; private set; }

        public CharacterStats(int maxHP, int physicalDamage, int magicalDamage)
        {
            MaxHP = maxHP;
            PhysicalDamage = physicalDamage;
            MagicalDamage = magicalDamage;
        }
    }
}