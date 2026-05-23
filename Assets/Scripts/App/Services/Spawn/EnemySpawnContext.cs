using System;
using System.Collections.Generic;
using App.Services;
using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    public sealed class EnemySpawnContext
    {
        public EnemySpawnContext(
            GameMode gameMode,
            EnemySpawnCatalog catalog,
            EnemySpawnSettings settings,
            IReadOnlyList<SpawnPoint> spawnPoints,
            Transform player,
            System.Random random)
        {
            GameMode = gameMode;
            Catalog = catalog;
            Settings = settings;
            SpawnPoints = spawnPoints;
            Player = player;
            RandomSource = random;
        }

        public GameMode GameMode { get; }
        public EnemySpawnCatalog Catalog { get; }
        public EnemySpawnSettings Settings { get; }
        public IReadOnlyList<SpawnPoint> SpawnPoints { get; }
        public Transform Player { get; }
        public System.Random RandomSource { get; }
    }
}
