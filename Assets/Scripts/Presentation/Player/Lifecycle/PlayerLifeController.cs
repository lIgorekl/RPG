using UnityEngine;
using Core.Combat;
using Gameplay.Characters;

namespace Presentation.Player
{
    // Управляет состоянием жизни игрока
    // Обрабатывает получение урона, оглушение и смерть
    public class PlayerLifeController
    {
        private readonly PlayerEntity _player;
        private readonly Animator _animator;

        // Длительность оглушения после получения урона
        private readonly float _stunDuration;

        // Таймер текущего оглушения
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

            // Подписываемся на события игрока
            _player.Died += OnPlayerDied;
            _player.DamageReceived += OnDamageReceived;
        }

        // Обновляет состояние оглушения
        public void Update()
        {
            if (!_isStunned)
                return;

            // Уменьшаем оставшееся время оглушения
            _stunTimer -= Time.deltaTime;

            // Снимаем оглушение после окончания таймера
            if (_stunTimer <= 0f)
            {
                _isStunned = false;
            }
        }

        // Обрабатывает смерть игрока
        private void OnPlayerDied()
        {
            _isDead = true;

            // Запускаем анимацию смерти
            if (_animator != null)
            {
                _animator.SetTrigger("Death");
            }

            Debug.Log("Game Over: Player died");
        }

        // Обрабатывает получение урона
        private void OnDamageReceived(Damage damage)
        {
            // Активируем оглушение на заданное время
            _isStunned = true;
            _stunTimer = _stunDuration;

            // Запускаем анимацию получения урона
            if (_animator != null)
            {
                _animator.SetTrigger("Hurt");
            }
        }

        // Отписываемся от событий игрока
        public void Dispose()
        {
            if (_player == null)
                return;

            _player.Died -= OnPlayerDied;
            _player.DamageReceived -= OnDamageReceived;
        }
    }
}