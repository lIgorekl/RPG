using UnityEngine;
using App.SaveLoad;
using Presentation.Scene;

namespace App.Services
{
    // Интерфейс сервиса сохранения
    // Определяет методы сохранения и загрузки игрового прогресса
    public interface ISaveService
    {
        // Сохраняет данные игрока и игрового мира
        void Save(PlayerSaveData data);

        // Загружает сохраненные данные
        PlayerSaveData Load();
    }
}