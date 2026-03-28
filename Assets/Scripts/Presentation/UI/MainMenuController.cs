using App;

namespace Presentation.UI
{
    public class MainMenuController
    {
        public void StartGame()
        {
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
    }
}