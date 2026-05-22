using UnityEngine;

namespace Presentation.AI
{
    // Поведение врага ближнего боя.
    // Использует state machine из BaseEnemyBehaviour.
    public class EnemyBehaviour : BaseCombatEnemyBehaviour
    {
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float attackRadius = 2f;
        [SerializeField] private float moveSpeed = 3f;

        protected override void Start()
        {
            _stateMachine =
                new MeleeEnemyStateMachine();

            base.Start();

            var meleeStateMachine =
                (MeleeEnemyStateMachine)_stateMachine;

            meleeStateMachine.EnterIdle(this);
        }

        // Радиус, на котором враг начинает реагировать на игрока
        public override float DetectionRadius => detectionRadius;

        // Дистанция атаки
        public float AttackRadius => attackRadius;

        // Скорость перемещения
        public float MoveSpeed => moveSpeed;

        public bool ShouldAttackPlayer()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance <= AttackRadius;
        }

        public bool ShouldReturnToIdle()
        {
            return !CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        public void UpdateChase()
        {
            MovementService.MoveTo(
                Agent,
                Player.position);

            RotateToPlayer();
        }

        public bool ShouldChasePlayer()
        {
            if (!AggroPolicy.CanAggro())
                return false;

            return CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        public bool ShouldStopAttack()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance > AttackRadius;
        }

        public void EnterAttack()
        {
            if (Agent != null)
            {
                Agent.isStopped = true;
            }
        }

        public void ExitAttack()
        {
            if (Agent != null)
            {
                Agent.isStopped = false;
            }
        }

        public void UpdateAttack()
        {
            RotateToPlayer();

            EnemyView.Attack(Player);
        }

        public override void EnterStunState(float duration)
        {
            ((MeleeEnemyStateMachine)_stateMachine)
                .EnterStun(this, duration);
        }
    }
}