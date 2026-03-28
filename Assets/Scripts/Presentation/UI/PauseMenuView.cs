using UnityEngine;
using UnityEngine.InputSystem;
using App;

namespace Presentation.UI
{
    public class PauseMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private bool _isPaused;
        private PauseMenuController _controller;

        private void Update()
        {
            var keyboard = Keyboard.current;

            if (keyboard == null)
                return;

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                TogglePause();
            }
        }

        private void Awake()
        {
            _controller = new PauseMenuController();
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;

            panel.SetActive(_isPaused);

            Time.timeScale = _isPaused ? 0f : 1f;
        }

        // Кнопка Resume
        public void Resume()
        {
            _controller.Resume(() =>
            {
                _isPaused = false;
                panel.SetActive(false);
                Time.timeScale = 1f;
            });
        }

        // Кнопка Main Menu
        public void GoToMenu()
        {
            _controller.GoToMenu(() =>
            {
                Time.timeScale = 1f;
                GameEntryPoint.Instance.SceneService.LoadMenu();
            });
        }

        // Кнопка Save
        public void Save()
        {
            Debug.Log("Save clicked");
        }

        // Кнопка Load
        public void Load()
        {
            Debug.Log("Load clicked");
        }
    }
}