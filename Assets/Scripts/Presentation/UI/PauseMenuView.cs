using UnityEngine;
using UnityEngine.InputSystem;
using App;
using App.Services;
using Gameplay.Stats;        // для Health
using Presentation.Player;

namespace Presentation.UI
{
    public class PauseMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private bool _isPaused;
        private PauseMenuController _controller;
        private ISaveService _saveService;
        private PlayerController _player;

        private void Update()
        {
            var keyboard = Keyboard.current;

            if (keyboard == null)
                return;

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                TogglePause();
            }

            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                Save();
            }
        }

        private void Awake()
        {
            _controller = new PauseMenuController();

            var entryPoint = FindObjectOfType<GameSceneEntryPoint>();

            if (entryPoint == null)
            {
                Debug.LogError("GameSceneEntryPoint NOT FOUND");
                return;
            }

            _saveService = entryPoint.GetSaveService();
            _player = entryPoint.GetPlayer();

            if (_saveService == null)
                Debug.LogError("SaveService is NULL");

            if (_player == null)
                Debug.LogError("Player is NULL");
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
            var player = _player;
            var entity = player.GetEntity();

            var entryPoint = FindObjectOfType<GameSceneEntryPoint>();

            _saveService.Save(
                player.transform,
                entity.CurrentHP,
                entity.MaxHP,
                entryPoint.GetEnemies()
            );

            Debug.Log("SAVE BUTTON CLICKED");
        }

        // Кнопка Load
        public void Load()
        {
            var data = _saveService.Load();

            if (data == null)
            {
                Debug.Log("No save found");
                return;
            }

            var player = FindObjectOfType<PlayerController>();
            player.ApplySaveData(data);

            Debug.Log("Game Loaded");
        }
    }
}