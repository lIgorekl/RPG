using UnityEngine;

namespace Gameplay.Combat.Weapons
{
    // Конфигурация оружия врага.
    // Хранится как ScriptableObject и содержит параметры,
    // определяющие характеристики конкретного оружия.
    [CreateAssetMenu(
        fileName = "EnemyWeaponConfig",
        menuName = "RPG/Combat/Enemy Weapon Config")]
    public class EnemyWeaponConfig : ScriptableObject
    {
        // Множитель урона относительно базового урона врага
        [SerializeField] private float damageMultiplier = 1f;

        // Время между атаками этим оружием
        [SerializeField] private float attackCooldown = 1f;

        // Нужно ли использовать тяжелую анимацию атаки
        [SerializeField] private bool useHeavyAttackAnimation;

        // Префаб снаряда для дальнего оружия
        // Для ближнего оружия может быть не задан
        [SerializeField] private GameObject projectilePrefab;

        // Возвращает множитель урона.
        // Минимальное значение ограничено 0.1,
        // чтобы избежать нулевого или отрицательного урона.
        public float DamageMultiplier =>
            Mathf.Max(0.1f, damageMultiplier);

        // Возвращает время перезарядки.
        // Минимальное значение также ограничено 0.1 секунды.
        public float AttackCooldown =>
            Mathf.Max(0.1f, attackCooldown);

        // Возвращает информацию,
        // какую анимацию атаки следует использовать
        public bool UseHeavyAttackAnimation =>
            useHeavyAttackAnimation;

        // Возвращает префаб снаряда,
        // если он предусмотрен для данного оружия
        public GameObject ProjectilePrefab =>
            projectilePrefab;
    }
}