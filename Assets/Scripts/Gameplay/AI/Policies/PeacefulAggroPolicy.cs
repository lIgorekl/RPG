namespace Gameplay.AI
{
    public class PeacefulAggroPolicy : IAggroPolicy
    {
        public bool CanAggro()
        {
            return false;
        }
    }
}