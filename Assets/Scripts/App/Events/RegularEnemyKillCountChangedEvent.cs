namespace App.Events
{
    public readonly struct RegularEnemyKillCountChangedEvent
    {
        public RegularEnemyKillCountChangedEvent(int killCount)
        {
            KillCount = killCount;
        }

        public int KillCount { get; }
    }
}
