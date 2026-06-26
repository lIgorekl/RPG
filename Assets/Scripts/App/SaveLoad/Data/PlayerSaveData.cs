using System.Collections.Generic;

namespace App.SaveLoad
{
    // Данные сохранения игрока
    // Содержат всю информацию, необходимую для восстановления состояния игры
    [System.Serializable]
    public class PlayerSaveData
    {
        // Позиция игрока в мире
        public float PositionX;
        public float PositionY;
        public float PositionZ;

        // Здоровье игрока
        public float CurrentHp;
        public float MaxHp;

        // Список всех врагов на сцене
        public List<EnemySaveData> Enemies;
    }
}