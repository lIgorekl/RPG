using UnityEngine;
using UnityEngine.UI;
using Presentation.Scene;
using Core.Combat;

namespace Presentation.UI
{
    public class EnemyHealthBarView : MonoBehaviour
    {
        [SerializeField] private BaseEnemyView enemyView;
        [SerializeField] private Image hpFill;

        private IDamageable _damageable;

        private void Start()
        {
            if (enemyView == null || hpFill == null)
            {
                Debug.LogError("EnemyHealthBarView not configured");
                return;
            }

            var entity = enemyView.GetEntity();
            entity.HealthChanged += OnHealthChanged;

            OnHealthChanged(entity.CurrentHP, entity.MaxHP);
        }

        private void OnDestroy()
        {
            if (enemyView != null)
            {
                var entity = enemyView.GetEntity();
                if (entity != null)
                    entity.HealthChanged -= OnHealthChanged;
            }
        }

        private void OnHealthChanged(int current, int max)
        {
            hpFill.fillAmount = (float)current / max;
        }
    }
}