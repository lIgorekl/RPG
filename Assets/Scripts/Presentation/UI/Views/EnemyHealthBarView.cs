using UnityEngine;
using UnityEngine.UI;
using Presentation.Scene;

namespace Presentation.UI
{
    // Отображает полоску здоровья врага
    // Обновляет UI при изменении здоровья EnemyEntity
    public class EnemyHealthBarView : MonoBehaviour
    {
        // Представление врага на сцене
        [SerializeField] private BaseEnemyView enemyView;

        // Заполнение полоски здоровья
        [SerializeField] private Image hpFill;

        private void Start()
        {
            // Если ссылка не назначена вручную, ищем врага среди родительских объектов
            if (enemyView == null)
                enemyView = GetComponentInParent<BaseEnemyView>();

            // Проверяем корректность настройки компонента
            if (enemyView == null || hpFill == null)
            {
                Debug.LogError(
                    $"{name}: EnemyHealthBarView not configured " +
                    $"(enemyView={enemyView != null}, hpFill={hpFill != null})");
                return;
            }

            var entity = enemyView.GetEntity();

            // Подписываемся на изменение здоровья врага
            entity.HealthChanged += OnHealthChanged;

            // Сразу отображаем текущее значение здоровья
            OnHealthChanged(entity.CurrentHP, entity.MaxHP);
        }

        private void OnDestroy()
        {
            if (enemyView == null)
                return;

            var entity = enemyView.GetEntity();

            // Отписываемся от события при уничтожении объекта
            if (entity != null)
                entity.HealthChanged -= OnHealthChanged;
        }

        // Обновляет заполнение полоски здоровья
        private void OnHealthChanged(int current, int max)
        {
            hpFill.fillAmount = (float)current / max;
        }
    }
}