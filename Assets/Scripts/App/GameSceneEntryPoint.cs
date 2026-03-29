using UnityEngine;
using App.Services;
using App.SaveLoad;
using App.Repositories;
using Presentation.Player;
using Presentation.Scene;

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

            IPlayerRepository repository = new JsonPlayerRepository();
            _saveLoadInteractor = new SaveLoadInteractor(repository);

            _saveService = new SaveService(_saveLoadInteractor);

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
    }
}