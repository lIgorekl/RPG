using App;
using App.Services;

namespace Presentation.UI
{
    public class MainMenuController
    {
        public void StartNormalGame()
        {
            GameEntryPoint.Instance
                .GetGameModeService()
                .SetMode(GameMode.Normal);

            GameEntryPoint.Instance.SceneService.LoadGame();
        }

        public void StartPeacefulGame()
        {
            GameEntryPoint.Instance
                .GetGameModeService()
                .SetMode(GameMode.Peaceful);

            GameEntryPoint.Instance.SceneService.LoadGame();
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
            GameEntryPoint.Instance
                .GetAudioService()
                .SetMusicVolume(value);
        }

        public void ChangeSfxVolume(float value)
        {
            GameEntryPoint.Instance
                .GetAudioService()
                .SetSfxVolume(value);
        }

        public void SetNormalMode()
        {
            GameEntryPoint.Instance
                .GetGameModeService()
                .SetMode(GameMode.Normal);
        }

        public void SetPeacefulMode()
        {
            GameEntryPoint.Instance
                .GetGameModeService()
                .SetMode(GameMode.Peaceful);
        }
    }
}