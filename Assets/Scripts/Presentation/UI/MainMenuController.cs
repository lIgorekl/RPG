using App;
using App.Services;

namespace Presentation.UI
{
    public class MainMenuController
    {
        private readonly ISceneService _sceneService;
        private readonly IAudioService _audioService;
        private readonly IGameModeService _gameModeService;
        private readonly AudioSettingsService _audioSettingsService;

        public MainMenuController(
            ISceneService sceneService,
            IAudioService audioService,
            IGameModeService gameModeService,
            AudioSettingsService audioSettingsService)
        {
            _sceneService = sceneService;
            _audioService = audioService;
            _gameModeService = gameModeService;
            _audioSettingsService = audioSettingsService;
        }
        public void StartNormalGame()
        {
            _gameModeService.SetMode(GameMode.Normal);

            _sceneService.LoadGame();
        }

        public void StartPeacefulGame()
        {
            _gameModeService.SetMode(GameMode.Peaceful);

            _sceneService.LoadGame();
        }

        public void OpenSettings(System.Action onOpen)
        {
            onOpen?.Invoke();
        }

        public void CloseSettings(System.Action onClose)
        {
            onClose?.Invoke();
        }

        public void ChangeMusicVolume(float value)
        {
            _audioSettingsService.SetMusicVolume(value);
        }

        public void ChangeSfxVolume(float value)
        {
            _audioSettingsService.SetSFXVolume(value);
        }

        public void SetNormalMode()
        {
            _gameModeService.SetMode(GameMode.Normal);
        }

        public void SetPeacefulMode()
        {
            _gameModeService.SetMode(GameMode.Peaceful);
        }
    }
}