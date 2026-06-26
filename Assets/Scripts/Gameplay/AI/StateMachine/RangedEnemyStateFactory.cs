namespace Presentation.AI
{
    // Фабрика состояний дальнего врага
    // Создает объекты состояний для машины состояний стрелка
    public class RangedEnemyStateFactory
    {
        // Машина состояний, для которой создаются состояния
        private readonly RangedEnemyStateMachine _stateMachine;

        public RangedEnemyStateFactory(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Создает состояние ожидания
        public RangedIdleState CreateIdle()
        {
            return new RangedIdleState(_stateMachine);
        }

        // Создает состояние удержания дистанции
        public MaintainDistanceState CreateMaintainDistance()
        {
            return new MaintainDistanceState(_stateMachine);
        }

        // Создает состояние атаки
        public RangedAttackState CreateAttack()
        {
            return new RangedAttackState(_stateMachine);
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