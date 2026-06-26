namespace Gameplay.AI
{
    // Политика агрессии для обычного режима игры
    // Разрешает врагам автоматически атаковать игрока
    public class NormalAggroPolicy : IAggroPolicy
    {
        // В обычном режиме агрессия всегда разрешена
        public bool CanAggro()
        {
            return true;
        }
    }
}