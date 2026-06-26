using App.Services;
using UnityEngine;

namespace App.Events
{
    // Обработчик воспроизведения победной музыки.
    // Следит за количеством убийств обычных врагов
    // и запускает музыку после достижения
    // заданного условия.
    public sealed class VictoryMusicOnKillHandler
    {
        // Шина игровых событий
        private readonly IGameEventBus _eventBus;

        // Настройки игровых событий
        private readonly GameEventsSettings _settings;

        // Сервис воспроизведения звука
        private readonly IAudioService _audioService;

        // Флаг,
        // предотвращающий повторное воспроизведение
        private bool _victoryMusicPlayed;

        public VictoryMusicOnKillHandler(
            IGameEventBus eventBus,
            GameEventsSettings settings,
            IAudioService audioService)
        {
            _eventBus = eventBus;
            _settings = settings;
            _audioService = audioService;

            // Подписываемся на изменение
            // количества убитых обычных врагов
            _eventBus.Subscribe<RegularEnemyKillCountChangedEvent>(
                OnKillCountChanged);
        }

        // Вызывается после изменения
        // количества убийств
        private void OnKillCountChanged(
            RegularEnemyKillCountChangedEvent countEvent)
        {
            // Если музыка уже была воспроизведена,
            // повторно ничего не делаем
            if (_victoryMusicPlayed)
                return;

            // Если нужное количество убийств
            // еще не достигнуто,
            // продолжаем ждать
            if (countEvent.KillCount <
                _settings.VictoryMusicKillCount)
            {
                return;
            }

            // Получаем победную музыку
            var clip =
                _settings.VictoryMusic;

            // Если музыка не назначена,
            // выводим предупреждение
            if (clip == null)
            {
                Debug.LogWarning(
                    "VictoryMusicOnKillHandler: VictoryMusic is not set.");
                return;
            }

            // Запоминаем,
            // что музыка уже воспроизводилась
            _victoryMusicPlayed = true;

            // Сообщение для отладки
            Debug.Log(
                $"VictoryMusicOnKillHandler: playing victory music " +
                $"(kill count {countEvent.KillCount}).");

            // Запускаем музыку
            _audioService.PlayMusicOnce(clip);

            // Сообщаем системе,
            // что победная музыка воспроизведена
            _eventBus.Publish(
                new VictoryMusicPlayedEvent());
        }
    }
}