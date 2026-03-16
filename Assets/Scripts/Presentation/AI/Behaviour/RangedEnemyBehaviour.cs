using UnityEngine;
using Presentation.Combat;

namespace Presentation.AI
{
    // Поведение врага дальнего боя.
    // Враг держит дистанцию от игрока и атакует магическими снарядами.
    public class RangedEnemyBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private float detectionRadius = 12f;

        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 10f;

        [SerializeField] private float moveSpeed = 3f;

        [SerializeField] private ProjectileView projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;

        private Presentation.Player.PlayerController _playerController;

        // Используется состояниями
        public ProjectileView ProjectilePrefab => projectilePrefab;
        public Transform ProjectileSpawnPoint => projectileSpawnPoint;
        public Presentation.Player.PlayerController PlayerController => _playerController;

        protected override void Start()
        {
            base.Start();

            // Получаем контроллер игрока
            _playerController = _player.GetComponent<Presentation.Player.PlayerController>();

            // Начальное состояние AI
            _stateMachine.ChangeState(new RangedIdleState(this));
        }

        // Создание и запуск магического снаряда
        public void ShootProjectile()
        {
            var projectile = Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.identity);

            Vector3 direction =
                Player.position - projectileSpawnPoint.position;

            direction.y = 0f;
            direction.Normalize();

            projectile.transform.forward = direction;

            var entity = EnemyView.GetEntity();
            var damage = entity.GetMagicalDamage();

            projectile.Initialize(damage, transform);
        }

        // Параметры поведения
        public override float DetectionRadius => detectionRadius;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public float MoveSpeed => moveSpeed;
    }
}