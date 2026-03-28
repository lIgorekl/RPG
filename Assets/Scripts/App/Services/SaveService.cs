using UnityEngine;

namespace App.Services
{
    public class SaveService : ISaveService
    {
        public void Save()
        {
            Debug.Log("Game Saved");
        }

        public void Load()
        {
            Debug.Log("Game Loaded");
        }
    }
}