namespace Gameplay.Combat.Weapons
{
    public sealed class EnemyWeapon
    {
        public EnemyWeapon(EnemyWeaponConfig config)
        {
            Config = config;
        }

        public EnemyWeaponConfig Config { get; }

        public float DamageMultiplier => Config.DamageMultiplier;
        public float AttackCooldown => Config.AttackCooldown;
        public bool UseHeavyAttackAnimation => Config.UseHeavyAttackAnimation;
        public UnityEngine.GameObject ProjectilePrefab => Config.ProjectilePrefab;
    }
}
