using UnityEngine;
using App.Services;

namespace Presentation.AI
{
    public class BossIdleState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _wanderTimer;
        private float _wanderDelay = 3f;

        public BossIdleState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            var mode = _behaviour.GameModeService.CurrentMode;

            // NORMAL — агр
            if (mode == GameMode.Normal)
            {
                if (_behaviour.CombatEvaluator.IsTargetDetected(
                    _behaviour.Self,
                    _behaviour.Player,
                    _behaviour.DetectionRadius))
                {
                    _behaviour.StateMachine.ChangeState(
                        _behaviour.StateFactory.CreateBossChase(_behaviour));
                    return;
                }
            }

            // PEACEFUL — только после удара
            if (mode == GameMode.Peaceful && _behaviour.IsActivated)
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateBossChase(_behaviour));
                return;
            }

            // БЛУЖДАНИЕ
            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0f)
            {
                _behaviour.WanderService.TryWander(
                    _behaviour.Agent,
                    _behaviour.Self,
                    6f);

                _wanderTimer = _wanderDelay;
            }
        }

        public void Exit() { }
    }
}