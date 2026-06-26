namespace Gameplay.AI
{
    // Политика агрессии для мирного режима игры
    // Запрещает врагам автоматически атаковать игрока
    public class PeacefulAggroPolicy : IAggroPolicy
    {
        // В мирном режиме агрессия всегда запрещена
        public bool CanAggro()
        {
            return false;
        }
    }
}