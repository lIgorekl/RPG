using Presentation.AI;
using Presentation.Scene;
using Core.Combat;
using UnityEngine;

namespace Gameplay.Combat
{
    // Обрабатывает получение урона врагом
    // Применяет урон и при необходимости запускает оглушение
    public class EnemyDamageProcessor
    {
        // Выполняет обработку входящего урона
        public void ProcessDamage(
            BaseEnemyView enemyView,
            Damage damage,
            float stunDuration)
        {
            if (enemyView == null)
                return;

            var entity = enemyView.GetEntity();

            // Не обрабатываем урон для мертвого врага
            if (entity.IsDead)
                return;

            // Передаем урон игровой сущности врага
            entity.ReceiveDamage(damage);

            // Если враг умер от этого удара,
            // дополнительная обработка не требуется
            if (entity.IsDead)
                return;

            ProcessStun(
                enemyView,
                stunDuration);
        }

        // Обрабатывает оглушение после получения урона
        private void ProcessStun(
            BaseEnemyView enemyView,
            float stunDuration)
        {
            var behaviour =
                enemyView.GetComponent<BaseEnemyBehaviour>();

            if (behaviour == null)
                return;

            var entity = enemyView.GetEntity();

            // Вычисляем процент оставшегося здоровья
            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            // Враги в состоянии бегства не оглушаются
            if (hpPercent < 0.3f)
                return;

            // Если враг использует боевые состояния,
            // переводим его в состояние оглушения
            if (behaviour is BaseCombatEnemyBehaviour combatBehaviour)
            {
                combatBehaviour.EnterStunState(
                    stunDuration);
            }
        }
    }
}