namespace Presentation.AI
{
    // Машина состояний дальнего врага
    // Управляет переключением между состояниями поведения стрелка
    public class RangedEnemyStateMachine
        : EnemyStateMachineBase
    {
        // Фабрика для создания состояний дальнего врага
        private readonly RangedEnemyStateFactory _factory;

        // Удобный доступ к владельцу машины состояний
        public RangedEnemyBehaviour RangedBehaviour =>
            (RangedEnemyBehaviour)Behaviour;

        public RangedEnemyStateMachine(
            RangedEnemyBehaviour behaviour)
            : base(behaviour)
        {
            // Создаем фабрику состояний
            _factory = new RangedEnemyStateFactory(this);
        }

        // Переводит врага в состояние ожидания
        public void EnterRangedIdle()
        {
            ChangeState(_factory.CreateIdle());
        }

        // Переводит врага в состояние удержания дистанции
        public void EnterMaintainDistance()
        {
            ChangeState(
                _factory.CreateMaintainDistance());
        }

        // Переводит врага в состояние атаки
        public void EnterRangedAttack()
        {
            ChangeState(_factory.CreateAttack());
        }

        // Переводит врага в состояние бегства
        public void EnterFlee()
        {
            ChangeState(_factory.CreateFlee());
        }

        // Переводит врага в состояние оглушения
        public void EnterStun(float duration)
        {
            ChangeState(
                _factory.CreateStun(duration));
        }

        // Возвращает врага в стартовое состояние
        public override void EnterDefaultState()
        {
            EnterRangedIdle();
        }
    }
}