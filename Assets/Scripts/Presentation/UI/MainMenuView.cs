using UnityEngine;
using App;

namespace Presentation.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private UnityEngine.UI.Slider musicSlider;
        [SerializeField] private UnityEngine.UI.Slider sfxSlider;

        private MainMenuController _controller;

        private void Awake()
        {
            var entryPoint = GameEntryPoint.Instance;

            _controller = new MainMenuController(
                entryPoint.SceneService,
                entryPoint.GetAudioService(),
                entryPoint.GetGameModeService()
            );
        }

        private void Start()
        {
            var audio = GameEntryPoint.Instance.GetAudioService();

            musicSlider.SetValueWithoutNotify(audio.GetMusicVolume());
            sfxSlider.SetValueWithoutNotify(audio.GetSfxVolume());
        }

        public void PlayNormal()
        {
            _controller.StartNormalGame();
        }

        public void PlayPeaceful()
        {
            _controller.StartPeacefulGame();
        }

        public void OpenSettings()
        {
            _controller.OpenSettings(() =>
            {
                mainPanel.SetActive(false);
                settingsPanel.SetActive(true);
            });
        }

        public void CloseSettings()
        {
            _controller.CloseSettings(() =>
            {
                settingsPanel.SetActive(false);
                mainPanel.SetActive(true);
            });
        }

        public void OnMusicVolumeChanged(float value)
        {
            _controller.ChangeMusicVolume(value);
        }

        public void OnSfxVolumeChanged(float value)
        {
            _controller.ChangeSfxVolume(value);
        }
    }
}