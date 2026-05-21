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
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // В peaceful не агримся
            if (_behaviour.AggroPolicy.CanAggro())
            {
                if (distance <= _behaviour.DetectionRadius)
                {
                    _behaviour.StateMachine.ChangeState(
                        _behaviour.StateFactory.CreateMaintainDistance(_behaviour));
                    return;
                }
            }

            // БЛУЖДАНИЕ (как у melee)
            _wanderTimer -= Time.deltaTime;

            var entity = _behaviour.EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            if (hpPercent < 0.3f)
            {
                if (_behaviour.CombatEvaluator.IsTargetDetected(
                    _behaviour.Self,
                    _behaviour.Player,
                    _behaviour.DetectionRadius))
                {
                    _behaviour.StateMachine.ChangeState(
                        _behaviour.StateFactory.CreateFlee(_behaviour));

                    return;
                }
            }

            if (_wanderTimer <= 0f)
            {
                _behaviour.WanderService.TryWander(
                    _behaviour.Agent,
                    _behaviour.Self,
                    5f);

                _wanderTimer = _wanderDelay;
            }
        }

        public void Exit() { }
    }
}