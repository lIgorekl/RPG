using UnityEngine;

namespace Presentation.AI
{
    // Состояние оглушения босса
    // Во время оглушения босс не может двигаться и выполнять атаки
    public class BossStunState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        // Таймер длительности оглушения
        private float _timer;

        public BossStunState(
            BossStateMachine stateMachine,
            float duration)
        {
            _stateMachine = stateMachine;

            // Запоминаем длительность оглушения
            _timer = duration;
        }

        // Вызывается при входе в состояние оглушения
        public void Enter()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Останавливаем движение босса
            behaviour.MovementService.Stop(
                behaviour.Agent);

            // Запускаем анимацию получения урона
            if (behaviour.EnemyView.Animator != null)
            {
                behaviour.EnemyView.Animator
                    .SetTrigger("Hurt");
            }
        }

        public void Update()
        {
            // Уменьшаем время оглушения
            _timer -= Time.deltaTime;

            // После окончания оглушения
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