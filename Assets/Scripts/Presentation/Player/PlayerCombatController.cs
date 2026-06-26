using UnityEngine;
using UnityEngine.InputSystem;
using Core.Gameplay;
using Gameplay.Characters;

namespace Presentation.Player
{
    // Контроллер боевой системы игрока
    // Обрабатывает атаки, кулдауны и поворот персонажа перед атакой
    public class PlayerCombatController
    {
        // Игровая сущность игрока
        private readonly PlayerEntity _player;

        // Подсистема выполнения атак
        private readonly PlayerCombat _combat;

        // Unity компоненты для анимации и направления атаки
        private readonly Animator _animator;
        private readonly Camera _camera;
        private readonly Transform _owner;

        // Кулдауны магической и ближней атаки
        private readonly Cooldown _magicCooldown;
        private readonly Cooldown _meleeCooldown;

        // Флаг активной ближней атаки
        private bool _isAttacking;

        public bool IsMagicOnCooldown =>
            _magicCooldown.IsActive;

        public float MagicCooldownProgress =>
            _magicCooldown.Progress;

        public bool IsAttacking => _isAttacking;

        // События для уведомления UI о начале и окончании кулдауна
        public event System.Action MagicCooldownStarted;
        public event System.Action MagicCooldownFinished;

        public PlayerCombatController(
            PlayerEntity player,
            PlayerCombat combat,
            Animator animator,
            Camera camera,
            Transform owner,
            float magicCooldown,
            float meleeCooldown)
        {
            _player = player;
            _combat = combat;

            _animator = animator;
            _camera = camera;
            _owner = owner;

            // Создаем объекты кулдаунов для атак
            _magicCooldown =
                new Cooldown(magicCooldown);

            _meleeCooldown =
                new Cooldown(meleeCooldown);
        }

        // Обновляет состояние боевой системы
        public void Update(PlayerInputData input)
        {
            UpdateCooldowns();

            HandleMelee(input);

            HandleMagic(input);
        }

        // Обновляет кулдауны атак
        private void UpdateCooldowns()
        {
            bool wasActive =
                _magicCooldown.IsActive;

            _magicCooldown.Update(Time.deltaTime);

            // Сообщаем о завершении кулдауна магии
            if (wasActive &&
                !_magicCooldown.IsActive)
            {
                MagicCooldownFinished?.Invoke();
            }

            _meleeCooldown.Update(Time.deltaTime);

            // Завершаем состояние атаки после окончания кулдауна
            if (!_meleeCooldown.IsActive &&
                _isAttacking)
            {
                _isAttacking = false;
                _combat.StopMelee();
            }
        }

        // Обрабатывает ближнюю атаку
        private void HandleMelee(PlayerInputData input)
        {
            if (!input.MeleeAttackPressed)
                return;

            // Не даем атаковать во время кулдауна
            if (_meleeCooldown.IsActive)
                return;

            // Поворачиваем игрока в сторону камеры
            RotateTowardsCamera();

            // Запускаем анимацию атаки
            if (_animator != null)
                _animator.SetTrigger("Attack");

            _isAttacking = true;

            // Выполняем атаку с текущим физическим уроном
            _combat.MeleeAttack(
                _player.GetPhysicalDamage());

            _meleeCooldown.Start();
        }

        // Обрабатывает магическую атаку
        private void HandleMagic(PlayerInputData input)
        {
            if (!input.MagicAttackPressed)
                return;

            // Не даем использовать магию во время кулдауна
            if (_magicCooldown.IsActive)
                return;

            RotateTowardsCamera();

            // Создаем магическую атаку
            _combat.CastMagic(
                _player.GetMagicalDamage(),
                _owner);

            _magicCooldown.Start();

            MagicCooldownStarted?.Invoke();
        }

        // Поворачивает игрока в направлении взгляда камеры
        private void RotateTowardsCamera()
        {
            if (_camera == null)
                return;

            Vector3 forward =
                _camera.transform.forward;

            // Убираем наклон по вертикали
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.01f)
                return;

            Quaternion rotation =
                Quaternion.LookRotation(forward);

            _owner.rotation = rotation;
        }
    }
}