using UnityEngine;
using Gameplay.Combat.Weapons;

namespace Presentation.AI
{
    // Поведение врага ближнего боя.
    // Использует state machine из BaseEnemyBehaviour.
    public class EnemyBehaviour : BaseCombatEnemyBehaviour,
        IEnemyWeaponHolder,
        IEnemyWeaponAssignable
    {
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float attackRadius = 2f;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private EnemyWeaponLoadout weaponLoadout;

        private EnemyWeapon _currentWeapon;

        public EnemyWeapon CurrentWeapon => _currentWeapon;
        public float AttackCooldown =>
            _currentWeapon?.AttackCooldown ?? 1f;

        public void SetWeapon(EnemyWeaponConfig config)
        {
            _currentWeapon =
                config != null ? new EnemyWeapon(config) : null;
        }

        public void AssignRandomWeapon(System.Random random)
        {
            if (weaponLoadout == null)
                return;

            SetWeapon(weaponLoadout.PickRandom(random));
        }

        protected override void Start()
        {
            if (_currentWeapon == null)
                AssignRandomWeapon(new System.Random());

            _stateMachine =
                new MeleeEnemyStateMachine(this);

            base.Start();

            ((MeleeEnemyStateMachine)_stateMachine)
                .EnterIdle();
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

            if (_currentWeapon != null)
            {
                EnemyView.Attack(
                    Player,
                    _currentWeapon.DamageMultiplier,
                    _currentWeapon.UseHeavyAttackAnimation);
            }
            else
            {
                EnemyView.Attack(Player);
            }
        }

        public override void EnterStunState(float duration)
        {
            ((MeleeEnemyStateMachine)_stateMachine)
                .EnterStun(duration);
        }
    }
}
