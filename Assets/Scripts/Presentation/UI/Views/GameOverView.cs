using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Presentation.Player;

namespace Presentation.UI
{
    // Отображает экран окончания игры
    // Показывается после смерти игрока и позволяет перезапустить сцену
    public class GameOverView : MonoBehaviour
    {
        // Контроллер игрока
        [SerializeField] private PlayerController playerController;

        // Панель экрана Game Over
        [SerializeField] private GameObject panel;

        // Кнопка перезапуска уровня
        [SerializeField] private Button restartButton;

        private void Start()
        {
            // Скрываем экран Game Over при запуске сцены
            if (panel != null)
                panel.SetActive(false);

            if (playerController != null)
            {
                var player = playerController.GetEntity();

                // Подписываемся на событие смерти игрока
                player.Died += OnPlayerDied;
            }

            // Назначаем обработчик нажатия кнопки
            if (restartButton != null)
                restartButton.onClick.AddListener(RestartScene);
        }

        private void OnDestroy()
        {
            if (playerController == null)
                return;

            var player = playerController.GetEntity();

            // Отписываемся от события при уничтожении объекта
            if (player != null)
                player.Died -= OnPlayerDied;
        }

        // Вызывается после смерти игрока
        private void OnPlayerDied()
        {
            // Показываем экран окончания игры
            if (panel != null)
                panel.SetActive(true);
        }

        // Перезагружает текущую сцену
        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}