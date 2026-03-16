using UnityEngine;

namespace Presentation.AI
{
    // Общий интерфейс поведения врага.
    // Используется системами AI (например EnemyWander),
    // чтобы работать с любым типом врага.
    public interface IEnemyBehaviour
    {
        Transform Player { get; }
        float DetectionRadius { get; }
    }
}