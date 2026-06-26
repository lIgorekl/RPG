using App;
using App.Services;

namespace Presentation.UI
{
    // Контроллер главного меню
    // Обрабатывает действия пользователя и взаимодействует с сервисами игры
    public class MainMenuController
    {
        // Сервис загрузки сцен
        private readonly ISceneService _sceneService;

        // Сервис воспроизведения звука
        private readonly IAudioService _audioService;

        // Сервис управления режимом игры
        private readonly IGameModeService _gameModeService;

        // Сервис настроек звука
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

        // Запускает игру в обычном режиме
        public void StartNormalGame()
        {
            _gameModeService.SetMode(GameMode.Normal);

            _sceneService.LoadGame();
        }

        // Запускает игру в мирном режиме
        public void StartPeacefulGame()
        {
            _gameModeService.SetMode(GameMode.Peaceful);

            _sceneService.LoadGame();
        }

        // Выполняет действие открытия настроек
        public void OpenSettings(System.Action onOpen)
        {
            onOpen?.Invoke();
        }

        // Выполняет действие закрытия настроек
        public void CloseSettings(System.Action onClose)
        {
            onClose?.Invoke();
        }

        // Изменяет громкость музыки
        public void ChangeMusicVolume(float value)
        {
            _audioSettingsService.SetMusicVolume(value);
        }

        // Изменяет громкость звуковых эффектов
        public void ChangeSfxVolume(float value)
        {
            _audioSettingsService.SetSFXVolume(value);
        }

        // Устанавливает обычный режим игры
        public void SetNormalMode()
        {
            _gameModeService.SetMode(GameMode.Normal);
        }

        // Устанавливает мирный режим игры
        public void SetPeacefulMode()
        {
            _gameModeService.SetMode(GameMode.Peaceful);
        }
    }
}