using System;

namespace App.SaveLoad
{
    // Данные сохранения врага
    // Используются для восстановления состояния врага после загрузки игры
    [Serializable]
    public class EnemySaveData
    {
        // Позиция врага на сцене
        public float PositionX;
        public float PositionY;
        public float PositionZ;

        // Текущее здоровье врага
        public int CurrentHp;

        // Флаг смерти врага
        public bool IsDead;

        // Уникальный идентификатор врага
        public string Id;
    }
}