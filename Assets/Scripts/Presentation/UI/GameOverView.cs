using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Presentation.Player;

namespace Presentation.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button restartButton;

        private void Start()
        {
            panel.SetActive(false);

            var player = playerController.GetEntity();
            player.Died += OnPlayerDied;

            restartButton.onClick.AddListener(RestartScene);
        }

        private void OnDestroy()
        {
            if (playerController != null)
            {
                var player = playerController.GetEntity();
                if (player != null)
                    player.Died -= OnPlayerDied;
            }
        }

        private void OnPlayerDied()
        {
            panel.SetActive(true);
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}