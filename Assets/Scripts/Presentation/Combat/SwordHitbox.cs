using UnityEngine;
using Core.Combat;

namespace Presentation.Combat
{
    // Хитбокс ближней атаки игрока (меч).
    // Активируется во время удара и наносит урон первому попавшему объекту.
    public class SwordHitbox : MonoBehaviour
    {
        private Damage _damage;

        // Активен ли хитбокс в данный момент
        private bool _isActive;

        // Уже был нанесён урон в этой атаке
        private bool _hasHit;

        // Инициализация урона перед атакой
        public void Initialize(Damage damage)
        {
            _damage = damage;
        }

        // Включается в момент удара
        public void Activate()
        {
            _isActive = true;
            _hasHit = false;
        }

        // Выключается после окончания атаки
        public void Deactivate()
        {
            _isActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive)
                return;

            if (_hasHit)
                return;

            var damageable = other.GetComponent<IDamageable>();

            if (damageable == null)
                return;

            damageable.ReceiveDamage(_damage);

            // Не даём мечу нанести урон несколько раз за одну атаку
            _hasHit = true;
        }
    }
}