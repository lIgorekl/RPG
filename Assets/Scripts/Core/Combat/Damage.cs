namespace Core.Combat
{
    // Тип урона в боевой системе
    public enum DamageType
    {
        Physical,
        Magical
    }

    // Структура с данными об уроне
    // Передается между объектами во время атаки
    public struct Damage
    {
        // Величина урона
        public int Value;

        // Тип урона
        public DamageType Type;

        public Damage(int value, DamageType type)
        {
            Value = value;
            Type = type;
        }
    }
}