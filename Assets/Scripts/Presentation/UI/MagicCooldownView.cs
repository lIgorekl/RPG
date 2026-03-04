using UnityEngine;
using UnityEngine.UI;
using Presentation.Player;

namespace Presentation.UI
{
    public class MagicCooldownView : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Image cooldownImage;

        private bool _isTracking;

        private void Start()
        {
            if (playerController == null || cooldownImage == null)
            {
                Debug.LogError("MagicCooldownView not configured");
                return;
            }

            playerController.MagicCooldownStarted += OnCooldownStarted;
            playerController.MagicCooldownFinished += OnCooldownFinished;

            cooldownImage.fillAmount = 0f;
        }

        private void OnDestroy()
        {
            if (playerController != null)
            {
                playerController.MagicCooldownStarted -= OnCooldownStarted;
                playerController.MagicCooldownFinished -= OnCooldownFinished;
            }
        }

        private void Update()
        {
            if (_isTracking)
            {
                cooldownImage.fillAmount =
                    playerController.MagicCooldownProgress;
            }
        }

        private void OnCooldownStarted()
        {
            _isTracking = true;
            cooldownImage.fillAmount = 1f;
        }

        private void OnCooldownFinished()
        {
            _isTracking = false;
            cooldownImage.fillAmount = 0f;
        }
    }
}