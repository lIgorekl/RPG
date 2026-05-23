using UnityEngine;
using App.Services;
using App.Services.Spawn;
using App.SaveLoad;
using App.Repositories;
using Presentation.Player;
using Presentation.Scene;
using System.Collections.Generic;
using Presentation.AI;

namespace App
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private BaseEnemyView[] enemies;

        [Header("Enemy Spawning")]
        [SerializeField] private bool enableEnemySpawning = true;
        [SerializeField] private SpawnPoint[] spawnPoints;
        [SerializeField] private EnemySpawnCatalog spawnCatalog;
        [SerializeField] private EnemySpawnSettings spawnSettings;

        private readonly List<BaseEnemyView> _registeredEnemies = new();
        private SaveLoadInteractor _saveLoadInteractor;
        private ISaveService _saveService;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("GameSceneEntryPoint Initialize CALLED");

            var audioService =
                GameEntryPoint.Instance.GetAudioService();

            var gameModeService =
                GameEntryPoint.Instance.GetGameModeService();

            IPlayerRepository repository = new JsonPlayerRepository();
            _saveLoadInteractor = new SaveLoadInteractor(repository);
            _saveService = new SaveService(_saveLoadInteractor);

            if (player != null)
            {
                player.InitializeAudio(audioService);
            }

            _registeredEnemies.Clear();
            RegisterSceneEnemies();
            SpawnAndRegisterEnemies(gameModeService);

            var playerTransform = player != null ? player.transform : null;

            foreach (var enemy in _registeredEnemies)
            {
                if (enemy == null)
                    continue;

                enemy.InitializeAudio(audioService);

                var behaviour =
                    enemy.GetComponent<BaseEnemyBehaviour>();

                if (behaviour == null)
                    continue;

                if (playerTransform != null)
                    behaviour.SetPlayer(playerTransform);

                behaviour.Initialize(gameModeService);
            }

            Debug.Log("Enemies count: " + _registeredEnemies.Count);
            Debug.Log("SaveService CREATED");
        }

        private void RegisterSceneEnemies()
        {
            if (enemies == null)
                return;

            foreach (var enemy in enemies)
                RegisterEnemy(enemy);
        }

        private void SpawnAndRegisterEnemies(IGameModeService gameModeService)
        {
            if (!enableEnemySpawning ||
                spawnCatalog == null ||
                spawnSettings == null ||
                spawnPoints == null ||
                spawnPoints.Length == 0)
            {
                return;
            }

            var spawnService = new EnemySpawnService(
                new EnemyFactory(),
                new RandomSpawnPointSelector(),
                new WeightedEnemySpawnDefinitionSelector());

            var context = new EnemySpawnContext(
                gameModeService.CurrentMode,
                spawnCatalog,
                spawnSettings,
                spawnPoints,
                player != null ? player.transform : null,
                new System.Random());

            var spawnedEnemies = spawnService.Spawn(context);

            foreach (var enemy in spawnedEnemies)
                RegisterEnemy(enemy);
        }

        private void RegisterEnemy(BaseEnemyView enemy)
        {
            if (enemy == null || _registeredEnemies.Contains(enemy))
                return;

            _registeredEnemies.Add(enemy);
        }

        public ISaveService GetSaveService()
        {
            return _saveService;
        }

        public PlayerController GetPlayer()
        {
            return player;
        }

        public BaseEnemyView[] GetEnemies()
        {
            return _registeredEnemies.ToArray();
        }

        public void ApplyEnemiesSaveData(List<EnemySaveData> enemiesData)
        {
            foreach (var enemy in _registeredEnemies)
            {
                if (enemy == null)
                    continue;

                enemy.gameObject.SetActive(false);
            }

            foreach (var enemyData in enemiesData)
            {
                foreach (var enemy in _registeredEnemies)
                {
                    if (enemy == null)
                        continue;

                    if (enemy.GetId() != enemyData.Id)
                        continue;

                    enemy.ApplySaveData(enemyData);
                    break;
                }
            }
        }
    }
}
