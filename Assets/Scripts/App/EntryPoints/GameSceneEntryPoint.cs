using UnityEngine;
using App.Services;
using App.SaveLoad;
using App.Repositories;
using Presentation.Player;
using Presentation.Scene;
using System.Collections.Generic;
using Presentation.AI;

namespace App
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private BaseEnemyView[] enemies;

        private SaveLoadInteractor _saveLoadInteractor;
        private ISaveService _saveService;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("GameSceneEntryPoint Initialize CALLED");

            var audioService = 
                GameEntryPoint.Instance.GetAudioService();

            var gameModeService =
                GameEntryPoint.Instance.GetGameModeService();

            // SAVE
            IPlayerRepository repository = new JsonPlayerRepository();
            _saveLoadInteractor = new SaveLoadInteractor(repository);
            _saveService = new SaveService(_saveLoadInteractor);

            if (player != null)
            {
                player.InitializeAudio(audioService);
            }

            foreach (var enemy in enemies)
            {
                if (enemy == null)
                    continue;

                enemy.InitializeAudio(audioService);

                var behaviour =
                    enemy.GetComponent<BaseEnemyBehaviour>();

                if (behaviour != null)
                {
                    behaviour.Initialize(
                        gameModeService);
                }
            }

            Debug.Log("Enemies count: " + enemies.Length);
            Debug.Log("SaveService CREATED");
        }

        public ISaveService GetSaveService()
        {
            return _saveService;
        }

        public PlayerController GetPlayer()
        {
            return player;
        }

        public BaseEnemyView[] GetEnemies()
        {
            return enemies;
        }

        public void ApplyEnemiesSaveData(List<EnemySaveData> enemiesData)
        {
            // 1. выключаем всех
            foreach (var enemy in enemies)
            {
                if (enemy == null)
                    continue;

                enemy.gameObject.SetActive(false);
            }

            // 2. восстанавливаем
            foreach (var enemyData in enemiesData)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy == null)
                        continue;

                    if (enemy.GetId() != enemyData.Id)
                        continue;

                    enemy.ApplySaveData(enemyData);
                    break;
                }
            }
        }
    }
}