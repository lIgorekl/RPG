using UnityEngine.SceneManagement;

namespace App.Services
{
    // Сервис загрузки сцен
    // Отвечает за переход между главным меню и игровой сценой
    public class SceneService : ISceneService
    {
        // Загружает игровую сцену
        public void LoadGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        // Загружает сцену главного меню
        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}