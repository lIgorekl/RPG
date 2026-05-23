using System;
using System.Collections.Generic;

namespace App.Services.Spawn
{
    public class WeightedEnemySpawnDefinitionSelector
        : IEnemySpawnDefinitionSelector
    {
        public EnemySpawnDefinition Select(
            IReadOnlyList<EnemySpawnDefinition> definitions,
            Random random)
        {
            if (definitions == null || definitions.Count == 0)
                return null;

            int totalWeight = 0;
            for (int i = 0; i < definitions.Count; i++)
                totalWeight += definitions[i].SpawnWeight;

            int roll = random.Next(0, totalWeight);
            int cumulative = 0;

            for (int i = 0; i < definitions.Count; i++)
            {
                cumulative += definitions[i].SpawnWeight;
                if (roll < cumulative)
                    return definitions[i];
            }

            return definitions[definitions.Count - 1];
        }
    }
}
