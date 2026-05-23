using UnityEngine;

namespace Presentation.AI
{
    public class BossEnrageState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _timer;

        public BossEnrageState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = 3f;

            _stateMachine.BossBehaviour.SetEnraged(true);
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _stateMachine.EnterChase();
            }
        }

        public void Exit() { }
    }
}
