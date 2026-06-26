using UnityEngine;

namespace Presentation.AI
{
    // Состояние оглушения врага
    // Временно блокирует движение и действия врага
    public class StunState : IEnemyState
    {
        private readonly EnemyStateMachineBase _stateMachine;

        // Удобный доступ к поведению боевого врага
        private BaseCombatEnemyBehaviour Behaviour =>
            (BaseCombatEnemyBehaviour)_stateMachine.Behaviour;

        // Оставшееся время оглушения
        private float _timer;

        public StunState(
            EnemyStateMachineBase stateMachine,
            float duration)
        {
            _stateMachine = stateMachine;

            // Сохраняем длительность оглушения
            _timer = duration;
        }

        public void Enter()
        {
            // Останавливаем врага при входе в состояние
            Behaviour.EnterStun();
        }

        public void Update()
        {
            // Уменьшаем таймер оглушения
            _timer -= Time.deltaTime;

            // После окончания оглушения
            // возвращаемся в обычное состояние
            if (_timer <= 0f)
            {
                _stateMachine.EnterDefaultState();
            }
        }

        public void Exit()
        {
            // Возвращаем возможность двигаться
            Behaviour.ExitStun();
        }
    }
}
