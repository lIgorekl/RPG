using UnityEngine;

namespace Presentation.AI
{
    // Состояние тяжелой атаки босса
    // Выполняет мощный удар с большой задержкой,
    // после которого босс переходит в восстановление
    public class BossHeavyAttackState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        // Таймер подготовки тяжелой атаки
        private float _timer;

        public BossHeavyAttackState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние
        public void Enter()
        {
            // Подготовка тяжелой атаки длится 3 секунды
            _timer = 3f;

            var behaviour =
                _stateMachine.BossBehaviour;

            // Во время подготовки босс не двигается
            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Если игрок вышел из радиуса атаки,
            // прекращаем подготовку и начинаем преследование
            if (!behaviour.ShouldAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            // Уменьшаем таймер подготовки.
            // В фазе ярости подготовка проходит быстрее
            _timer -=
                Time.deltaTime *
                behaviour.AttackSpeedMultiplier;

            // Когда подготовка закончилась
            if (_timer <= 0f)
            {
                // Проверяем, что игрок все еще рядом
                if (!behaviour.ShouldAttack())
                {
                    _stateMachine.EnterChase();
                    return;
                }

                // Выполняем тяжелую атаку
                behaviour.EnemyView.Attack(
                    behaviour.Player,
                    behaviour.HeavyAttackDamageMultiplier,
                    true);

                // После удара переходим в состояние восстановления
                _stateMachine.EnterRecover();
            }
        }

        // Вызывается при выходе из состояния
        public void Exit()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Возобновляем движение босса
            behaviour.MovementService.Resume(
                behaviour.Agent);
        }
    }
}