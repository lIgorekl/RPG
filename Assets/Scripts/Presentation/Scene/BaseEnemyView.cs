using UnityEngine;
using Gameplay.Characters;
using Gameplay.Stats;
using Core.Combat;

namespace Presentation.Scene
{
    public abstract class BaseEnemyView : MonoBehaviour, IDamageable
    {
        [Header("Base Enemy Stats")]
        [SerializeField] protected int maxHP = 50;
        [SerializeField] protected int physicalDamage = 10;
        [SerializeField] protected int magicalDamage = 0;

        protected EnemyEntity enemy;

        public EnemyEntity GetEntity()
        {
            return enemy;
        }

        protected virtual void Awake()
        {
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);
            enemy = new EnemyEntity(stats);
            enemy.Died += OnEnemyDied;
        }

        protected virtual void OnDestroy()
        {
            if (enemy != null)
                enemy.Died -= OnEnemyDied;
        }

        public virtual void ReceiveDamage(Damage damage)
        {
            enemy.ReceiveDamage(damage);
        }

        protected virtual void OnEnemyDied()
        {
            Destroy(gameObject);
        }

        public bool IsDead => enemy.IsDead;
    }
}