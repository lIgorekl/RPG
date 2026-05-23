using Presentation.AI;
using Presentation.Scene;

namespace App.Events
{
    public sealed class EnemyDeathEventPublisher
    {
        private readonly IGameEventBus _eventBus;

        public EnemyDeathEventPublisher(IGameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Register(BaseEnemyView enemyView)
        {
            if (enemyView == null)
                return;

            var entity = enemyView.GetEntity();
            if (entity == null)
                return;

            void OnDied()
            {
                entity.Died -= OnDied;

                var category = ResolveCategory(enemyView);
                _eventBus.Publish(new EnemyDiedEvent(category, enemyView));
            }

            entity.Died += OnDied;
        }

        private static EnemyCategory ResolveCategory(BaseEnemyView enemyView)
        {
            if (enemyView.GetComponent<BossBehaviour>() != null)
                return EnemyCategory.Boss;

            return EnemyCategory.Regular;
        }
    }
}
