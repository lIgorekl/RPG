using UnityEngine;

namespace Presentation.AI
{
    // Состояние бегства врага
    // Активируется при низком уровне здоровья и заставляет врага убегать от игрока
    public class FleeState : IEnemyState
    {
        private readonly EnemyStateMachineBase _stateMachine;

        // Удобный доступ к поведению боевого врага
        private BaseCombatEnemyBehaviour Behaviour =>
            (BaseCombatEnemyBehaviour)_stateMachine.Behaviour;

        // Таймер для периодического пересчета направления бегства
        private float _recalculateTimer;

        // Дистанция, на которую враг пытается убежать
        private const float FleeDistance = 10f;

        // Интервал между пересчетами маршрута
        private const float RecalculateDelay = 1.5f;

        public FleeState(
            EnemyStateMachineBase stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            // Подготавливаем врага к состоянию бегства
            Behaviour.EnterFlee();

            // Сразу разрешаем первый расчет маршрута
            _recalculateTimer = 0f;
        }

        public void Update()
        {
            var entity = Behaviour.EnemyView.GetEntity();

            // Вычисляем текущий процент здоровья
            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            // Если здоровье восстановилось,
            // возвращаемся к обычному поведению
            if (hpPercent >= 0.3f)
            {
                _stateMachine.EnterDefaultState();
                return;
            }

            // Уменьшаем таймер пересчета маршрута
            _recalculateTimer -= Time.deltaTime;

            // Перестраиваем маршрут если прошло достаточно времени
            // или враг застрял на месте
            if (_recalculateTimer <= 0f ||
                Behaviour.Agent.velocity.magnitude < 0.1f)
            {
                // Получаем направление от игрока к врагу
                Vector3 direction =
                    Behaviour.Self.position -
                    Behaviour.Player.position;

                // Если враг находится слишком близко к игроку,
                // выбираем случайное направление
                if (direction.sqrMagnitude < 0.01f)
                {
                    direction =
                        Random.insideUnitSphere;

                    direction.y = 0f;
                }

                direction.Normalize();

                // Пытаемся найти точку для бегства
                Behaviour.MovementService.TryMoveToRandomDirection(
                    Behaviour.Agent,
                    Behaviour.Self,
                    direction,
                    FleeDistance);

                // Перезапускаем таймер пересчета
                _recalculateTimer =
                    RecalculateDelay;
            }

            // Дистанция, после которой считаем игрока потерянным
            float fleeExitDistance =
                Behaviour.DetectionRadius * 1.5f;

            // Если игрок слишком далеко,
            // завершаем состояние бегства
            if (Behaviour.CombatEvaluator.IsTargetLost(
                Behaviour.Self,
                Behaviour.Player,
                fleeExitDistance))
            {
                _stateMachine.EnterDefaultState();
            }
        }

        public void Exit()
        {
            // Дополнительная логика при выходе не требуется
        }
    }
}
