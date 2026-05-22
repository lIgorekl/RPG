using UnityEngine;
using App.Services;

namespace Presentation.AI
{
    // Состояние удержания дистанции для дальнего врага.
    // Враг старается держаться между MinDistance и MaxDistance от игрока.
    public class MaintainDistanceState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;

        public MaintainDistanceState(RangedEnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            _behaviour.TickMaintainDistance();
        }

        public void Exit() { }
    }
}