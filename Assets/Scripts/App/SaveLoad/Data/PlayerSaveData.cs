using System.Collections.Generic;

namespace App.SaveLoad
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;

        public float CurrentHp;
        public float MaxHp;

        public List<EnemySaveData> Enemies;
    }
}