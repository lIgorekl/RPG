using App.Services;

namespace Presentation.AI
{
    // Состояние преследования игрока.
    // Враг бежит за игроком, пока тот находится в радиусе обнаружения.
    public class ChaseState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;

        public ChaseState(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            _behaviour.TickChase();
        }

        public void Exit() { }
    }
}