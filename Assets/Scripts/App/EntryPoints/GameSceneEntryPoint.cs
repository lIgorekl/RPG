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
    // Точка входа игровой сцены
    // Инициализирует игрока, врагов, сервисы сохранения, события и систему очков
    public class GameSceneEntryPoint : MonoBehaviour, IGameEnemyRegistry
    {
        // Игрок и враги, размещенные на сцене
        [SerializeField] private PlayerController player;
        [SerializeField] private BaseEnemyView[] enemies;

        [Header("Enemy Spawning")]

        // Настройки системы спавна врагов
        [SerializeField] private bool enableEnemySpawning = true;
        [SerializeField] private SpawnPoint[] spawnPoints;
        [SerializeField] private EnemySpawnCatalog spawnCatalog;
        [SerializeField] private EnemySpawnSettings spawnSettings;

        [Header("Game Events")]

        // Настройки игровых событий
        [SerializeField] private GameEventsSettings gameEventsSettings;

        [Header("Score")]

        // Настройки системы очков
        [SerializeField] private ScoreSettings scoreSettings;
        [SerializeField] private ScoreboardView scoreboardView;

        // Список всех зарегистрированных врагов
        private readonly List<BaseEnemyView> _registeredEnemies = new();

        private SaveLoadInteractor _saveLoadInteractor;
        private ISaveService _saveService;
        private IGameEventBus _eventBus;
        private GameEventsInstaller _gameEventsInstaller;

        private void Awake()
        {
            Initialize();
        }

        // Выполняет инициализацию всех игровых систем
        private void Initialize()
        {
            Debug.Log("GameSceneEntryPoint Initialize CALLED");

            var audioService =
                GameEntryPoint.Instance.GetAudioService();

            var gameModeService =
                GameEntryPoint.Instance.GetGameModeService();

            // Создаем систему сохранений
            IPlayerRepository repository = new JsonPlayerRepository();

            _saveLoadInteractor =
                new SaveLoadInteractor(repository);

            _saveService =
                new SaveService(_saveLoadInteractor);

            // Подключаем аудио игроку
            if (player != null)
            {
                player.InitializeAudio(audioService);
            }

            _registeredEnemies.Clear();

            // Регистрируем врагов со сцены
            RegisterSceneEnemies();

            // Создаем случайных врагов через систему спавна
            SpawnAndRegisterEnemies(gameModeService);

            // Настраиваем всех зарегистрированных врагов
            foreach (var enemy in _registeredEnemies)
                SetupEnemy(enemy, audioService, gameModeService);

            // Запускаем систему игровых событий
            InitializeGameEvents(audioService);

            Debug.Log("Enemies count: " + _registeredEnemies.Count);
            Debug.Log("SaveService CREATED");
        }

        // Инициализирует систему игровых событий
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

            // Создаем шину событий
            _eventBus = new GameEventBus();

            // Создаем систему подсчета очков
            if (scoreSettings != null)
                new ScoreService(_eventBus, scoreSettings);

            // Устанавливаем игровые события
            _gameEventsInstaller = new GameEventsInstaller(
                _eventBus,
                gameEventsSettings,
                audioService,
                this,
                spawnPoints,
                player);

            // Регистрируем всех врагов в системе событий
            foreach (var enemy in _registeredEnemies)
                _gameEventsInstaller.RegisterEnemy(enemy);

            InitializeScoreboard();
        }

        // Подключает UI таблицы очков
        private void InitializeScoreboard()
        {
            if (scoreboardView == null)
                scoreboardView = FindFirstObjectByType<ScoreboardView>();

            if (scoreboardView == null || _eventBus == null)
                return;

            scoreboardView.Initialize(_eventBus);
        }

        // Регистрирует врага, созданного во время игры
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

        // Настраивает врага после создания
        private void SetupEnemy(
            BaseEnemyView enemy,
            IAudioService audioService,
            IGameModeService gameModeService)
        {
            if (enemy == null)
                return;

            enemy.InitializeAudio(audioService);

            var behaviour =
                enemy.GetComponent<BaseEnemyBehaviour>();

            if (behaviour == null)
                return;

            // Передаем врагу ссылку на игрока (чтоб знал, кого атаковать)
            if (player != null)
                behaviour.SetPlayer(player.transform);

            // Инициализируем AI врага (мирная/нормальная агрессия)
            behaviour.Initialize(gameModeService);
        }

        // Регистрирует врагов, уже размещенных на сцене
        private void RegisterSceneEnemies()
        {
            if (enemies == null)
                return;

            foreach (var enemy in enemies)
                RegisterEnemy(enemy);
        }

        // Создает случайных врагов через систему спавна
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

            // Формируем контекст спавна
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

        // Добавляет врага в общий список
        private void RegisterEnemy(BaseEnemyView enemy)
        {
            if (enemy == null || _registeredEnemies.Contains(enemy))
                return;

            _registeredEnemies.Add(enemy);
        }

        // Возвращает сервис сохранений
        public ISaveService GetSaveService()
        {
            return _saveService;
        }

        // Возвращает игрока
        public PlayerController GetPlayer()
        {
            return player;
        }

        // Возвращает всех зарегистрированных врагов
        public BaseEnemyView[] GetEnemies()
        {
            return _registeredEnemies.ToArray();
        }

        // Загружает сохраненное состояние врагов
        public void ApplyEnemiesSaveData(List<EnemySaveData> enemiesData)
        {
            // Сначала скрываем всех врагов
            foreach (var enemy in _registeredEnemies)
            {
                if (enemy == null)
                    continue;

                enemy.gameObject.SetActive(false);
            }

            // Затем восстанавливаем врагов из сохранения
            foreach (var enemyData in enemiesData)
            {
                foreach (var enemy in _registeredEnemies)
                {
                    if (enemy == null)
                        continue;

                    // Ищем врага по уникальному идентификатору
                    if (enemy.GetId() != enemyData.Id)
                        continue;

                    enemy.ApplySaveData(enemyData);

                    break;
                }
            }
        }
    }
}