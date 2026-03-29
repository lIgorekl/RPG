using System;

namespace App.SaveLoad
{
    [Serializable]
    public class EnemySaveData
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;

        public int CurrentHp;
        public bool IsDead;

        public string Id;
    }
}