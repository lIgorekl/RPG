using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Presentation.Player;

namespace Presentation.UI
{
    // Отображает здоровье игрока в интерфейсе
    // Обновляет полоску и текст при изменении HP
    public class HPBarView : MonoBehaviour
    {
        // Ссылка на контроллер игрока
        [SerializeField] private PlayerController playerController;

        // Заполнение полоски здоровья
        [SerializeField] private Image hpFill;

        // Текстовое отображение текущего HP
        [SerializeField] private TMP_Text hpText;

        private void Start()
        {
            // Проверяем корректность настройки компонента
            if (playerController == null || hpFill == null)
            {
                Debug.LogError("HPBarView is not configured properly");
                return;
            }

            var player = playerController.GetEntity();

            // Подписываемся на изменение здоровья игрока
            player.HealthChanged += OnHealthChanged;

            // Инициализируем интерфейс текущим состоянием здоровья
            OnHealthChanged(player.CurrentHP, player.MaxHP);
        }

        private void OnDestroy()
        {
            if (playerController == null)
                return;

            var player = playerController.GetEntity();

            // Отписываемся от события при уничтожении объекта
            if (player != null)
                player.HealthChanged -= OnHealthChanged;
        }

        // Обновляет отображение здоровья в интерфейсе
        private void OnHealthChanged(int current, int max)
        {
            // Вычисляем процент оставшегося здоровья
            float normalized = (float)current / max;

            hpFill.fillAmount = normalized;

            // Обновляем текстовое отображение HP
            if (hpText != null)
                hpText.text = $"HP: {current} / {max}";
        }
    }
}