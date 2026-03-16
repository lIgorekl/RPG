using UnityEngine;
using Core.Combat;

namespace Presentation.Combat
{
    // Представление снаряда (магическая атака).
    // Отвечает за движение снаряда и нанесение урона при столкновении.
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;

        private Damage _damage;
        private Transform _ownerTransform;

        // Инициализация снаряда после создания
        public void Initialize(Damage damage, Transform owner)
        {
            _damage = damage;
            _ownerTransform = owner;

            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            // Простое движение вперёд
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var root = other.transform.root;

            // Не наносим урон владельцу снаряда
            if (root == _ownerTransform)
                return;

            // Сначала пробуем найти любой IDamageable объект
            var damageable = root.GetComponent<IDamageable>();

            if (damageable != null)
            {
                Debug.Log("Projectile dealt damage to " + root.name);
                damageable.ReceiveDamage(_damage);
                Destroy(gameObject);
                return;
            }

            // Отдельная проверка для игрока
            var player = root.GetComponent<Presentation.Player.PlayerController>();

            if (player != null)
            {
                Debug.Log("Projectile dealt damage to Player");
                player.GetEntity().ReceiveDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}