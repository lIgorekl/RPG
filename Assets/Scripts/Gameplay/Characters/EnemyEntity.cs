using Gameplay.Stats;
using Core.Combat;
using UnityEngine;

namespace Gameplay.Characters
{
    // Игровая сущность врага
    // Хранит характеристики, здоровье и получает урон
    public class EnemyEntity : BaseCharacter
    {
        public EnemyEntity(CharacterStats stats) : base(stats)
        {
        }

        // Вызывается после получения урона
        // При необходимости здесь можно добавить дополнительную логику
        protected override void OnDamageReceived(Damage damage)
        {
        }

        // Вызывается после смерти врага
        // При необходимости здесь можно добавить дополнительную логику
        protected override void OnDeath()
        {
        }
    }
}