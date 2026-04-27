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
            var entity = _behaviour.EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // В peaceful не агримся
            if (_behaviour.GameModeService.CurrentMode != GameMode.Peaceful)
            {
                if (distance <= _behaviour.DetectionRadius)
                {
                    _behaviour.StateMachine.ChangeState(
                        new MaintainDistanceState(_behaviour));
                    return;
                }
            }

            // БЛУЖДАНИЕ (как у melee)
            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0f)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector3 randomDirection =
                        Random.insideUnitSphere * 5f +
                        _behaviour.Self.position;

                    if (UnityEngine.AI.NavMesh.SamplePosition(
                        randomDirection,
                        out UnityEngine.AI.NavMeshHit hit,
                        5f,
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