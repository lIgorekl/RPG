using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Presentation.Player;

namespace Presentation.UI
{
    // UI полоска здоровья игрока.
    // Подписывается на событие изменения HP PlayerEntity.
    public class HPBarView : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Image hpFill;
        [SerializeField] private TMP_Text hpText;

        private void Start()
        {
            if (playerController == null || hpFill == null)
            {
                Debug.LogError("HPBarView is not configured properly");
                return;
            }

            var player = playerController.GetEntity();

            player.HealthChanged += OnHealthChanged;

            // Инициализация UI текущим состоянием HP
            OnHealthChanged(player.CurrentHP, player.MaxHP);
        }

        private void OnDestroy()
        {
            if (playerController == null)
                return;

            var player = playerController.GetEntity();

            if (player != null)
                player.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int current, int max)
        {
            float normalized = (float)current / max;

            hpFill.fillAmount = normalized;

            if (hpText != null)
                hpText.text = $"HP: {current} / {max}";
        }
    }
}