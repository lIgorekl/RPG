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

        public void Save(PlayerSaveData data)
        {
            _interactor.Save(data);
        }

        public PlayerSaveData Load()
        {
            return _interactor.Load();
        }
    }
}