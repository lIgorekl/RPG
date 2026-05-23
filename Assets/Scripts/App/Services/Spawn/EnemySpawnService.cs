using System;
using System.Collections.Generic;
using App.Services;
using Presentation.AI;
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

            var allowedDefinitions = FilterDefinitions(context);
            if (allowedDefinitions.Count == 0)
                return Array.Empty<BaseEnemyView>();

            if (context.SpawnPoints == null || context.SpawnPoints.Count == 0)
                return Array.Empty<BaseEnemyView>();

            var meleeDefinition = FindDefinition(
                allowedDefinitions,
                EnemySpawnCombatRole.Melee,
                isBoss: false);

            var rangedDefinition = FindDefinition(
                allowedDefinitions,
                EnemySpawnCombatRole.Ranged,
                isBoss: false);

            var regularDefinitions = FilterRegularDefinitions(
                allowedDefinitions);

            int minCount = context.Settings.MinSpawnCount;
            int maxCount = context.Settings.MaxSpawnCount;

            int requiredTypes = 0;
            if (meleeDefinition != null)
                requiredTypes++;
            if (rangedDefinition != null)
                requiredTypes++;

            int targetCount = context.RandomSource.Next(minCount, maxCount + 1);
            targetCount = Mathf.Max(targetCount, requiredTypes);
            targetCount = Mathf.Clamp(targetCount, minCount, maxCount);

            var spawnPlan = BuildSpawnPlan(
                targetCount,
                meleeDefinition,
                rangedDefinition,
                regularDefinitions,
                context.RandomSource);

            if (spawnPlan.Count == 0)
                return Array.Empty<BaseEnemyView>();

            var selectedPoints = _spawnPointSelector.Select(
                context.SpawnPoints,
                spawnPlan.Count,
                context.RandomSource);

            return SpawnFromPlan(
                spawnPlan,
                selectedPoints,
                context);
        }

        private List<EnemySpawnDefinition> BuildSpawnPlan(
            int targetCount,
            EnemySpawnDefinition meleeDefinition,
            EnemySpawnDefinition rangedDefinition,
            List<EnemySpawnDefinition> regularDefinitions,
            System.Random random)
        {
            var plan = new List<EnemySpawnDefinition>(targetCount);

            if (meleeDefinition != null)
                plan.Add(meleeDefinition);

            if (rangedDefinition != null &&
                !ContainsDefinition(plan, rangedDefinition))
            {
                plan.Add(rangedDefinition);
            }

            while (plan.Count < targetCount)
            {
                if (regularDefinitions.Count == 0)
                    break;

                var definition = _definitionSelector.Select(
                    regularDefinitions,
                    random);

                if (definition == null)
                    break;

                plan.Add(definition);
            }

            return plan;
        }

        private List<BaseEnemyView> SpawnFromPlan(
            List<EnemySpawnDefinition> spawnPlan,
            IReadOnlyList<SpawnPoint> spawnPoints,
            EnemySpawnContext context)
        {
            var spawnedEnemies = new List<BaseEnemyView>(spawnPlan.Count);
            int pointCount = Mathf.Min(spawnPlan.Count, spawnPoints.Count);

            for (int i = 0; i < pointCount; i++)
            {
                var definition = spawnPlan[i];
                var spawnPoint = spawnPoints[i];

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

        private static List<EnemySpawnDefinition> FilterRegularDefinitions(
            List<EnemySpawnDefinition> definitions)
        {
            var result = new List<EnemySpawnDefinition>();

            foreach (var definition in definitions)
            {
                if (definition == null || definition.IsBoss)
                    continue;

                result.Add(definition);
            }

            return result;
        }

        private static EnemySpawnDefinition FindDefinition(
            List<EnemySpawnDefinition> definitions,
            EnemySpawnCombatRole role,
            bool isBoss)
        {
            foreach (var definition in definitions)
            {
                if (definition == null)
                    continue;

                if (definition.IsBoss != isBoss)
                    continue;

                if (ResolveCombatRole(definition) == role)
                    return definition;
            }

            return null;
        }

        private static EnemySpawnCombatRole ResolveCombatRole(
            EnemySpawnDefinition definition)
        {
            if (definition.IsBoss)
                return definition.CombatRole;

            var prefab = definition.Prefab;
            if (prefab != null)
            {
                if (prefab.GetComponent<RangedEnemyBehaviour>() != null)
                    return EnemySpawnCombatRole.Ranged;

                if (prefab.GetComponent<EnemyBehaviour>() != null)
                    return EnemySpawnCombatRole.Melee;
            }

            return definition.CombatRole;
        }

        private static bool ContainsDefinition(
            List<EnemySpawnDefinition> plan,
            EnemySpawnDefinition definition)
        {
            return plan.Contains(definition);
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
