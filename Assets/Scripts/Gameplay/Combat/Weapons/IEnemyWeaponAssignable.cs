namespace Gameplay.Combat.Weapons
{
    // Интерфейс для компонентов,
    // которые поддерживают назначение случайного оружия
    public interface IEnemyWeaponAssignable
    {
        // Назначает случайное оружие врагу
        // с использованием переданного генератора случайных чисел
        void AssignRandomWeapon(System.Random random);
    }
}