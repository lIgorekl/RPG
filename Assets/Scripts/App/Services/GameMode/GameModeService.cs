namespace App.Services
{
    // Доступные режимы игры
    public enum GameMode
    {
        Normal,
        Peaceful
    }

    // Интерфейс сервиса игрового режима
    // Позволяет получать и изменять текущий режим игры
    public interface IGameModeService
    {
        GameMode CurrentMode { get; }

        void SetMode(GameMode mode);
    }

    // Сервис хранения текущего режима игры
    public class GameModeService : IGameModeService
    {
        // Текущий режим игры
        public GameMode CurrentMode { get; private set; } =
            GameMode.Normal;

        // Устанавливает режим игры
        public void SetMode(GameMode mode)
        {
            CurrentMode = mode;
        }
    }
}