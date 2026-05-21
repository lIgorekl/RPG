using UnityEngine;
using Core.Combat;
using Presentation.Combat;

namespace Presentation.Player
{
    // Система боевых действий игрока.
    // Отвечает за ближнюю атаку (меч) и магическую атаку (снаряд).
    public class PlayerCombat
    {
        // Зависимости
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

        // Запускает ближнюю атаку (активирует хитбокс меча)
        public void MeleeAttack(Damage damage)
        {
            _swordHitbox.Initialize(damage);
            _swordHitbox.Activate();
        }

        // Выключает хитбокс после окончания атаки
        public void StopMelee()
        {
            _swordHitbox.Deactivate();
        }

        // Создаёт магический снаряд и задаёт ему направление
        public void CastMagic(Damage damage, Transform owner)
        {
            if (_projectilePrefab == null ||
                _spawnPoint == null ||
                _camera == null)
                return;

            Vector3 direction = _camera.transform.forward;

            direction.y = 0;
            direction.Normalize();

            Quaternion rotation = Quaternion.LookRotation(direction);

            var projectile = Object.Instantiate(
                _projectilePrefab,
                _spawnPoint.position,
                rotation);

            projectile.Initialize(damage, owner);
        }
    }
}