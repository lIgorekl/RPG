using App.Services.Spawn;
using UnityEngine;

namespace App.Events
{
    // Настройки игровых событий.
    // Хранятся как ScriptableObject и позволяют
    // настраивать условия появления босса
    // и воспроизведения победной музыки.
    [CreateAssetMenu(
        fileName = "GameEventsSettings",
        menuName = "RPG/Events/Game Events Settings")]
    public class GameEventsSettings : ScriptableObject
    {
        // Количество убийств,
        // необходимое для появления босса
        [SerializeField]
        private int bossSpawnKillCount = 3;

        // Количество убийств,
        // необходимое для воспроизведения победной музыки
        [SerializeField]
        private int victoryMusicKillCount = 5;

        // Музыкальная композиция,
        // проигрываемая после достижения условия
        [SerializeField]
        private AudioClip victoryMusic;

        // Описание босса,
        // которое будет использоваться при спавне
        [SerializeField]
        private EnemySpawnDefinition bossSpawnDefinition;

        // Возвращает количество убийств
        // для появления босса.
        // Значение не может быть меньше 1.
        public int BossSpawnKillCount =>
            Mathf.Max(1, bossSpawnKillCount);

        // Возвращает количество убийств
        // для победной музыки.
        // Гарантирует, что музыка не включится
        // раньше появления босса.
        public int VictoryMusicKillCount =>
            Mathf.Max(
                BossSpawnKillCount,
                victoryMusicKillCount);

        // Музыкальная композиция победы
        public AudioClip VictoryMusic =>
            victoryMusic;

        // Конфигурация босса
        public EnemySpawnDefinition BossSpawnDefinition =>
            bossSpawnDefinition;
    }
}