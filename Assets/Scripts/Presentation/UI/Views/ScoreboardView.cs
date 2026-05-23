using App.Events;
using TMPro;
using UnityEngine;

namespace Presentation.UI
{
    public class ScoreboardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private IGameEventBus _eventBus;

        public void Initialize(IGameEventBus eventBus)
        {
            _eventBus = eventBus;

            if (_eventBus != null)
                _eventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);

            UpdateDisplay(0);
        }

        private void OnDestroy()
        {
            if (_eventBus != null)
                _eventBus.Unsubscribe<ScoreChangedEvent>(OnScoreChanged);
        }

        private void OnScoreChanged(ScoreChangedEvent scoreEvent)
        {
            UpdateDisplay(scoreEvent.TotalScore);
        }

        private void UpdateDisplay(int totalScore)
        {
            if (scoreText == null)
                return;

            scoreText.text = $"Score: {totalScore}";
        }
    }
}
