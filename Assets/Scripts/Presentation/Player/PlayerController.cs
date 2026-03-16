using UnityEngine;
using Gameplay.Characters;
using Gameplay.Stats;
using UnityEngine.InputSystem;
using Core.Combat;
using Presentation.Combat;
using Core.Gameplay;

namespace Presentation.Player
{
    // Главный контроллер игрока.
    // Координирует движение, атаки, кулдауны и получение урона.
    public class PlayerController : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] private int maxHP = 100;
        [SerializeField] private int physicalDamage = 20;
        [SerializeField] private int magicalDamage = 15;

        [SerializeField] private Camera playerCamera;

        [SerializeField] private float attackDistance = 5f;
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private float stunDuration = 0.2f;

        [SerializeField] private ProjectileView magicProjectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private SwordHitbox swordHitbox;

        [SerializeField] private float meleeAttackDistance = 2f;

        [SerializeField] private float magicCooldown = 2f;
        [SerializeField] private float meleeCooldown = 0.6f;

        // Подсистемы игрока
        private PlayerMovement _movement;
        private PlayerCombat _combat;

        // Кулдауны атак
        private Cooldown _magicCooldown;
        private Cooldown _meleeCooldown;

        // Используется UI системой
        public bool IsMagicOnCooldown => _magicCooldown.IsActive;
        public float MagicCooldownProgress => _magicCooldown.Progress;

        public event System.Action MagicCooldownStarted;
        public event System.Action MagicCooldownFinished;

        // Состояние игрока
        private float _stunTimer;
        private bool _isStunned;
        private bool _isAttacking;
        private bool _isDead;

        // Unity компоненты
        private CharacterController _characterController;
        private Animator _animator;

        // Доменная сущность игрока (геймплейная логика)
        private PlayerEntity _player;

        private void Awake()
        {
            // Получаем Unity компоненты
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();

            // Инициализация систем игрока
            _movement = new PlayerMovement(
                _characterController,
                playerCamera,
                _animator,
                walkSpeed,
                runSpeed,
                rotationSpeed);

            _combat = new PlayerCombat(
                playerCamera,
                projectileSpawnPoint,
                magicProjectilePrefab,
                swordHitbox);

            // Кулдауны
            _magicCooldown = new Cooldown(magicCooldown);
            _meleeCooldown = new Cooldown(meleeCooldown);

            // Создаём игровую сущность игрока
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);
            _player = new PlayerEntity(stats);

            // Подписка на события игрока
            _player.Died += OnPlayerDied;
            _player.DamageReceived += OnDamageReceived;

            _player.HealthChanged += (current, max) =>
            {
                Debug.Log($"Player HP changed: {current}/{max}");
            };
        }

        private void Update()
        {
            if (_isDead) return;

            // Обработка стана
            if (_isStunned)
            {
                _stunTimer -= Time.deltaTime;

                if (_stunTimer <= 0f)
                    _isStunned = false;

                return;
            }

            // Обновление кулдауна магии
            bool wasActive = _magicCooldown.IsActive;
            _magicCooldown.Update(Time.deltaTime);

            if (wasActive && !_magicCooldown.IsActive)
                MagicCooldownFinished?.Invoke();

            // Обновление кулдауна ближней атаки
            _meleeCooldown.Update(Time.deltaTime);

            if (!_meleeCooldown.IsActive && _isAttacking)
            {
                _isAttacking = false;
                _combat.StopMelee();
            }

            // Движение игрока
            _movement.Update();

            var mouse = Mouse.current;
            if (mouse == null || playerCamera == null) return;

            // Ближняя атака
            if (mouse.leftButton.wasPressedThisFrame && !_meleeCooldown.IsActive)
            {
                RotateTowardsCamera();

                if (_animator != null)
                    _animator.SetTrigger("Attack");

                _isAttacking = true;

                _combat.MeleeAttack(_player.GetPhysicalDamage());

                _meleeCooldown.Start();
            }

            // Магическая атака
            if (mouse.rightButton.wasPressedThisFrame && !_magicCooldown.IsActive)
            {
                RotateTowardsCamera();

                _combat.CastMagic(
                    _player.GetMagicalDamage(),
                    transform);

                _magicCooldown.Start();
                MagicCooldownStarted?.Invoke();
            }
        }

        // Возвращает доменную сущность игрока
        public PlayerEntity GetEntity()
        {
            return _player;
        }

        // Поворот игрока в направлении камеры (перед атакой)
        private void RotateTowardsCamera()
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            cameraForward.y = 0f;

            if (cameraForward.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = targetRotation;
            }
        }

        // Вызывается при смерти игрока
        private void OnPlayerDied()
        {
            _isDead = true;

            if (_animator != null)
                _animator.SetTrigger("Death");

            Debug.Log("Game Over: Player died");
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.Died -= OnPlayerDied;

            _player.DamageReceived -= OnDamageReceived;
        }

        // Реакция на получение урона
        private void OnDamageReceived(Damage damage)
        {
            _isStunned = true;
            _stunTimer = stunDuration;

            if (_animator != null)
                _animator.SetTrigger("Hurt");
        }
    }
}