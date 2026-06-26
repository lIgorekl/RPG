using UnityEngine;
using App.Services;

namespace App
{
    // Точка входа в игру
    // Создает и хранит основные сервисы, которые используются во всем проекте
    public class GameEntryPoint : MonoBehaviour
    {
        // Источники звука для музыки и эффектов
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        // Музыка главного меню
        [SerializeField] private AudioClip menuMusic;

        // Единственный экземпляр объекта в игре
        private static GameEntryPoint _instance;

        // Основные сервисы приложения
        private IAudioService _audioService;
        private ISceneService _sceneService;
        private IGameModeService _gameModeService;
        private AudioSettingsService _audioSettingsService;

        // Публичный доступ к сервисам
        public ISceneService SceneService => _sceneService;
        public static GameEntryPoint Instance => _instance;
        public IAudioService GetAudioService() => _audioService;
        public IGameModeService GetGameModeService() => _gameModeService;

        private void Awake()
        {
            // Проверяем существует ли уже объект GameEntryPoint
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            // Сохраняем единственный экземпляр
            _instance = this;

            // Не уничтожаем объект при смене сцены
            DontDestroyOnLoad(gameObject);

            // Сохраняем аудиоисточники между сценами
            PersistAudioSources();

            // Создаем и настраиваем сервисы
            Initialize();
        }

        // Сохраняет аудиообъекты между сценами
        private void PersistAudioSources()
        {
            if (musicSource != null)
                DontDestroyOnLoad(musicSource.gameObject);

            if (sfxSource != null)
                DontDestroyOnLoad(sfxSource.gameObject);
        }

        // Создает все основные сервисы игры
        private void Initialize()
        {
            _sceneService = new SceneService();
            _audioService = new AudioService();
            _gameModeService = new GameModeService();
            _audioSettingsService = new AudioSettingsService();

            // Передаем AudioSource в аудиосервис
            _audioService.Initialize(
                musicSource,
                sfxSource);

            // Запускаем музыку главного меню
            _audioService.PlayMusic(menuMusic);
        }

        // Возвращает сервис настроек звука
        public AudioSettingsService GetAudioSettingsService()
        {
            return _audioSettingsService;
        }
    }
}