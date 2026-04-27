namespace App.Services
{
    public enum GameMode
    {
        Normal,
        Peaceful
    }

    public interface IGameModeService
    {
        GameMode CurrentMode { get; }
        void SetMode(GameMode mode);
    }

    public class GameModeService : IGameModeService
    {
        public GameMode CurrentMode { get; private set; } = GameMode.Normal;

        public void SetMode(GameMode mode)
        {
            CurrentMode = mode;
        }
    }
}