using System;
using System.Collections.Generic;

namespace App.Services.Spawn
{
    public interface IEnemySpawnDefinitionSelector
    {
        EnemySpawnDefinition Select(
            IReadOnlyList<EnemySpawnDefinition> definitions,
            Random random);
    }
}
