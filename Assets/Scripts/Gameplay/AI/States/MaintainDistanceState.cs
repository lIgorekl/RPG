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
                    _behaviour.StateFactory.CreateRangedIdle(_behaviour));
                return;
            }

            // Направление к игроку
            Vector3 direction =
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;
            direction.Normalize();

            // Если игрок слишком близко — отходим назад
            if (_behaviour.CombatEvaluator.IsTooClose(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.MinDistance))
            {
                Vector3 target =
                    _behaviour.Self.position - direction * 2f;

                _behaviour.MovementService.MoveTo(
                    _behaviour.Agent,
                    target);
            }
            // Если игрок слишком далеко — подходим ближе
            else if (_behaviour.CombatEvaluator.IsTooFar(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.MaxDistance))
            {
                Vector3 target =
                    _behaviour.Self.position + direction * 2f;

                _behaviour.MovementService.MoveTo(
                    _behaviour.Agent,
                    target);
            }
            // Если дистанция подходящая — начинаем атаковать
            else
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateRangedAttack(_behaviour));
            }

            // Всегда поворачиваемся к игроку
            _behaviour.MovementService.RotateTo(
                _behaviour.Self,
                _behaviour.Player.position);
        }

        public void Exit() { }
    }
}