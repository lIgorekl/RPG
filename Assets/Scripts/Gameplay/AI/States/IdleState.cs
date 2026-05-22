using UnityEngine;
using App.Services;

namespace Presentation.AI
{
    public class IdleState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;

        private float _wanderTimer;
        private float _wanderDelay = 2f;

        public IdleState(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            _behaviour.TickIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit() { }
    }
}