using Presentation.Scene;

namespace App.Events
{
    // Интерфейс регистрации врагов в игровых событиях
    // Используется для добавления врагов, созданных во время игры
    public interface IGameEnemyRegistry
    {
        // Регистрирует нового врага в системе событий
        void RegisterSpawnedEnemy(BaseEnemyView enemy);
    }
}