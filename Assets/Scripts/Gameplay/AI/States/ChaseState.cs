namespace Presentation.AI
{
    // Состояние преследования игрока
    // В этом состоянии враг движется к игроку и пытается подойти на дистанцию атаки
    public class ChaseState : IEnemyState
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        public ChaseState(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            // Дополнительная подготовка при входе не требуется
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.EnemyBehaviour;

            // Если здоровье стало низким, переходим в бегство
            if (behaviour.ShouldFlee())
            {
                _stateMachine.EnterFlee();
                return;
            }

            // Если игрок вышел из радиуса обнаружения,
            // возвращаемся в состояние ожидания
            if (behaviour.ShouldReturnToIdle())
            {
                _stateMachine.EnterIdle();
                return;
            }

            // Если подошли достаточно близко,
            // переходим к атаке
            if (behaviour.ShouldAttackPlayer())
            {
                _stateMachine.EnterAttack();
                return;
            }

            // Продолжаем преследование игрока
            behaviour.UpdateChase();
        }

        public void Exit()
        {
            // Дополнительная логика при выходе не требуется
        }
    }
}