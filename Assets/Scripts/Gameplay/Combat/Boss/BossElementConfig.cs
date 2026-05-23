using UnityEngine;

namespace Gameplay.Combat.Boss
{
    [CreateAssetMenu(
        fileName = "BossElementConfig",
        menuName = "RPG/Combat/Boss Element Config")]
    public class BossElementConfig : ScriptableObject
    {
        [SerializeField] private Color particleColor = Color.white;

        public Color ParticleColor => particleColor;
    }
}
