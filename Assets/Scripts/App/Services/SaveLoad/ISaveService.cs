using UnityEngine;
using App.SaveLoad;
using Presentation.Scene;

namespace App.Services
{
    public interface ISaveService
    {
        void Save(PlayerSaveData data);
        PlayerSaveData Load();
    }
}