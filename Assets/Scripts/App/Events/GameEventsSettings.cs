using App.Services.Spawn;
using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(
        fileName = "GameEventsSettings",
        menuName = "RPG/Events/Game Events Settings")]
    public class GameEventsSettings : ScriptableObject
    {
        [SerializeField] private int bossSpawnKillCount = 3;
        [SerializeField] private int victoryMusicKillCount = 5;
        [SerializeField] private AudioClip victoryMusic;
        [SerializeField] private EnemySpawnDefinition bossSpawnDefinition;

        public int BossSpawnKillCount => Mathf.Max(1, bossSpawnKillCount);
        public int VictoryMusicKillCount =>
            Mathf.Max(BossSpawnKillCount, victoryMusicKillCount);
        public AudioClip VictoryMusic => victoryMusic;
        public EnemySpawnDefinition BossSpawnDefinition =>
            bossSpawnDefinition;
    }
}
