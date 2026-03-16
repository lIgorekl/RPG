namespace Presentation.AI
{
    // Интерфейс состояния AI врага.
    // Каждое состояние реализует логику поведения (Idle, Chase, Attack и т.д.).
    public interface IEnemyState
    {
        void Enter();
        void Update();
        void Exit();
    }
}