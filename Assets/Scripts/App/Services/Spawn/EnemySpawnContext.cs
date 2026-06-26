using System;
using System.Collections.Generic;
using App.Services;
using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    // Контекст спавна врагов.
    // Объединяет все данные, необходимые системе спавна,
    // в одном объекте.
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

        // Текущий режим игры (Normal / Peaceful)
        public GameMode GameMode { get; }

        // Каталог всех возможных врагов
        public EnemySpawnCatalog Catalog { get; }

        // Настройки спавна
        public EnemySpawnSettings Settings { get; }

        // Доступные точки появления врагов
        public IReadOnlyList<SpawnPoint> SpawnPoints { get; }

        // Ссылка на игрока
        public Transform Player { get; }

        // Генератор случайных чисел
        public System.Random RandomSource { get; }
    }
}