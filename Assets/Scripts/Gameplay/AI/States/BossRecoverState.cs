using UnityEngine;

namespace Presentation.AI
{
    // Состояние восстановления после тяжелой атаки
    // В этом состоянии босс делает паузу перед тем,
    // как снова начать преследование игрока
    public class BossRecoverState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        // Таймер восстановления
        private float _timer;

        public BossRecoverState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние восстановления
        public void Enter()
        {
            // Длительность восстановления
            _timer = 3f;

            var behaviour =
                _stateMachine.BossBehaviour;

            // Во время восстановления босс не двигается
            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            // Уменьшаем таймер восстановления
            _timer -= Time.deltaTime;

            // Когда восстановление закончилось,
            // возвращаемся к преследованию игрока
            if (_timer <= 0f)
            {
                _stateMachine.EnterChase();
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