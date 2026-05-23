using Presentation.Scene;

namespace App.Events
{
    public readonly struct BossSpawnedEvent
    {
        public BossSpawnedEvent(BaseEnemyView bossView)
        {
            BossView = bossView;
        }

        public BaseEnemyView BossView { get; }
    }
}
