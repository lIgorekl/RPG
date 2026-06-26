namespace App.Services
{
    // Интерфейс сервиса загрузки сцен
    // Определяет методы перехода между основными сценами игры
    public interface ISceneService
    {
        // Загружает игровую сцену
        void LoadGame();

        // Загружает главное меню
        void LoadMenu();
    }
}