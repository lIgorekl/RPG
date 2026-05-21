using UnityEngine;
using UnityEngine.InputSystem;
using Core.Gameplay;
using Gameplay.Characters;

namespace Presentation.Player
{
    public class PlayerCombatController
    {
        private readonly PlayerEntity _player;
        private readonly PlayerCombat _combat;

        private readonly Animator _animator;
        private readonly Camera _camera;
        private readonly Transform _owner;

        private readonly Cooldown _magicCooldown;
        private readonly Cooldown _meleeCooldown;

        private bool _isAttacking;

        public bool IsMagicOnCooldown =>
            _magicCooldown.IsActive;

        public float MagicCooldownProgress =>
            _magicCooldown.Progress;

        public bool IsAttacking => _isAttacking;

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

            _magicCooldown =
                new Cooldown(magicCooldown);

            _meleeCooldown =
                new Cooldown(meleeCooldown);
        }

        public void Update(PlayerInputData input)
        {
            UpdateCooldowns();

            HandleMelee(input);

            HandleMagic(input);
        }

        private void UpdateCooldowns()
        {
            bool wasActive =
                _magicCooldown.IsActive;

            _magicCooldown.Update(Time.deltaTime);

            if (wasActive &&
                !_magicCooldown.IsActive)
            {
                MagicCooldownFinished?.Invoke();
            }

            _meleeCooldown.Update(Time.deltaTime);

            if (!_meleeCooldown.IsActive &&
                _isAttacking)
            {
                _isAttacking = false;
                _combat.StopMelee();
            }
        }

        private void HandleMelee(PlayerInputData input)
        {
            if (!input.MeleeAttackPressed)
                return;

            if (_meleeCooldown.IsActive)
                return;

            RotateTowardsCamera();

            if (_animator != null)
                _animator.SetTrigger("Attack");

            _isAttacking = true;

            _combat.MeleeAttack(
                _player.GetPhysicalDamage());

            _meleeCooldown.Start();
        }

        private void HandleMagic(PlayerInputData input)
        {
            if (!input.MagicAttackPressed)
                return;

            if (_magicCooldown.IsActive)
                return;

            RotateTowardsCamera();

            _combat.CastMagic(
                _player.GetMagicalDamage(),
                _owner);

            _magicCooldown.Start();

            MagicCooldownStarted?.Invoke();
        }

        private void RotateTowardsCamera()
        {
            if (_camera == null)
                return;

            Vector3 forward =
                _camera.transform.forward;

            forward.y = 0f;

            if (forward.sqrMagnitude < 0.01f)
                return;

            Quaternion rotation =
                Quaternion.LookRotation(forward);

            _owner.rotation = rotation;
        }
    }
}