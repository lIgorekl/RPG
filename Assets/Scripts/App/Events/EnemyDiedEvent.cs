using Presentation.Scene;

namespace App.Events
{
    // Событие смерти врага.
    // Передается через шину событий после того,
    // как любой враг погиб.
    public readonly struct EnemyDiedEvent
    {
        // Создает событие смерти врага
        public EnemyDiedEvent(
            EnemyCategory category,
            BaseEnemyView enemyView)
        {
            Category = category;
            EnemyView = enemyView;
        }

        // Категория погибшего врага
        // (обычный или босс)
        public EnemyCategory Category { get; }

        // Представление погибшего врага
        public BaseEnemyView EnemyView { get; }
    }
}