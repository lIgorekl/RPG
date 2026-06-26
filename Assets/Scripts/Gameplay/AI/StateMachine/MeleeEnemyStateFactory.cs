namespace Presentation.AI
{
    // Фабрика состояний ближнего врага
    // Создает объекты состояний для машины состояний
    public class MeleeEnemyStateFactory
    {
        // Машина состояний, для которой создаются состояния
        private readonly MeleeEnemyStateMachine _stateMachine;

        public MeleeEnemyStateFactory(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Создает состояние ожидания
        public IdleState CreateIdle()
        {
            return new IdleState(_stateMachine);
        }

        // Создает состояние преследования игрока
        public ChaseState CreateChase()
        {
            return new ChaseState(_stateMachine);
        }

        // Создает состояние атаки
        public AttackState CreateAttack()
        {
            return new AttackState(_stateMachine);
        }

        // Создает состояние бегства
        public FleeState CreateFlee()
        {
            return new FleeState(_stateMachine);
        }

        // Создает состояние оглушения
        public StunState CreateStun(float duration)
        {
            return new StunState(
                _stateMachine,
                duration);
        }
    }
}