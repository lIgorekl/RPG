using UnityEngine;
using UnityEngine.UI;
using Presentation.Scene;

namespace Presentation.UI
{
    // UI элемент полоски HP врага.
    // Подписывается на событие изменения здоровья EnemyEntity.
    public class EnemyHealthBarView : MonoBehaviour
    {
        [SerializeField] private BaseEnemyView enemyView;
        [SerializeField] private Image hpFill;

        private void Start()
        {
            if (enemyView == null)
                enemyView = GetComponentInParent<BaseEnemyView>();

            if (enemyView == null || hpFill == null)
            {
                Debug.LogError(
                    $"{name}: EnemyHealthBarView not configured " +
                    $"(enemyView={enemyView != null}, hpFill={hpFill != null})");
                return;
            }

            var entity = enemyView.GetEntity();

            entity.HealthChanged += OnHealthChanged;

            OnHealthChanged(entity.CurrentHP, entity.MaxHP);
        }

        private void OnDestroy()
        {
            if (enemyView == null)
                return;

            var entity = enemyView.GetEntity();

            if (entity != null)
                entity.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int current, int max)
        {
            hpFill.fillAmount = (float)current / max;
        }
    }
}