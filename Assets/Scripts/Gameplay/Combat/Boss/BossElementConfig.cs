using UnityEngine;

namespace Gameplay.Combat.Boss
{
    // Конфигурация стихии босса.
    // Хранится как ScriptableObject и содержит параметры,
    // определяющие внешний вид выбранной стихии.
    [CreateAssetMenu(
        fileName = "BossElementConfig",
        menuName = "RPG/Combat/Boss Element Config")]
    public class BossElementConfig : ScriptableObject
    {
        // Цвет визуальных эффектов стихии
        // (например, частиц или свечения)
        [SerializeField] private Color particleColor = Color.white;

        // Возвращает цвет выбранной стихии
        public Color ParticleColor => particleColor;
    }
}