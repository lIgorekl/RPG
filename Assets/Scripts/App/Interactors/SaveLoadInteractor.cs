using App.SaveLoad;
using App.Repositories;
using System.Collections.Generic;

public class SaveLoadInteractor
{
    private readonly IPlayerRepository _repository;

    public SaveLoadInteractor(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public void Save(PlayerSaveData data)
    {
        _repository.Save(data);
    }

    public PlayerSaveData Load()
    {
        return _repository.Load();
    }
}