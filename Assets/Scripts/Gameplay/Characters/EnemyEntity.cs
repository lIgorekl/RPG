using Gameplay.Stats;
using Core.Combat;
using UnityEngine;

namespace Gameplay.Characters
{
    public class EnemyEntity : BaseCharacter
    {
        public EnemyEntity(CharacterStats stats) : base(stats)
        {
        }

        protected override void OnDamageReceived(Damage damage)
        {
            Debug.Log($"Enemy received {damage.Value} {damage.Type} damage. HP: {CurrentHP}/{MaxHP}");
        }

        protected override void OnDeath()
        {
            Debug.Log("Enemy died");
        }
    }
}