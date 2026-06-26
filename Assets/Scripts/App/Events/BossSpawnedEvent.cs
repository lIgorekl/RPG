using Presentation.Scene;

namespace App.Events
{
    // Событие появления босса.
    // Передается через шину событий всем системам,
    // которым необходимо узнать,
    // что босс был создан.
    public readonly struct BossSpawnedEvent
    {
        // Создает событие
        // и сохраняет ссылку на созданного босса
        public BossSpawnedEvent(
            BaseEnemyView bossView)
        {
            BossView = bossView;
        }

        // Представление созданного босса
        public BaseEnemyView BossView { get; }
    }
}