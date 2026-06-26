using UnityEngine;

namespace Presentation.AI
{
    // Общий интерфейс поведения врага
    // Определяет минимальный набор данных,
    // необходимых для работы общих AI-систем
    public interface IEnemyBehaviour
    {
        // Ссылка на игрока
        Transform Player { get; }

        // Радиус обнаружения игрока
        float DetectionRadius { get; }

        // NavMeshAgent, управляющий перемещением врага
        UnityEngine.AI.NavMeshAgent Agent { get; }

        // Transform самого врага
        Transform Self { get; }
    }
}