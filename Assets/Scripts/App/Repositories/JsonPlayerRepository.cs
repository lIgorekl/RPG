using System.IO;
using UnityEngine;
using App.SaveLoad;
using App.Repositories;

namespace App.Repositories
{
    // Реализация репозитория сохранений через JSON файл
    // Отвечает за запись и чтение данных с диска
    public class JsonPlayerRepository : IPlayerRepository
    {
        // Путь к файлу сохранения
        private readonly string _filePath;

        public JsonPlayerRepository()
        {
            // Папка persistentDataPath гарантирует сохранение между сессиями игры
            _filePath = Application.persistentDataPath + "/save.json";
        }

        // Сохраняет данные игрока в JSON файл
        public void Save(PlayerSaveData data)
        {
            Debug.Log("Repository.Save CALLED");

            // Сериализация объекта в JSON строку
            string json = JsonUtility.ToJson(data);

            // Запись строки в файл
            File.WriteAllText(_filePath, json);

            Debug.Log("Saved to: " + _filePath);
        }

        // Загружает данные игрока из JSON файла
        public PlayerSaveData Load()
        {
            // Если файл не существует — сохранения нет
            if (!File.Exists(_filePath))
                return null;

            // Чтение JSON строки из файла
            string json = File.ReadAllText(_filePath);

            // Десериализация JSON обратно в объект
            return JsonUtility.FromJson<PlayerSaveData>(json);
        }
    }
}