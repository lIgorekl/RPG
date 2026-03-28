using UnityEngine;
using App.Services;

namespace App
{
    public class GameEntryPoint : MonoBehaviour
    {
        private static GameEntryPoint _instance;

        private ISaveService _saveService;
        private IAudioService _audioService;
        private ISceneService _sceneService;

        public ISceneService SceneService => _sceneService;
        public static GameEntryPoint Instance => _instance;
        public ISaveService GetSaveService() => _saveService;
        public IAudioService GetAudioService() => _audioService;

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
            _saveService = new SaveService();
            _audioService = new AudioService();
        }
    }
}