using App.SaveLoad;

namespace App.Repositories
{
    // Интерфейс репозитория игрока
    // Определяет методы сохранения и загрузки данных игрока
    public interface IPlayerRepository
    {
        // Сохраняет данные в хранилище
        void Save(PlayerSaveData data);

        // Загружает данные из хранилища
        PlayerSaveData Load();
    }
}