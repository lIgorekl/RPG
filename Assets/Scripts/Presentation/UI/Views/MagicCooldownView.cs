using UnityEngine;
using UnityEngine.UI;
using Presentation.Player;

namespace Presentation.UI
{
    // UI индикатор перезарядки магической атаки
    // Отображает оставшееся время кулдауна способности
    public class MagicCooldownView : MonoBehaviour
    {
        // Ссылка на игрока
        [SerializeField] private PlayerController playerController;

        // Изображение, отображающее прогресс кулдауна
        [SerializeField] private Image cooldownImage;

        // Выполняется ли сейчас отслеживание кулдауна
        private bool _isTracking;

        private void Start()
        {
            // Проверяем что все ссылки назначены
            if (playerController == null || cooldownImage == null)
            {
                Debug.LogError("MagicCooldownView not configured");
                return;
            }

            // Подписываемся на события начала и окончания кулдауна
            playerController.MagicCooldownStarted += OnCooldownStarted;
            playerController.MagicCooldownFinished += OnCooldownFinished;

            // Изначально кулдаун отсутствует
            cooldownImage.fillAmount = 0f;
        }

        private void OnDestroy()
        {
            if (playerController == null)
                return;

            // Отписываемся от событий при уничтожении объекта
            playerController.MagicCooldownStarted -= OnCooldownStarted;
            playerController.MagicCooldownFinished -= OnCooldownFinished;
        }

        private void Update()
        {
            // Если кулдаун не активен, обновление не требуется
            if (!_isTracking)
                return;

            // Обновляем отображение прогресса кулдауна
            cooldownImage.fillAmount =
                playerController.MagicCooldownProgress;
        }

        // Вызывается при начале кулдауна
        private void OnCooldownStarted()
        {
            _isTracking = true;

            // Индикатор полностью заполнен в начале перезарядки
            cooldownImage.fillAmount = 1f;
        }

        // Вызывается после окончания кулдауна
        private void OnCooldownFinished()
        {
            _isTracking = false;

            // Очищаем индикатор
            cooldownImage.fillAmount = 0f;
        }
    }
}