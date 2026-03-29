using UnityEngine;
using App.SaveLoad;
using App.Repositories;
using Presentation.Scene;
using System.Collections.Generic;

public class SaveLoadInteractor
{
    private readonly IPlayerRepository _repository;

    public SaveLoadInteractor(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public void Save(
        Transform playerTransform,
        float currentHp,
        float maxHp,
        BaseEnemyView[] enemies
    )
    {
        Debug.Log("Interactor.Save CALLED");

        PlayerSaveData data = new PlayerSaveData
        {
            PositionX = playerTransform.position.x,
            PositionY = playerTransform.position.y,
            PositionZ = playerTransform.position.z,
            CurrentHp = currentHp,
            MaxHp = maxHp,
            Enemies = new List<EnemySaveData>()
        };

        foreach (var enemy in enemies)
        {
            if (enemy == null)
                continue;

            var entity = enemy.GetEntity();

            var enemyData = new EnemySaveData
            {
                PositionX = enemy.transform.position.x,
                PositionY = enemy.transform.position.y,
                PositionZ = enemy.transform.position.z,
                CurrentHp = entity.CurrentHP,
                Id = enemy.GetId(),
                IsDead = entity.IsDead
            };

            data.Enemies.Add(enemyData);
        }

        _repository.Save(data);
    }

    public PlayerSaveData Load()
    {
        return _repository.Load();
    }
}