using System.IO;
using UnityEngine;
using App.SaveLoad;
using App.Repositories;

namespace App.Repositories
{
    public class JsonPlayerRepository : IPlayerRepository
    {
        private readonly string _filePath;

        public JsonPlayerRepository()
        {
            _filePath = Application.persistentDataPath + "/save.json";
        }

        public void Save(PlayerSaveData data)
        {
            Debug.Log("Repository.Save CALLED");

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_filePath, json);

            Debug.Log("Saved to: " + _filePath);
        }

        public PlayerSaveData Load()
        {
            if (!File.Exists(_filePath))
                return null;

            string json = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<PlayerSaveData>(json);
        }
    }
}