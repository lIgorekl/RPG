using UnityEngine;
using App.Services;
using App.Services.Spawn;
using App.SaveLoad;
using App.Repositories;
using App.Events;
using App.Services.Score;
using Presentation.Player;
using Presentation.Scene;
using Presentation.UI;
using System.Collections.Generic;
using Presentation.AI;

namespace App
{
    public class GameSceneEntryPoint : MonoBehaviour, IGameEnemyRegistry
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private BaseEnemyView[] enemies;

        [Header("Enemy Spawning")]
        [SerializeField] private bool enableEnemySpawning = true;
        [SerializeField] private SpawnPoint[] spawnPoints;
        [SerializeField] private EnemySpawnCatalog spawnCatalog;
        [SerializeField] private EnemySpawnSettings spawnSettings;

        [Header("Game Events")]
        [SerializeField] private GameEventsSettings gameEventsSettings;

        [Header("Score")]
        [SerializeField] private ScoreSettings scoreSettings;
        [SerializeField] private ScoreboardView scoreboardView;

        private readonly List<BaseEnemyView> _registeredEnemies = new();
        private SaveLoadInteractor _saveLoadInteractor;
        private ISaveService _saveService;
        private IGameEventBus _eventBus;
        private GameEventsInstaller _gameEventsInstaller;

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

            foreach (var enemy in _registeredEnemies)
                SetupEnemy(enemy, audioService, gameModeService);

            InitializeGameEvents(audioService);

            Debug.Log("Enemies count: " + _registeredEnemies.Count);
            Debug.Log("SaveService CREATED");
        }

        private void InitializeGameEvents(IAudioService audioService)
        {
            if (audioService == null)
            {
                Debug.LogError(
                    "GameSceneEntryPoint: AudioService is null. " +
                    "Start the game from MainMenu so GameEntryPoint initializes audio.");
                return;
            }

            if (gameEventsSettings == null)
            {
                Debug.LogWarning(
                    "GameSceneEntryPoint: GameEventsSettings is not assigned.");
                return;
            }

            if (gameEventsSettings.VictoryMusic == null)
            {
                Debug.LogWarning(
                    "GameSceneEntryPoint: VictoryMusic is not assigned in GameEventsSettings.");
            }

            _eventBus = new GameEventBus();

            if (scoreSettings != null)
                new ScoreService(_eventBus, scoreSettings);

            _gameEventsInstaller = new GameEventsInstaller(
                _eventBus,
                gameEventsSettings,
                audioService,
                this,
                spawnPoints,
                player);

            foreach (var enemy in _registeredEnemies)
                _gameEventsInstaller.RegisterEnemy(enemy);

            InitializeScoreboard();
        }

        private void InitializeScoreboard()
        {
            if (scoreboardView == null)
                scoreboardView = FindFirstObjectByType<ScoreboardView>();

            if (scoreboardView == null || _eventBus == null)
                return;

            scoreboardView.Initialize(_eventBus);
        }

        public void RegisterSpawnedEnemy(BaseEnemyView enemy)
        {
            if (enemy == null)
                return;

            RegisterEnemy(enemy);

            var audioService =
                GameEntryPoint.Instance.GetAudioService();

            var gameModeService =
                GameEntryPoint.Instance.GetGameModeService();

            SetupEnemy(enemy, audioService, gameModeService);

            _gameEventsInstaller?.RegisterEnemy(enemy);
        }

        private void SetupEnemy(
            BaseEnemyView enemy,
            IAudioService audioService,
            IGameModeService gameModeService)
        {
            if (enemy == null)
                return;

            enemy.InitializeAudio(audioService);

            var behaviour = enemy.GetComponent<BaseEnemyBehaviour>();
            if (behaviour == null)
                return;

            if (player != null)
                behaviour.SetPlayer(player.transform);

            behaviour.Initialize(gameModeService);
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
