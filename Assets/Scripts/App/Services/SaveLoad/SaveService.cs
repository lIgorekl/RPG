using UnityEngine;
using App.SaveLoad;
using Presentation.Scene;

namespace App.Services
{
    // Реализация сервиса сохранения
    // Является оберткой над SaveLoadInteractor и делегирует ему всю работу
    public class SaveService : ISaveService
    {
        // Внутренний слой, который реально выполняет сохранение и загрузку
        private readonly SaveLoadInteractor _interactor;

        public SaveService(SaveLoadInteractor interactor)
        {
            _interactor = interactor;
        }

        // Сохраняет данные через interactor
        public void Save(PlayerSaveData data)
        {
            _interactor.Save(data);
        }

        // Загружает данные через interactor
        public PlayerSaveData Load()
        {
            return _interactor.Load();
        }
    }
}