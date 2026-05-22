using UnityEngine;

namespace Presentation.AI
{
    public class BossEnrageState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _timer;

        public BossEnrageState(
            BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = 3f;

            _behaviour.SetEnraged(true);
        }

        public void Update()
        {
            _behaviour.TickEnrage(
                ref _timer);
        }

        public void Exit() { }
    }
}