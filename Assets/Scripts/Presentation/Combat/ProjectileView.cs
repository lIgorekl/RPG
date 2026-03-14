using UnityEngine;
using Core.Combat;

namespace Presentation.Combat
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;

        private Damage _damage;
        private ProjectileOwner _owner;
        private Transform _ownerTransform;

        public void Initialize(Damage damage, Transform owner)
        {
            _damage = damage;
            _ownerTransform = owner;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var root = other.transform.root;

            // не бьём того, кто выстрелил
            if (root == _ownerTransform)
                return;

            // сначала проверяем обычный IDamageable
            var damageable = root.GetComponent<IDamageable>();

            if (damageable != null)
            {
                Debug.Log("Projectile dealt damage to " + root.name);
                damageable.ReceiveDamage(_damage);
            }
            else
            {
                // если это игрок
                var player = root.GetComponent<Presentation.Player.PlayerController>();

                if (player != null)
                {
                    Debug.Log("Projectile dealt damage to Player");
                    player.GetEntity().ReceiveDamage(_damage);
                }
            }

            Destroy(gameObject);
        }
    }
}