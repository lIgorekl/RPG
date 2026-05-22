namespace Presentation.AI
{
    public class BossChaseState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        public BossChaseState(
            BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            _behaviour.TickChase();
        }

        public void Exit() { }
    }
}