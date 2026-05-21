using UnityEngine;
using Core.Combat;
using Gameplay.Characters;

namespace Presentation.Player
{
    // Управляет состоянием жизни игрока:
    // смерть, стан, получение урона.
    public class PlayerLifeController
    {
        private readonly PlayerEntity _player;
        private readonly Animator _animator;

        private readonly float _stunDuration;

        private float _stunTimer;

        private bool _isStunned;
        private bool _isDead;

        public bool IsDead => _isDead;
        public bool IsStunned => _isStunned;

        public PlayerLifeController(
            PlayerEntity player,
            Animator animator,
            float stunDuration)
        {
            _player = player;
            _animator = animator;
            _stunDuration = stunDuration;

            _player.Died += OnPlayerDied;
            _player.DamageReceived += OnDamageReceived;
        }

        public void Update()
        {
            if (!_isStunned)
                return;

            _stunTimer -= Time.deltaTime;

            if (_stunTimer <= 0f)
            {
                _isStunned = false;
            }
        }

        private void OnPlayerDied()
        {
            _isDead = true;

            if (_animator != null)
            {
                _animator.SetTrigger("Death");
            }

            Debug.Log("Game Over: Player died");
        }

        private void OnDamageReceived(Damage damage)
        {
            _isStunned = true;
            _stunTimer = _stunDuration;

            if (_animator != null)
            {
                _animator.SetTrigger("Hurt");
            }
        }

        public void Dispose()
        {
            if (_player == null)
                return;

            _player.Died -= OnPlayerDied;
            _player.DamageReceived -= OnDamageReceived;
        }
    }
}