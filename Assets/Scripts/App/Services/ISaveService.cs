using UnityEngine;
using App.SaveLoad;
using Presentation.Scene;

namespace App.Services
{
    public interface ISaveService
    {
        void Save(
            Transform playerTransform,
            float currentHp,
            float maxHp,
            BaseEnemyView[] enemies
        );
        PlayerSaveData Load();
    }
}