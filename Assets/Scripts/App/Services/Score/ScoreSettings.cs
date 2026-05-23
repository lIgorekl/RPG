using UnityEngine;

namespace App.Services.Score
{
    [CreateAssetMenu(
        fileName = "ScoreSettings",
        menuName = "RPG/Score/Score Settings")]
    public class ScoreSettings : ScriptableObject
    {
        [SerializeField] private int regularEnemyPoints = 10;
        [SerializeField] private int bossPoints = 50;

        public int RegularEnemyPoints => Mathf.Max(0, regularEnemyPoints);
        public int BossPoints => Mathf.Max(0, bossPoints);
    }
}
