using UnityEngine;
using App.Services;

namespace App
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("Game Scene Entry Initialized");

            // Получаем сервисы
            var saveService = GameEntryPoint.Instance.GetSaveService();
            var audioService = GameEntryPoint.Instance.GetAudioService();

            // Пример использования (можно оставить лог)
            Debug.Log("Services injected into scene");
        }
    }
}