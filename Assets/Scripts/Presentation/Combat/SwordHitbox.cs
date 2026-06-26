using UnityEngine;
using Core.Combat;
using App;
using App.Services;

namespace Presentation.Combat
{
    // Хитбокс ближней атаки игрока
    // Наносит урон объекту при попадании во время атаки
    public class SwordHitbox : MonoBehaviour
    {
        // Звук попадания мечом
        [SerializeField] private AudioClip hitSound;

        // Урон текущей атаки
        private Damage _damage;

        // Активен ли хитбокс в данный момент
        private bool _isActive;

        // Было ли уже попадание во время текущей атаки
        private bool _hasHit;

        private IAudioService _audioService;

        // Устанавливает урон перед началом атаки
        public void Initialize(Damage damage)
        {
            _damage = damage;
        }

        // Активирует хитбокс
        public void Activate()
        {
            _isActive = true;

            // Сбрасываем флаг попадания для новой атаки
            _hasHit = false;
        }

        // Деактивирует хитбокс
        public void Deactivate()
        {
            _isActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Хитбокс работает только во время атаки
            if (!_isActive)
                return;

            // Не допускаем повторных попаданий одной атакой
            if (_hasHit)
                return;

            var damageable = other.GetComponent<IDamageable>();

            // Проверяем, может ли объект получать урон
            if (damageable == null)
                return;

            // Наносим урон цели
            damageable.ReceiveDamage(_damage);

            // Воспроизводим звук попадания
            _audioService?.PlaySFXAtPoint(hitSound, transform.position);

            // Запоминаем факт попадания
            _hasHit = true;
        }

        // Инициализирует аудиосервис
        public void InitializeAudio(IAudioService audioService)
        {
            _audioService = audioService;
        }
    }
}