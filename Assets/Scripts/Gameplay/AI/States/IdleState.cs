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
            var entity = _behaviour.EnemyView.GetEntity();
            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            if (_behaviour.AggroPolicy.CanAggro())
            {
                if (_behaviour.CombatEvaluator.IsTargetDetected(
                    _behaviour.Self,
                    _behaviour.Player,
                    _behaviour.DetectionRadius))
                {
                    _behaviour.StateMachine.ChangeState(
                        _behaviour.StateFactory.CreateChase(_behaviour));
                    return;
                }
            }

            _wanderTimer -= Time.deltaTime;

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