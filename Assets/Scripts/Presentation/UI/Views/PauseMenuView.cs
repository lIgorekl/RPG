using UnityEngine;
using UnityEngine.InputSystem;
using App;
using App.Services;
using Gameplay.Stats;
using Presentation.Player;
using App.SaveLoad;
using System.Collections.Generic;
using Presentation.Scene;

namespace Presentation.UI
{
    // Окно паузы в игре
    // Позволяет поставить игру на паузу, сохранить прогресс и загрузить сохранение
    public class PauseMenuView : MonoBehaviour
    {
        // Панель меню паузы
        [SerializeField] private GameObject panel;

        // Текущее состояние паузы
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

            // Открытие или закрытие меню паузы по Escape
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                TogglePause();
            }

            // Быстрое сохранение по F5
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                Save();
            }
        }

        private void Awake()
        {
            _controller = new PauseMenuController();

            // Получаем точку входа игровой сцены
            _entryPoint = FindFirstObjectByType<GameSceneEntryPoint>();

            if (_entryPoint == null)
            {
                Debug.LogError("GameSceneEntryPoint NOT FOUND");
                return;
            }

            // Получаем необходимые сервисы и ссылки
            _saveService = _entryPoint.GetSaveService();
            _player = _entryPoint.GetPlayer();

            if (_saveService == null)
                Debug.LogError("SaveService is NULL");

            if (_player == null)
                Debug.LogError("Player is NULL");
        }

        // Переключает состояние паузы
        private void TogglePause()
        {
            _isPaused = !_isPaused;

            panel.SetActive(_isPaused);

            // При паузе время в игре останавливается
            Time.timeScale = _isPaused ? 0f : 1f;
        }

        // Кнопка Resume
        // Продолжает игру
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
        // Возвращает игрока в главное меню
        public void GoToMenu()
        {
            _controller.GoToMenu(() =>
            {
                // Перед переходом обязательно снимаем паузу
                Time.timeScale = 1f;

                GameEntryPoint.Instance.SceneService.LoadMenu();
            });
        }

        // Кнопка Save
        // Сохраняет игрока и всех врагов на сцене
        public void Save()
        {
            // Получаем данные игрока
            var data = _player.CreateSaveData();

            data.Enemies = new List<EnemySaveData>();

            // Сохраняем состояние каждого врага
            foreach (var enemy in _entryPoint.GetEnemies())
            {
                if (enemy == null)
                    continue;

                var e = enemy.GetEntity();

                data.Enemies.Add(new EnemySaveData
                {
                    // Позиция врага
                    PositionX = enemy.transform.position.x,
                    PositionY = enemy.transform.position.y,
                    PositionZ = enemy.transform.position.z,

                    // Состояние здоровья
                    CurrentHp = e.CurrentHP,
                    IsDead = e.IsDead,

                    // Уникальный идентификатор врага
                    Id = enemy.GetId()
                });
            }

            _saveService.Save(data);

            Debug.Log("SAVE BUTTON CLICKED");
        }

        // Кнопка Load
        // Загружает последнее сохранение
        public void Load()
        {
            var data = _saveService.Load();

            // Если сохранение отсутствует
            if (data == null)
            {
                Debug.Log("No save found");
                return;
            }

            // Восстанавливаем игрока
            _player.ApplySaveData(data);

            // Восстанавливаем всех врагов
            _entryPoint.ApplyEnemiesSaveData(data.Enemies);

            Debug.Log("Game Loaded");
        }
    }
}