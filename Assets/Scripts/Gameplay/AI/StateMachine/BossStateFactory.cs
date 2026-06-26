namespace Presentation.AI
{
    // Фабрика состояний босса
    // Отвечает за создание всех состояний машины состояний босса
    public class BossStateFactory
    {
        // Машина состояний, которой будут принадлежать создаваемые состояния
        private readonly BossStateMachine _stateMachine;

        public BossStateFactory(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Создает состояние ожидания
        public BossIdleState CreateIdle()
        {
            return new BossIdleState(_stateMachine);
        }

        // Создает состояние преследования игрока
        public BossChaseState CreateChase()
        {
            return new BossChaseState(_stateMachine);
        }

        // Создает состояние обычной атаки
        public BossAttackState CreateAttack()
        {
            return new BossAttackState(_stateMachine);
        }

        // Создает состояние тяжелой атаки
        public BossHeavyAttackState CreateHeavyAttack()
        {
            return new BossHeavyAttackState(_stateMachine);
        }

        // Создает состояние восстановления после тяжелой атаки
        public BossRecoverState CreateRecover()
        {
            return new BossRecoverState(_stateMachine);
        }

        // Создает состояние оглушения
        public BossStunState CreateStun(float duration)
        {
            return new BossStunState(
                _stateMachine,
                duration);
        }

        // Создает состояние ярости
        public BossEnrageState CreateEnrage()
        {
            return new BossEnrageState(_stateMachine);
        }
    }
}