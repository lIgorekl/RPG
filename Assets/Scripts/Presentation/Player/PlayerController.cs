using UnityEngine;
using Gameplay.Characters;
using Gameplay.Stats;
using UnityEngine.InputSystem;
using Core.Combat;
using Presentation.Combat;

namespace Presentation.Player
{
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

        private MovementState _currentMovementState = MovementState.Walk;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float stunDuration = 0.2f;
        [SerializeField] private ProjectileView magicProjectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private SwordHitbox swordHitbox;
        [SerializeField] private float meleeAttackDistance = 2f;
        [SerializeField] private float magicCooldown = 2f;
        [SerializeField] private float meleeCooldown = 0.6f;

        private float _meleeCooldownTimer;
        private bool _isMeleeOnCooldown;
        public bool IsMagicOnCooldown => _isMagicOnCooldown;

        public float MagicCooldownProgress =>
            _isMagicOnCooldown ? _magicCooldownTimer / magicCooldown : 0f;

        public event System.Action MagicCooldownStarted;
        public event System.Action MagicCooldownFinished;

        private float _magicCooldownTimer;
        private bool _isMagicOnCooldown;

        private float _stunTimer;
        private bool _isStunned;
        private bool _isAttacking;

        private CharacterController _characterController;

        private PlayerEntity _player;
        private Animator _animator;

        private void Awake()
        {
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);
            _player = new PlayerEntity(stats);
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
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
            if (_isStunned)
            {
                _stunTimer -= Time.deltaTime;
                if (_stunTimer <= 0f)
                {
                    _isStunned = false;
                }
                return;
            }
            if (_isMagicOnCooldown)
            {
                _magicCooldownTimer -= Time.deltaTime;

                if (_magicCooldownTimer <= 0f)
                {
                    _isMagicOnCooldown = false;
                    MagicCooldownFinished?.Invoke();
                }
            }

            if (_isMeleeOnCooldown)
            {
                _meleeCooldownTimer -= Time.deltaTime;

                if (_meleeCooldownTimer <= 0f)
                {
                    _isMeleeOnCooldown = false;
                    _isAttacking = false;

                    swordHitbox.Deactivate();
                }
            }
            HandleMovement();
            var mouse = Mouse.current;
            if (mouse == null || playerCamera == null) return;

            if (mouse.leftButton.wasPressedThisFrame && !_isMeleeOnCooldown)
            {
                RotateTowardsCamera();

                if (_animator != null)
                    _animator.SetTrigger("Attack");

                _isAttacking = true;

                swordHitbox.Initialize(_player.GetPhysicalDamage());
                swordHitbox.Activate();

                _isMeleeOnCooldown = true;
                _meleeCooldownTimer = meleeCooldown;
            }

            if (mouse.rightButton.wasPressedThisFrame && !_isMagicOnCooldown)
            {
                RotateTowardsCamera();
                SpawnMagicProjectile(_player.GetMagicalDamage());

                _isMagicOnCooldown = true;
                _magicCooldownTimer = magicCooldown;

                MagicCooldownStarted?.Invoke();
            }
        }

        public PlayerEntity GetEntity()
        {
            return _player;
        }

        

        private void TryMeleeAttack(Damage damage)
        {
            Vector3 origin = transform.position + Vector3.up * 1f; // чуть выше центра
            Vector3 direction = transform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, meleeAttackDistance))
            {
                var damageable = hit.collider.GetComponent<Core.Combat.IDamageable>();
                if (damageable != null)
                {
                    damageable.ReceiveDamage(damage);
                }
            }
        }

        public void PerformMeleeAttack()
        {
            TryMeleeAttack(_player.GetPhysicalDamage());
        }

        private void SpawnMagicProjectile(Damage damage)
        {
            if (magicProjectilePrefab == null || projectileSpawnPoint == null || playerCamera == null)
                return;

            // Создаём снаряд в точке спавна
            Vector3 direction = playerCamera.transform.forward;
            direction.y = 0f;
            direction.Normalize();

            Quaternion rotation = Quaternion.LookRotation(direction);

            var projectile = Instantiate(
                magicProjectilePrefab,
                projectileSpawnPoint.position,
                rotation);

            // Передаём урон внутрь снаряда
            projectile.Initialize(damage, transform);
        }

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

        private void HandleMovement()
        {
            if (_isAttacking)
                return;

            if (playerCamera == null || _characterController == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            // Определяем состояние движения
            _currentMovementState = keyboard.leftShiftKey.isPressed
                ? MovementState.Run
                : MovementState.Walk;

            float horizontal = 0f;
            float vertical = 0f;

            if (keyboard.aKey.isPressed) horizontal = -1f;
            if (keyboard.dKey.isPressed) horizontal = 1f;
            if (keyboard.wKey.isPressed) vertical = 1f;
            if (keyboard.sKey.isPressed) vertical = -1f;

            Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
            if (inputDirection.sqrMagnitude < 0.01f)
            {
                if (_animator != null)
                    _animator.SetFloat("Speed", 0f);

                return;
            }

            // Направления камеры
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            float speed = _currentMovementState == MovementState.Run ? runSpeed : walkSpeed;
            _characterController.Move(moveDirection * speed * Time.deltaTime);

            // Поворот персонажа
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (_animator != null)
            {
                float speedPercent = _currentMovementState == MovementState.Run ? 1f : 0.5f;
                _animator.SetFloat("Speed", speedPercent);
            }
        }

        private bool _isDead;

        private void OnPlayerDied()
        {
            _isDead = true;

            if (_animator != null)
            {
                _animator.SetTrigger("Death");
            }

            Debug.Log("Game Over: Player died");
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.Died -= OnPlayerDied;

            _player.DamageReceived -= OnDamageReceived;
        }

        private void OnDamageReceived(Damage damage)
        {
            _isStunned = true;
            _stunTimer = stunDuration;

            if (_animator != null)
            {
                _animator.SetTrigger("Hurt");
            }
        }

        private enum MovementState
        {
            Walk,
            Run
        }
    }
}