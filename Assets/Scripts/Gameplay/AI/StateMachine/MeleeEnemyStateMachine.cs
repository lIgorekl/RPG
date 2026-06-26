namespace Presentation.AI
{
    // Машина состояний ближнего врага
    // Управляет переключением между состояниями поведения
    public class MeleeEnemyStateMachine
        : EnemyStateMachineBase
    {
        // Фабрика для создания состояний врага
        private readonly MeleeEnemyStateFactory _factory;

        // Удобный доступ к владельцу машины состояний
        public EnemyBehaviour EnemyBehaviour =>
            (EnemyBehaviour)Behaviour;

        public MeleeEnemyStateMachine(
            EnemyBehaviour behaviour)
            : base(behaviour)
        {
            // Создаем фабрику состояний
            _factory = new MeleeEnemyStateFactory(this);
        }

        // Переводит врага в состояние ожидания
        public void EnterIdle()
        {
            ChangeState(_factory.CreateIdle());
        }

        // Переводит врага в состояние преследования игрока
        public void EnterChase()
        {
            ChangeState(_factory.CreateChase());
        }

        // Переводит врага в состояние атаки
        public void EnterAttack()
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

        // Возвращает врага в состояние по умолчанию
        public override void EnterDefaultState()
        {
            EnterIdle();
        }
    }
}