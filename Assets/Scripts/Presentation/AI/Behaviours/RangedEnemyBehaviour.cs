using UnityEngine;
using Presentation.Combat;
using Gameplay.Combat.Weapons;
using Core.Combat;

namespace Presentation.AI
{
    // Поведение врага дальнего боя.
    // Враг держит дистанцию от игрока и атакует магическими снарядами.
    public class RangedEnemyBehaviour : BaseCombatEnemyBehaviour,
        IEnemyWeaponHolder,
        IEnemyWeaponAssignable
    {
        [SerializeField] private float detectionRadius = 12f;

        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 10f;

        [SerializeField] private float moveSpeed = 3f;

        [SerializeField] private ProjectileView projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private EnemyWeaponLoadout weaponLoadout;

        private Presentation.Player.PlayerController _playerController;
        private EnemyWeapon _currentWeapon;

        public EnemyWeapon CurrentWeapon => _currentWeapon;
        public float AttackCooldown =>
            _currentWeapon?.AttackCooldown ?? 2f;

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

        // Используется состояниями
        public ProjectileView ProjectilePrefab => projectilePrefab;
        public Transform ProjectileSpawnPoint => projectileSpawnPoint;
        public Presentation.Player.PlayerController PlayerController => _playerController;

        protected override void Start()
        {
            if (_currentWeapon == null)
                AssignRandomWeapon(new System.Random());

            _stateMachine =
                new RangedEnemyStateMachine(this);

            base.Start();

            _playerController =
                _player.GetComponent<Presentation.Player.PlayerController>();

            ((RangedEnemyStateMachine)_stateMachine)
                .EnterRangedIdle();
        }

        // Создание и запуск магического снаряда
        public void PerformAttack()
        {
            var prefab = ResolveProjectilePrefab();
            if (prefab == null)
                return;

            var projectile = Instantiate(
                prefab,
                projectileSpawnPoint.position,
                Quaternion.identity);

            Vector3 direction =
                Player.position - projectileSpawnPoint.position;

            direction.y = 0f;
            direction.Normalize();

            projectile.transform.forward = direction;

            var entity = EnemyView.GetEntity();
            var baseDamage = entity.GetMagicalDamage();
            float multiplier =
                _currentWeapon?.DamageMultiplier ?? 1f;

            var damage = new Damage(
                Mathf.RoundToInt(baseDamage.Value * multiplier),
                baseDamage.Type);

            projectile.Initialize(damage, transform);
        }

        private ProjectileView ResolveProjectilePrefab()
        {
            if (_currentWeapon?.ProjectilePrefab != null)
            {
                return _currentWeapon.ProjectilePrefab
                    .GetComponent<ProjectileView>();
            }

            return projectilePrefab;
        }

        // Параметры поведения
        public override float DetectionRadius => detectionRadius;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public float MoveSpeed => moveSpeed;

        public bool ShouldReturnToRangedIdle()
        {
            return !CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        public bool ShouldStartRangedAttack()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance >= MinDistance &&
                distance <= MaxDistance;
        }

        public void UpdateMaintainDistance()
        {
            MovementService.MaintainDistance(
                Agent,
                Self,
                Player,
                MinDistance,
                MaxDistance,
                MoveSpeed);

            RotateToPlayer();
        }

        public bool ShouldStopRangedAttack()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance < MinDistance ||
                distance > MaxDistance;
        }

        public void EnterRangedAttack()
        {
            if (Agent != null)
            {
                Agent.isStopped = true;
            }
        }

        public void ExitRangedAttack()
        {
            if (Agent != null)
            {
                Agent.isStopped = false;
            }
        }

        public void UpdateRangedAttack()
        {
            RotateToPlayer();

            PerformAttack();
        }

        public bool ShouldMaintainDistance()
        {
            if (!AggroPolicy.CanAggro())
                return false;

            return CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        public override void EnterStunState(float duration)
        {
            ((RangedEnemyStateMachine)_stateMachine)
                .EnterStun(duration);
        }
    }
}
