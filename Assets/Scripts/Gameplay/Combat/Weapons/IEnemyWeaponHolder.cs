namespace Gameplay.Combat.Weapons
{
    public interface IEnemyWeaponHolder
    {
        EnemyWeapon CurrentWeapon { get; }
        float AttackCooldown { get; }
    }
}
