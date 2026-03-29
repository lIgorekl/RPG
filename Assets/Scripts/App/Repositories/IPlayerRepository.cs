using App.SaveLoad;

namespace App.Repositories
{
    public interface IPlayerRepository
    {
        void Save(PlayerSaveData data);
        PlayerSaveData Load();
    }
}