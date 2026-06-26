namespace Presentation.AI
{
    // Базовая машина состояний врага
    // Управляет текущим состоянием и переключением между состояниями
    public abstract class EnemyStateMachineBase
    {
        // Владелец машины состояний
        public BaseEnemyBehaviour Behaviour { get; }

        // Текущее активное состояние
        protected IEnemyState _currentState;

        public IEnemyState CurrentState => _currentState;

        protected EnemyStateMachineBase(
            BaseEnemyBehaviour behaviour)
        {
            Behaviour = behaviour;
        }

        // Переключает врага в новое состояние
        protected void ChangeState(IEnemyState newState)
        {
            // Вызываем логику выхода из предыдущего состояния
            _currentState?.Exit();

            // Сохраняем новое состояние
            _currentState = newState;

            // Вызываем логику входа в новое состояние
            _currentState?.Enter();
        }

        // Обновляет текущее состояние
        public void Update()
        {
            _currentState?.Update();
        }

        // Переход в стартовое состояние
        public abstract void EnterDefaultState();
    }
}