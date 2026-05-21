using Presentation.AI;
using Presentation.Scene;
using Core.Combat;
using UnityEngine;

namespace Gameplay.Combat
{
    public class EnemyDamageProcessor
    {
        public void ProcessDamage(
            BaseEnemyView enemyView,
            Damage damage,
            float stunDuration)
        {
            if (enemyView == null)
                return;

            var entity = enemyView.GetEntity();

            if (entity.IsDead)
                return;

            entity.ReceiveDamage(damage);

            if (entity.IsDead)
                return;

            ProcessStun(
                enemyView,
                stunDuration);
        }

        private void ProcessStun(
            BaseEnemyView enemyView,
            float stunDuration)
        {
            var behaviour =
                enemyView.GetComponent<BaseEnemyBehaviour>();

            if (behaviour == null)
                return;

            var entity = enemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            // flee enemies не станим
            if (hpPercent < 0.3f)
                return;

            if (behaviour is BossBehaviour bossBehaviour)
            {
                behaviour.StateMachine.ChangeState(
                    behaviour.StateFactory.CreateBossStun(
                        bossBehaviour,
                        stunDuration));
            }
            else
            {
                behaviour.StateMachine.ChangeState(
                    new StunState(
                        behaviour,
                        stunDuration));
            }
        }
    }
}