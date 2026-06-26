using App.Events;
using TMPro;
using UnityEngine;

namespace Presentation.UI
{
    // Представление таблицы очков.
    // Отображает текущее количество очков
    // и обновляет текст при изменении счета.
    public class ScoreboardView : MonoBehaviour
    {
        // UI-текст,
        // в котором отображается счет
        [SerializeField] private TMP_Text scoreText;

        // Шина игровых событий
        private IGameEventBus _eventBus;

        // Инициализирует представление
        // и подписывается на изменение счета
        public void Initialize(
            IGameEventBus eventBus)
        {
            _eventBus = eventBus;

            // Подписываемся
            // на изменение количества очков
            if (_eventBus != null)
            {
                _eventBus.Subscribe<ScoreChangedEvent>(
                    OnScoreChanged);
            }

            // Отображаем начальное значение
            UpdateDisplay(0);
        }

        // Освобождаем подписку
        // при уничтожении объекта
        private void OnDestroy()
        {
            if (_eventBus != null)
            {
                _eventBus.Unsubscribe<ScoreChangedEvent>(
                    OnScoreChanged);
            }
        }

        // Вызывается,
        // когда количество очков изменилось
        private void OnScoreChanged(
            ScoreChangedEvent scoreEvent)
        {
            UpdateDisplay(scoreEvent.TotalScore);
        }

        // Обновляет текст на экране
        private void UpdateDisplay(int totalScore)
        {
            // Если текстовое поле
            // не назначено,
            // ничего не делаем
            if (scoreText == null)
                return;

            scoreText.text =
                $"Score: {totalScore}";
        }
    }
}