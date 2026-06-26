using UnityEngine;
using Presentation.Combat;
using Gameplay.Combat.Weapons;
using Core.Combat;

namespace Presentation.AI
{
    // Поведение врага дальнего боя
    // Управляет дистанцией до игрока и атаками снарядами
    public class RangedEnemyBehaviour : BaseCombatEnemyBehaviour,
        IEnemyWeaponHolder,
        IEnemyWeaponAssignable
    {
        // Радиус обнаружения игрока
        [SerializeField] private float detectionRadius = 12f;

        // Допустимая дистанция для ведения боя
        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 10f;

        // Скорость перемещения врага
        [SerializeField] private float moveSpeed = 3f;

        // Префаб снаряда и точка его создания
        [SerializeField] private ProjectileView projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;

        // Набор доступного оружия врага
        [SerializeField] private EnemyWeaponLoadout weaponLoadout;

        private Presentation.Player.PlayerController _playerController;

        // Текущее оружие врага
        private EnemyWeapon _currentWeapon;

        public EnemyWeapon CurrentWeapon => _currentWeapon;

        // Время перезарядки текущего оружия
        public float AttackCooldown =>
            _currentWeapon?.AttackCooldown ?? 2f;

        // Устанавливает конкретное оружие
        public void SetWeapon(EnemyWeaponConfig config)
        {
            _currentWeapon =
                config != null ? new EnemyWeapon(config) : null;
        }

        // Выбирает случайное оружие из доступного набора
        public void AssignRandomWeapon(System.Random random)
        {
            if (weaponLoadout == null)
                return;

            SetWeapon(weaponLoadout.PickRandom(random));
        }

        // Доступ к данным для состояний AI
        public ProjectileView ProjectilePrefab => projectilePrefab;
        public Transform ProjectileSpawnPoint => projectileSpawnPoint;
        public Presentation.Player.PlayerController PlayerController => _playerController;

        protected override void Start()
        {
            // Если оружие не назначено заранее, выбираем случайное
            if (_currentWeapon == null)
                AssignRandomWeapon(new System.Random());

            // Создаем машину состояний дальнего врага
            _stateMachine =
                new RangedEnemyStateMachine(this);

            base.Start();

            // Получаем ссылку на игрока
            _playerController =
                _player.GetComponent<Presentation.Player.PlayerController>();

            // Запускаем начальное состояние ожидания
            ((RangedEnemyStateMachine)_stateMachine)
                .EnterRangedIdle();
        }

        // Создает и запускает магический снаряд
        public void PerformAttack()
        {
            var prefab = ResolveProjectilePrefab();

            if (prefab == null)
                return;

            var projectile = Instantiate(
                prefab,
                projectileSpawnPoint.position,
                Quaternion.identity);

            // Вычисляем направление на игрока
            Vector3 direction =
                Player.position - projectileSpawnPoint.position;

            // Игнорируем разницу по высоте
            direction.y = 0f;

            direction.Normalize();

            // Поворачиваем снаряд в сторону игрока
            projectile.transform.forward = direction;

            var entity = EnemyView.GetEntity();

            // Получаем базовый магический урон врага
            var baseDamage = entity.GetMagicalDamage();

            // Получаем множитель урона оружия
            float multiplier =
                _currentWeapon?.DamageMultiplier ?? 1f;

            // Рассчитываем итоговый урон с учетом оружия
            var damage = new Damage(
                Mathf.RoundToInt(baseDamage.Value * multiplier),
                baseDamage.Type);

            // Передаем урон и владельца снаряду
            projectile.Initialize(damage, transform);
        }

        // Определяет какой префаб снаряда использовать
        private ProjectileView ResolveProjectilePrefab()
        {
            // Если оружие задает собственный снаряд, используем его
            if (_currentWeapon?.ProjectilePrefab != null)
            {
                return _currentWeapon.ProjectilePrefab
                    .GetComponent<ProjectileView>();
            }

            // Иначе используем стандартный снаряд врага
            return projectilePrefab;
        }

        // Параметры поведения врага
        public override float DetectionRadius => detectionRadius;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public float MoveSpeed => moveSpeed;

        // Проверяет потерял ли враг игрока
        public bool ShouldReturnToRangedIdle()
        {
            return !CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        // Проверяет находится ли игрок на дистанции атаки
        public bool ShouldStartRangedAttack()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance >= MinDistance &&
                distance <= MaxDistance;
        }

        // Поддерживает оптимальную дистанцию до игрока
        public void UpdateMaintainDistance()
        {
            MovementService.MaintainDistance(
                Agent,
                Self,
                Player,
                MinDistance,
                MaxDistance,
                MoveSpeed);

            // Во время движения продолжаем смотреть на игрока
            RotateToPlayer();
        }

        // Проверяет нужно ли прекратить атаку
        public bool ShouldStopRangedAttack()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance < MinDistance ||
                distance > MaxDistance;
        }

        // Подготавливает врага к стрельбе
        public void EnterRangedAttack()
        {
            if (Agent != null)
            {
                // Останавливаем перемещение во время атаки
                Agent.isStopped = true;
            }
        }

        // Возвращает возможность двигаться
        public void ExitRangedAttack()
        {
            if (Agent != null)
            {
                Agent.isStopped = false;
            }
        }

        // Выполняет атаку по игроку
        public void UpdateRangedAttack()
        {
            // Перед атакой всегда смотрим на цель
            RotateToPlayer();

            PerformAttack();
        }

        // Проверяет нужно ли поддерживать дистанцию
        public bool ShouldMaintainDistance()
        {
            // Если агрессия запрещена, ничего не делаем
            if (!AggroPolicy.CanAggro())
                return false;

            return CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        // Переводит врага в состояние оглушения
        public override void EnterStunState(float duration)
        {
            ((RangedEnemyStateMachine)_stateMachine)
                .EnterStun(duration);
        }
    }
}