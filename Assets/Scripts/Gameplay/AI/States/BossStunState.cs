using UnityEngine;

namespace Presentation.AI
{
    public class BossStunState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;
        private float _timer;

        public BossStunState(BossBehaviour behaviour, float duration)
        {
            _behaviour = behaviour;
            _timer = 2f;
        }

        public void Enter()
        {
            Debug.Log("BOSS STUN");

            _behaviour.MovementService.Stop(_behaviour.Agent);

            if (_behaviour.EnemyView.Animator != null)
            {
                _behaviour.EnemyView.Animator.SetTrigger("Hurt");
            }
        }

        public void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateBossChase(_behaviour));
            }
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }
    }
}