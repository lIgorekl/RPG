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
        }

        protected override void OnDeath()
        {
        }
    }
}