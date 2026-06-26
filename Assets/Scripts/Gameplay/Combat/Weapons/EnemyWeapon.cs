namespace Gameplay.Combat.Weapons
{
    // Класс, представляющий экземпляр оружия врага.
    // Хранит ссылку на его конфигурацию и предоставляет
    // удобный доступ к основным характеристикам.
    public sealed class EnemyWeapon
    {
        // Создает экземпляр оружия на основе конфигурации
        public EnemyWeapon(EnemyWeaponConfig config)
        {
            Config = config;
        }

        // Конфигурация, описывающая характеристики оружия
        public EnemyWeaponConfig Config { get; }

        // Множитель урона текущего оружия
        public float DamageMultiplier =>
            Config.DamageMultiplier;

        // Время между атаками
        public float AttackCooldown =>
            Config.AttackCooldown;

        // Использовать ли тяжелую анимацию атаки
        public bool UseHeavyAttackAnimation =>
            Config.UseHeavyAttackAnimation;

        // Префаб снаряда (для дальнего оружия)
        public UnityEngine.GameObject ProjectilePrefab =>
            Config.ProjectilePrefab;
    }
}