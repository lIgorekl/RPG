using UnityEngine.SceneManagement;

namespace App.Services
{
    public class SceneService : ISceneService
    {
        public void LoadGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}