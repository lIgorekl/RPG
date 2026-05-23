using Presentation.Scene;

namespace App.Events
{
    public interface IGameEnemyRegistry
    {
        void RegisterSpawnedEnemy(BaseEnemyView enemy);
    }
}
