using UnityEngine;

namespace Presentation.AI
{
    // Состояние ожидания босса
    // В этом состоянии босс патрулирует территорию
    // и ожидает появления игрока
    public class BossIdleState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        // Таймер до следующего случайного перемещения
        private float _wanderTimer;

        // Интервал между перемещениями
        private float _wanderDelay = 3f;

        public BossIdleState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние ожидания
        public void Enter()
        {
            // Чтобы первое перемещение произошло сразу
            _wanderTimer = 0f;
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Если игрок обнаружен,
            // переходим к преследованию
            if (behaviour.ShouldDetectPlayer())
            {
                _stateMachine.EnterChase();
                return;
            }

            // Уменьшаем таймер ожидания
            _wanderTimer -= Time.deltaTime;

            // Когда таймер закончился,
            // выбираем новую случайную точку
            if (_wanderTimer <= 0f)
            {
                behaviour.WanderService.TryWander(
                    behaviour.Agent,
                    behaviour.Self,
                    6f);

                // Запускаем таймер заново
                _wanderTimer = _wanderDelay;
            }
        }

        // При выходе из состояния
        // дополнительных действий выполнять не нужно
        public void Exit() { }
    }
}