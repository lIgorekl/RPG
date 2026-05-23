using System.Collections.Generic;
using Presentation.Scene;

namespace App.Services.Spawn
{
    public interface IEnemySpawnService
    {
        IReadOnlyList<BaseEnemyView> Spawn(EnemySpawnContext context);
    }
}
