using UnityEngine;
using Core.Combat;

namespace Presentation.Combat
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;

        private Damage _damage;

        public void Initialize(Damage damage)
        {
            _damage = damage;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.ReceiveDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}