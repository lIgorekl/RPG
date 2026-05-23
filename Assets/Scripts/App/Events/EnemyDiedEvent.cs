using Presentation.Scene;

namespace App.Events
{
    public readonly struct EnemyDiedEvent
    {
        public EnemyDiedEvent(EnemyCategory category, BaseEnemyView enemyView)
        {
            Category = category;
            EnemyView = enemyView;
        }

        public EnemyCategory Category { get; }
        public BaseEnemyView EnemyView { get; }
    }
}
