using UnityEngine;

namespace Presentation.AI
{
    // Состояние перехода босса в фазу ярости
    // В этом состоянии босс временно приостанавливает бой,
    // включает режим ярости и затем возвращается к преследованию игрока
    public class BossEnrageState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        // Таймер длительности перехода в ярость
        private float _timer;

        public BossEnrageState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние
        public void Enter()
        {
            // Длительность перехода в ярость
            _timer = 3f;

            // Включаем режим ярости
            _stateMachine.BossBehaviour.SetEnraged(true);
        }

        public void Update()
        {
            // Уменьшаем таймер
            _timer -= Time.deltaTime;

            // После завершения перехода
            // возвращаемся к преследованию игрока
            if (_timer <= 0f)
            {
                _stateMachine.EnterChase();
            }
        }

        // При выходе дополнительных действий не требуется
        public void Exit() { }
    }
}