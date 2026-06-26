using UnityEngine;
using Core.Combat;

namespace Presentation.Combat
{
    // Представление магического снаряда
    // Отвечает за движение и нанесение урона при столкновении
    public class ProjectileView : MonoBehaviour
    {
        // Скорость полета снаряда
        [SerializeField] private float speed = 10f;

        // Время жизни снаряда
        [SerializeField] private float lifeTime = 5f;

        private Damage _damage;
        private Transform _ownerTransform;

        // Инициализирует снаряд после создания
        public void Initialize(Damage damage, Transform owner)
        {
            _damage = damage;
            _ownerTransform = owner;

            // Автоматически уничтожаем снаряд через заданное время
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            // Перемещаем снаряд вперед
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var root = other.transform.root;

            // Игнорируем столкновение с владельцем снаряда
            if (root == _ownerTransform)
                return;

            // Пытаемся найти объект, способный получать урон
            var damageable = root.GetComponent<IDamageable>();

            if (damageable != null)
            {
                Debug.Log("Projectile dealt damage to " + root.name);

                // Наносим урон цели
                damageable.ReceiveDamage(_damage);

                // Уничтожаем снаряд после попадания
                Destroy(gameObject);

                return;
            }

            // Дополнительная проверка для игрока
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