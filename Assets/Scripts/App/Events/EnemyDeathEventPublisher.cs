using Presentation.AI;
using Presentation.Scene;

namespace App.Events
{
    // Публикует событие смерти врага.
    // Подписывается на событие смерти сущности
    // и передает информацию в шину игровых событий.
    public sealed class EnemyDeathEventPublisher
    {
        // Шина игровых событий
        private readonly IGameEventBus _eventBus;

        public EnemyDeathEventPublisher(
            IGameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        // Регистрирует врага
        // для отслеживания его смерти
        public void Register(
            BaseEnemyView enemyView)
        {
            // Проверяем,
            // что представление врага существует
            if (enemyView == null)
                return;

            // Получаем доменную сущность врага
            var entity =
                enemyView.GetEntity();

            // Если сущность отсутствует,
            // подписаться невозможно
            if (entity == null)
                return;

            // Локальный обработчик смерти
            void OnDied()
            {
                // После первой смерти
                // отписываемся от события,
                // чтобы обработчик не вызывался повторно
                entity.Died -= OnDied;

                // Определяем категорию врага
                var category =
                    ResolveCategory(enemyView);

                // Публикуем событие смерти врага
                _eventBus.Publish(
                    new EnemyDiedEvent(
                        category,
                        enemyView));
            }

            // Подписываемся
            // на событие смерти сущности
            entity.Died += OnDied;
        }

        // Определяет категорию врага
        private static EnemyCategory ResolveCategory(
            BaseEnemyView enemyView)
        {
            // Если на объекте есть BossBehaviour,
            // считаем врага боссом
            if (enemyView.GetComponent<
                BossBehaviour>() != null)
            {
                return EnemyCategory.Boss;
            }

            // Иначе считаем его обычным врагом
            return EnemyCategory.Regular;
        }
    }
}