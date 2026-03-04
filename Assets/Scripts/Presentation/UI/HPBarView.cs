using UnityEngine;
using UnityEngine.UI;
using Presentation.Player;

namespace Presentation.UI
{
    public class HPBarView : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Image hpFill;
        [SerializeField] private TMPro.TMP_Text hpText;

        private void Start()
        {
            if (playerController == null || hpFill == null)
            {
                Debug.LogError("HPBarView is not configured properly");
                return;
            }

            var player = playerController.GetEntity();
            player.HealthChanged += OnHealthChanged;

            // Инициализация текущего состояния
            OnHealthChanged(player.CurrentHP, player.MaxHP);
        }

        private void OnDestroy()
        {
            if (playerController != null)
            {
                var player = playerController.GetEntity();
                if (player != null)
                    player.HealthChanged -= OnHealthChanged;
            }
        }

        private void OnHealthChanged(int current, int max)
        {
            float normalized = (float)current / max;
            hpFill.fillAmount = normalized;

            if (hpText != null)
            {
                hpText.text = $"HP: {current} / {max}";
            }
        }
    }
}