using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Presentation.Player;

namespace Presentation.UI
{
    // UI экран окончания игры.
    // Показывается, когда игрок умирает, и позволяет перезапустить сцену.
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button restartButton;

        private void Start()
        {
            if (panel != null)
                panel.SetActive(false);

            if (playerController != null)
            {
                var player = playerController.GetEntity();
                player.Died += OnPlayerDied;
            }

            if (restartButton != null)
                restartButton.onClick.AddListener(RestartScene);
        }

        private void OnDestroy()
        {
            if (playerController == null)
                return;

            var player = playerController.GetEntity();

            if (player != null)
                player.Died -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            if (panel != null)
                panel.SetActive(true);
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}