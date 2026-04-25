using UnityEngine;

namespace Presentation.AI
{
    // Состояние ожидания дальнего врага.
    // Враг ничего не делает, пока игрок не войдёт в радиус обнаружения.
    public class RangedIdleState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;

        public RangedIdleState(RangedEnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // Если игрок замечен — начинаем удерживать дистанцию
            if (distance <= _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new MaintainDistanceState(_behaviour));
            }
        }

        public void Exit() { }
    }
}