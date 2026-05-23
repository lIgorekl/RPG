using UnityEngine;
using UnityEngine.InputSystem;
using App;
using App.Services;
using Gameplay.Stats;        // для Health
using Presentation.Player;
using App.SaveLoad;
using System.Collections.Generic;
using Presentation.Scene;

namespace Presentation.UI
{
    public class PauseMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private bool _isPaused;
        private PauseMenuController _controller;
        private ISaveService _saveService;
        private PlayerController _player;
        private GameSceneEntryPoint _entryPoint;

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

            _entryPoint = FindFirstObjectByType<GameSceneEntryPoint>();

            if (_entryPoint == null)
            {
                Debug.LogError("GameSceneEntryPoint NOT FOUND");
                return;
            }

            _saveService = _entryPoint.GetSaveService();
            _player = _entryPoint.GetPlayer();

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
            var data = _player.CreateSaveData();

            data.Enemies = new List<EnemySaveData>();

            foreach (var enemy in _entryPoint.GetEnemies())
            {
                if (enemy == null)
                    continue;

                var e = enemy.GetEntity();

                data.Enemies.Add(new EnemySaveData
                {
                    PositionX = enemy.transform.position.x,
                    PositionY = enemy.transform.position.y,
                    PositionZ = enemy.transform.position.z,
                    CurrentHp = e.CurrentHP,
                    IsDead = e.IsDead,
                    Id = enemy.GetId()
                });
            }

            _saveService.Save(data);

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

            _player.ApplySaveData(data);
            _entryPoint.ApplyEnemiesSaveData(data.Enemies);

            Debug.Log("Game Loaded");
        }
    }
}