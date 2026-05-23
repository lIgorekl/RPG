using App.Services;
using UnityEngine;

namespace App.Events
{
    public sealed class VictoryMusicOnKillHandler
    {
        private readonly IGameEventBus _eventBus;
        private readonly GameEventsSettings _settings;
        private readonly IAudioService _audioService;
        private bool _victoryMusicPlayed;

        public VictoryMusicOnKillHandler(
            IGameEventBus eventBus,
            GameEventsSettings settings,
            IAudioService audioService)
        {
            _eventBus = eventBus;
            _settings = settings;
            _audioService = audioService;

            _eventBus.Subscribe<RegularEnemyKillCountChangedEvent>(
                OnKillCountChanged);
        }

        private void OnKillCountChanged(
            RegularEnemyKillCountChangedEvent countEvent)
        {
            if (_victoryMusicPlayed)
                return;

            if (countEvent.KillCount < _settings.VictoryMusicKillCount)
                return;

            var clip = _settings.VictoryMusic;
            if (clip == null)
            {
                Debug.LogWarning(
                    "VictoryMusicOnKillHandler: VictoryMusic is not set.");
                return;
            }

            _victoryMusicPlayed = true;

            Debug.Log(
                $"VictoryMusicOnKillHandler: playing victory music " +
                $"(kill count {countEvent.KillCount}).");

            _audioService.PlayMusicOnce(clip);
            _eventBus.Publish(new VictoryMusicPlayedEvent());
        }
    }
}
