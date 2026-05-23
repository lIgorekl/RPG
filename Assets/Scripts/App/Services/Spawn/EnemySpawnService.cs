using System;
using System.Collections.Generic;
using App.Services;
using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    public class EnemySpawnService : IEnemySpawnService
    {
        private readonly IEnemyFactory _enemyFactory;
        private readonly ISpawnPointSelector _spawnPointSelector;
        private readonly IEnemySpawnDefinitionSelector _definitionSelector;
        private int _spawnCounter;

        public EnemySpawnService(
            IEnemyFactory enemyFactory,
            ISpawnPointSelector spawnPointSelector,
            IEnemySpawnDefinitionSelector definitionSelector)
        {
            _enemyFactory = enemyFactory;
            _spawnPointSelector = spawnPointSelector;
            _definitionSelector = definitionSelector;
        }

        public IReadOnlyList<BaseEnemyView> Spawn(EnemySpawnContext context)
        {
            if (context == null)
                return Array.Empty<BaseEnemyView>();

            var definitions = FilterDefinitions(context);
            if (definitions.Count == 0)
                return Array.Empty<BaseEnemyView>();

            if (context.SpawnPoints == null || context.SpawnPoints.Count == 0)
                return Array.Empty<BaseEnemyView>();

            int spawnCount = context.RandomSource.Next(
                context.Settings.MinSpawnCount,
                context.Settings.MaxSpawnCount + 1);

            var selectedPoints = _spawnPointSelector.Select(
                context.SpawnPoints,
                spawnCount,
                context.RandomSource);

            var spawnedEnemies = new List<BaseEnemyView>(selectedPoints.Count);

            foreach (var spawnPoint in selectedPoints)
            {
                var definition = _definitionSelector.Select(
                    definitions,
                    context.RandomSource);

                if (definition == null)
                    continue;

                string enemyId = BuildEnemyId(definition);
                var enemy = _enemyFactory.Create(
                    definition,
                    spawnPoint.Position,
                    spawnPoint.Rotation,
                    enemyId,
                    context.Player,
                    context.RandomSource);

                if (enemy != null)
                    spawnedEnemies.Add(enemy);
            }

            return spawnedEnemies;
        }

        private List<EnemySpawnDefinition> FilterDefinitions(
            EnemySpawnContext context)
        {
            var result = new List<EnemySpawnDefinition>();

            if (context.Catalog?.Definitions == null)
                return result;

            foreach (var definition in context.Catalog.Definitions)
            {
                if (definition == null)
                    continue;

                if (definition.IsAllowedIn(context.GameMode))
                    result.Add(definition);
            }

            return result;
        }

        private string BuildEnemyId(EnemySpawnDefinition definition)
        {
            _spawnCounter++;
            return $"{definition.IdPrefix}_{_spawnCounter}";
        }
    }
}
