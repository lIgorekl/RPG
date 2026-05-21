using UnityEngine;

namespace Presentation.AI
{
    public class BossEnrageState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _duration = 3f;
        private float _timer;

        public BossEnrageState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = _duration;

            _behaviour.SetEnraged(true);

            var renderer = _behaviour.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = Color.red;
            }

            Debug.Log("BOSS ENRAGED");
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
        }
    }
}