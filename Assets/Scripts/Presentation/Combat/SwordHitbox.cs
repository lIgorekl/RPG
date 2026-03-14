using UnityEngine;
using Core.Combat;

namespace Presentation.Combat
{
    public class SwordHitbox : MonoBehaviour
    {
        private Damage _damage;

        private bool _isActive;

        private bool _hasHit;

        public void Initialize(Damage damage)
        {
            _damage = damage;
        }

        public void Activate()
        {
            _isActive = true;
            _hasHit = false;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Sword hit something: " + other.name);

            if (!_isActive)
                return;

            if (_hasHit)
                return;

            var damageable = other.GetComponent<IDamageable>();

            if (damageable == null)
                return;

            damageable.ReceiveDamage(_damage);

            _hasHit = true;
        }
    }
}