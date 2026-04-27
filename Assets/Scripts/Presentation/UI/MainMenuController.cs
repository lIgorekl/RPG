using App;
using App.Services;

namespace Presentation.UI
{
    public class MainMenuController
    {
        private readonly ISceneService _sceneService;
        private readonly IAudioService _audioService;
        private readonly IGameModeService _gameModeService;

        public MainMenuController(
            ISceneService sceneService,
            IAudioService audioService,
            IGameModeService gameModeService)
        {
            _sceneService = sceneService;
            _audioService = audioService;
            _gameModeService = gameModeService;
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
            _audioService.SetMusicVolume(value);
        }

        public void ChangeSfxVolume(float value)
        {
            _audioService.SetSfxVolume(value);
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