namespace Core.Combat
{
    // Тип урона. Используется для разделения механик (физический / магический).
    public enum DamageType
    {
        Physical,
        Magical
    }

    // Структура, описывающая урон.
    // Передаётся между системами боя (оружие, снаряды, персонажи).
    public struct Damage
    {
        public int Value;
        public DamageType Type;

        public Damage(int value, DamageType type)
        {
            Value = value;
            Type = type;
        }
    }
}