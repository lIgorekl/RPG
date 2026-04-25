namespace Presentation.AI
{
    // Простая машина состояний врага.
    // Управляет переключением и обновлением AI состояний.
    public class EnemyStateMachine
    {
        private IEnemyState _currentState;

        // Переключение состояния AI
        public void ChangeState(IEnemyState newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            _currentState?.Enter();
        }

        // Обновление текущего состояния
        public void Update()
        {
            _currentState?.Update();
        }
    }
}