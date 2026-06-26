namespace App.Events
{
    // Событие изменения количества убитых обычных врагов.
    // Передается через шину событий всем системам,
    // которым важно знать текущее количество убийств.
    public readonly struct RegularEnemyKillCountChangedEvent
    {
        // Создает событие и сохраняет
        // текущее количество убийств
        public RegularEnemyKillCountChangedEvent(
            int killCount)
        {
            KillCount = killCount;
        }

        // Количество убитых обычных врагов
        public int KillCount { get; }
    }
}