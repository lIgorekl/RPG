using UnityEngine;
using Core.Combat;
using Presentation.Combat;

namespace Presentation.Player
{
    // Система боевых действий игрока
    // Выполняет ближние и магические атаки
    public class PlayerCombat
    {
        // Зависимости для создания и выполнения атак
        private Camera _camera;
        private Transform _spawnPoint;
        private ProjectileView _projectilePrefab;
        private SwordHitbox _swordHitbox;

        public PlayerCombat(
            Camera camera,
            Transform spawnPoint,
            ProjectileView projectilePrefab,
            SwordHitbox swordHitbox)
        {
            _camera = camera;
            _spawnPoint = spawnPoint;
            _projectilePrefab = projectilePrefab;
            _swordHitbox = swordHitbox;
        }

        // Выполняет ближнюю атаку через хитбокс меча
        public void MeleeAttack(Damage damage)
        {
            // Передаем урон в хитбокс меча
            _swordHitbox.Initialize(damage);

            // Включаем регистрацию попаданий
            _swordHitbox.Activate();
        }

        // Завершает ближнюю атаку
        public void StopMelee()
        {
            _swordHitbox.Deactivate();
        }

        // Создает магический снаряд
        public void CastMagic(Damage damage, Transform owner)
        {
            // Проверяем наличие необходимых объектов
            if (_projectilePrefab == null ||
                _spawnPoint == null ||
                _camera == null)
                return;

            // Получаем направление взгляда камеры
            Vector3 direction = _camera.transform.forward;

            // Убираем вертикальную составляющую
            direction.y = 0;

            direction.Normalize();

            // Рассчитываем поворот снаряда
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Создаем объект снаряда на сцене
            var projectile = Object.Instantiate(
                _projectilePrefab,
                _spawnPoint.position,
                rotation);

            // Передаем снаряду данные об уроне и владельце
            projectile.Initialize(damage, owner);
        }
    }
}