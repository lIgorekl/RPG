using UnityEngine;
using Gameplay.Characters;
using Gameplay.Stats;
using UnityEngine.InputSystem;
using Core.Combat;

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
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        private CharacterController _characterController;

        private PlayerEntity _player;

        private void Awake()
        {
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);
            _player = new PlayerEntity(stats);
            _characterController = GetComponent<CharacterController>();
            _player.Died += OnPlayerDied;
            _player.HealthChanged += (current, max) =>
            {
                Debug.Log($"Player HP changed: {current}/{max}");
            };
        }

        private void Update()
        {
            if (_isDead) return;
            HandleMovement();
            var mouse = Mouse.current;
            if (mouse == null || playerCamera == null) return;

            if (mouse.leftButton.wasPressedThisFrame)
            {
                TryAttack(_player.GetPhysicalDamage());
            }

            if (mouse.rightButton.wasPressedThisFrame)
            {
                TryAttack(_player.GetMagicalDamage());
            }
        }

        public PlayerEntity GetEntity()
        {
            return _player;
        }

        private void TryAttack(Damage damage)
        {
            Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
            {
                var damageable = hit.collider.GetComponent<Core.Combat.IDamageable>();
                if (damageable != null)
                {
                    damageable.ReceiveDamage(damage);
                }
            }
        }

        private void HandleMovement()
        {
            if (playerCamera == null || _characterController == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            float horizontal = 0f;
            float vertical = 0f;

            if (keyboard.aKey.isPressed) horizontal = -1f;
            if (keyboard.dKey.isPressed) horizontal = 1f;
            if (keyboard.wKey.isPressed) vertical = 1f;
            if (keyboard.sKey.isPressed) vertical = -1f;

            Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
            if (inputDirection.sqrMagnitude < 0.01f)
                return;

            // Направления камеры
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Поворот персонажа
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private bool _isDead;

        private void OnPlayerDied()
        {
            _isDead = true;
            Debug.Log("Game Over: Player died");
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.Died -= OnPlayerDied;
        }
    }
}