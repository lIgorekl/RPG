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

            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // NORMAL — агр
            if (mode == GameMode.Normal)
            {
                if (distance <= _behaviour.DetectionRadius)
                {
                    _behaviour.StateMachine.ChangeState(
                        new BossChaseState(_behaviour));
                    return;
                }
            }

            // PEACEFUL — только после удара
            if (mode == GameMode.Peaceful && _behaviour.IsActivated)
            {
                _behaviour.StateMachine.ChangeState(
                    new BossChaseState(_behaviour));
                return;
            }

            // БЛУЖДАНИЕ
            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0f)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector3 randomDirection =
                        Random.insideUnitSphere * 6f +
                        _behaviour.Self.position;

                    if (UnityEngine.AI.NavMesh.SamplePosition(
                        randomDirection,
                        out UnityEngine.AI.NavMeshHit hit,
                        6f,
                        UnityEngine.AI.NavMesh.AllAreas))
                    {
                        _behaviour.Agent.SetDestination(hit.position);
                        break;
                    }
                }

                _wanderTimer = _wanderDelay;
            }
        }

        public void Exit() { }
    }
}