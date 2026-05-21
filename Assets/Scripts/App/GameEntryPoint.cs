using UnityEngine;
using App.Services;

namespace App
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip menuMusic;
        private static GameEntryPoint _instance;

        private IAudioService _audioService;
        private ISceneService _sceneService;
        private IGameModeService _gameModeService;
        private AudioSettingsService _audioSettingsService;

        public ISceneService SceneService => _sceneService;
        public static GameEntryPoint Instance => _instance;
        public IAudioService GetAudioService() => _audioService;
        public IGameModeService GetGameModeService() => _gameModeService;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            _sceneService = new SceneService();
            _audioService = new AudioService();
            _gameModeService = new GameModeService();
            _audioSettingsService = new AudioSettingsService();
            _audioService.Initialize(musicSource, sfxSource);
            _audioService.PlayMusic(menuMusic);
        }

        public AudioSettingsService GetAudioSettingsService()
        {
            return _audioSettingsService;
        }
    }
}