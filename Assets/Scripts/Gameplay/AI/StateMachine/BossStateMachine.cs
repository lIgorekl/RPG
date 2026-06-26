namespace Presentation.AI
{
    // Машина состояний босса
    // Управляет переключением между всеми состояниями поведения босса
    public class BossStateMachine
        : EnemyStateMachineBase
    {
        // Фабрика для создания состояний босса
        private readonly BossStateFactory _factory;

        // Удобный доступ к поведению босса
        public BossBehaviour BossBehaviour =>
            (BossBehaviour)Behaviour;

        public BossStateMachine(
            BossBehaviour behaviour)
            : base(behaviour)
        {
            // Создаем фабрику состояний
            _factory = new BossStateFactory(this);
        }

        // Переводит босса в состояние ожидания
        public void EnterIdle()
        {
            ChangeState(_factory.CreateIdle());
        }

        // Переводит босса в состояние преследования игрока
        public void EnterChase()
        {
            ChangeState(_factory.CreateChase());
        }

        // Переводит босса в состояние обычной атаки
        public void EnterAttack()
        {
            ChangeState(_factory.CreateAttack());
        }

        // Переводит босса в состояние тяжелой атаки
        public void EnterHeavyAttack()
        {
            ChangeState(_factory.CreateHeavyAttack());
        }

        // Переводит босса в состояние восстановления после тяжелой атаки
        public void EnterRecover()
        {
            ChangeState(_factory.CreateRecover());
        }

        // Переводит босса в состояние оглушения
        public void EnterStun(float duration)
        {
            ChangeState(
                _factory.CreateStun(duration));
        }

        // Переводит босса в состояние ярости
        public void EnterEnrage()
        {
            ChangeState(_factory.CreateEnrage());
        }

        // Начальное состояние машины состояний
        public override void EnterDefaultState()
        {
            EnterIdle();
        }
    }
}