using App.SaveLoad;
using App.Repositories;
using System.Collections.Generic;

public class SaveLoadInteractor
{
    // Репозиторий отвечает за реальное хранение данных (файл, JSON и т.д.)
    private readonly IPlayerRepository _repository;

    public SaveLoadInteractor(IPlayerRepository repository)
    {
        _repository = repository;
    }

    // Сохраняет данные через репозиторий
    public void Save(PlayerSaveData data)
    {
        _repository.Save(data);
    }

    // Загружает данные через репозиторий
    public PlayerSaveData Load()
    {
        return _repository.Load();
    }
}