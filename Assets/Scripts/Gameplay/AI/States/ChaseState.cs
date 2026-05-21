using UnityEngine;
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
            var entity = _behaviour.EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            if (hpPercent < 0.3f)
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateFlee(_behaviour));

                return;
            }

            // Если игрок слишком далеко — возвращаемся в Idle
            if (_behaviour.CombatEvaluator.IsTargetLost(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.DetectionRadius))
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateIdle(_behaviour));
                return;
            }

            // Если игрок рядом — начинаем атаку
            if (_behaviour.CombatEvaluator.CanMeleeAttack(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.AttackRadius))
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateAttack(_behaviour));
                return;
            }

            // Направление на игрока
            _behaviour.MovementService.RotateTo(
                _behaviour.Self,
                _behaviour.Player.position);

            // Движение через NavMesh
            _behaviour.MovementService.MoveTo(
                _behaviour.Agent,
                _behaviour.Player.position);
        }

        public void Exit() { }
    }
}