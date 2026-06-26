using UnityEngine;
using App;

namespace Presentation.UI
{
    // Представление главного меню игры
    // Обрабатывает нажатия кнопок и взаимодействие с настройками
    public class MainMenuView : MonoBehaviour
    {
        // Панели главного меню и настроек
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;

        // Слайдеры громкости музыки и звуков
        [SerializeField] private UnityEngine.UI.Slider musicSlider;
        [SerializeField] private UnityEngine.UI.Slider sfxSlider;

        private MainMenuController _controller;

        private void Awake()
        {
            var entryPoint = GameEntryPoint.Instance;

            // Создаем контроллер главного меню
            _controller = new MainMenuController(
                entryPoint.SceneService,
                entryPoint.GetAudioService(),
                entryPoint.GetGameModeService(),
                entryPoint.GetAudioSettingsService()
            );
        }

        private void Start()
        {
            var audioSettings =
                GameEntryPoint.Instance.GetAudioSettingsService();

            // Загружаем сохраненные значения громкости в UI
            musicSlider.SetValueWithoutNotify(
                audioSettings.GetMusicVolume());

            sfxSlider.SetValueWithoutNotify(
                audioSettings.GetSFXVolume());
        }

        // Запускает игру в обычном режиме
        public void PlayNormal()
        {
            _controller.StartNormalGame();
        }

        // Запускает игру в мирном режиме
        public void PlayPeaceful()
        {
            _controller.StartPeacefulGame();
        }

        // Открывает окно настроек
        public void OpenSettings()
        {
            _controller.OpenSettings(() =>
            {
                mainPanel.SetActive(false);
                settingsPanel.SetActive(true);
            });
        }

        // Закрывает окно настроек
        public void CloseSettings()
        {
            _controller.CloseSettings(() =>
            {
                settingsPanel.SetActive(false);
                mainPanel.SetActive(true);
            });
        }

        // Изменяет громкость музыки
        public void OnMusicVolumeChanged(float value)
        {
            _controller.ChangeMusicVolume(value);
        }

        // Изменяет громкость звуковых эффектов
        public void OnSfxVolumeChanged(float value)
        {
            _controller.ChangeSfxVolume(value);
        }
    }
}