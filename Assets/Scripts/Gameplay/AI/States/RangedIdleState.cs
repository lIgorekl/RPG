namespace Presentation.AI
{
    // Состояние ожидания дальнего врага
    // В этом состоянии враг патрулирует территорию и ожидает появления игрока
    public class RangedIdleState : IEnemyState
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        // Таймер до следующего случайного перемещения
        private float _wanderTimer;

        // Задержка между перемещениями
        private float _wanderDelay = 2f;

        public RangedIdleState(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            // Сбрасываем таймер при входе в состояние
            _wanderTimer = 0f;
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.RangedBehaviour;

            // Если игрок обнаружен,
            // начинаем удерживать дистанцию для стрельбы
            if (behaviour.ShouldMaintainDistance())
            {
                _stateMachine.EnterMaintainDistance();
                return;
            }

            // Проверяем необходимость перехода в бегство
            if (behaviour.ShouldFlee())
            {
                // Убегаем только если игрок находится рядом
                if (behaviour.CombatEvaluator.IsTargetDetected(
                        behaviour.Self,
                        behaviour.Player,
                        behaviour.DetectionRadius))
                {
                    _stateMachine.EnterFlee();
                    return;
                }
            }

            // Выполняем патрулирование территории
            behaviour.UpdateIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit()
        {
            // Дополнительная логика при выходе не требуется
        }
    }
}