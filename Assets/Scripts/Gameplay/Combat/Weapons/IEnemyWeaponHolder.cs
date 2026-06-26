namespace Gameplay.Combat.Weapons
{
    // Интерфейс для объектов,
    // которые хранят текущее оружие врага
    // и предоставляют информацию о его параметрах.
    public interface IEnemyWeaponHolder
    {
        // Текущее оружие врага
        EnemyWeapon CurrentWeapon { get; }

        // Время между атаками текущим оружием
        float AttackCooldown { get; }
    }
}