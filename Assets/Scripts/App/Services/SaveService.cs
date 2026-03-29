using UnityEngine;
using App.SaveLoad;
using Presentation.Scene;

namespace App.Services
{
    public class SaveService : ISaveService
    {
        private readonly SaveLoadInteractor _interactor;

        public SaveService(SaveLoadInteractor interactor)
        {
            _interactor = interactor;
        }

        public void Save(
            Transform playerTransform,
            float currentHp,
            float maxHp,
            BaseEnemyView[] enemies
        )
        {
            _interactor.Save(playerTransform, currentHp, maxHp, enemies);
        }

        public PlayerSaveData Load()
        {
            return _interactor.Load();
        }
    }
}