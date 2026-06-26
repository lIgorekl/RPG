using UnityEngine;

namespace Presentation.AI
{
    // Состояние преследования игрока
    // В этом состоянии босс движется к игроку,
    // пока не сможет атаковать или не потеряет цель
    public class BossChaseState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        public BossChaseState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние
        // Дополнительная инициализация не требуется
        public void Enter() { }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Если игрок покинул радиус обнаружения,
            // возвращаемся в состояние ожидания
            if (behaviour.ShouldReturnToIdle())
            {
                _stateMachine.EnterIdle();
                return;
            }

            // Если игрок находится в радиусе атаки,
            // выбираем тип следующей атаки
            if (behaviour.ShouldAttack())
            {
                // С вероятностью 50% выполняется тяжелая атака
                if (Random.value > 0.5f)
                {
                    _stateMachine.EnterHeavyAttack();
                }
                // Иначе выполняется обычная атака
                else
                {
                    _stateMachine.EnterAttack();
                }

                return;
            }

            // Поворачиваем босса лицом к игроку
            behaviour.MovementService.RotateTo(
                behaviour.Self,
                behaviour.Player.position);

            // Продолжаем движение к игроку
            behaviour.MovementService.MoveTo(
                behaviour.Agent,
                behaviour.Player.position);
        }

        // Вызывается при выходе из состояния
        // Дополнительных действий выполнять не требуется
        public void Exit() { }
    }
}