using UnityEngine;
using App.Services;

namespace Presentation.AI
{
    public class RangedIdleState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;

        private float _wanderTimer;
        private float _wanderDelay = 2f;

        public RangedIdleState(RangedEnemyBehaviour behaviour)
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