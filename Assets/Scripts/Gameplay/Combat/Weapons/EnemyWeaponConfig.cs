using UnityEngine;

namespace Gameplay.Combat.Weapons
{
    [CreateAssetMenu(
        fileName = "EnemyWeaponConfig",
        menuName = "RPG/Combat/Enemy Weapon Config")]
    public class EnemyWeaponConfig : ScriptableObject
    {
        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private bool useHeavyAttackAnimation;
        [SerializeField] private GameObject projectilePrefab;

        public float DamageMultiplier => Mathf.Max(0.1f, damageMultiplier);
        public float AttackCooldown => Mathf.Max(0.1f, attackCooldown);
        public bool UseHeavyAttackAnimation => useHeavyAttackAnimation;
        public GameObject ProjectilePrefab => projectilePrefab;
    }
}
