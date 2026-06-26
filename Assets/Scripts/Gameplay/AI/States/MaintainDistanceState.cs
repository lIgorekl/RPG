namespace Presentation.AI
{
    // Состояние удержания дистанции для дальнего врага
    // В этом состоянии враг старается держаться на удобном расстоянии от игрока
    public class MaintainDistanceState : IEnemyState
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        public MaintainDistanceState(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            // Дополнительная подготовка не требуется
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.RangedBehaviour;

            // При низком здоровье переходим в бегство
            if (behaviour.ShouldFlee())
            {
                _stateMachine.EnterFlee();
                return;
            }

            // Если игрок потерян, возвращаемся в ожидание
            if (behaviour.ShouldReturnToRangedIdle())
            {
                _stateMachine.EnterRangedIdle();
                return;
            }

            // Если игрок находится на подходящей дистанции,
            // переходим к атаке
            if (behaviour.ShouldStartRangedAttack())
            {
                _stateMachine.EnterRangedAttack();
                return;
            }

            // Продолжаем поддерживать оптимальную дистанцию до игрока
            behaviour.UpdateMaintainDistance();
        }

        public void Exit()
        {
            // Дополнительная логика при выходе не требуется
        }
    }
}