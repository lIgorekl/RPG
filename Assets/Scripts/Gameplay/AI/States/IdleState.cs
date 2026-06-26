namespace Presentation.AI
{
    // Состояние ожидания ближнего врага
    // В этом состоянии враг стоит на месте или патрулирует территорию
    public class IdleState : IEnemyState
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        // Таймер для запуска случайного перемещения
        private float _wanderTimer;

        // Задержка между перемещениями
        private float _wanderDelay = 3f;

        public IdleState(
            MeleeEnemyStateMachine stateMachine)
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
                _stateMachine.EnemyBehaviour;

            // Если игрок обнаружен, начинаем преследование
            if (behaviour.ShouldChasePlayer())
            {
                _stateMachine.EnterChase();
                return;
            }

            // Проверяем необходимость перехода в бегство
            if (behaviour.ShouldFlee())
            {
                // Бегство запускается только если игрок находится рядом
                if (behaviour.CombatEvaluator
                    .IsTargetDetected(
                        behaviour.Self,
                        behaviour.Player,
                        behaviour.DetectionRadius))
                {
                    _stateMachine.EnterFlee();
                    return;
                }
            }

            // Выполняем логику ожидания и случайного перемещения
            behaviour.UpdateIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit()
        {
            // Дополнительных действий при выходе нет
        }
    }
}