using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Presentation.AI
{
    public class RangedAttackState : IEnemyState
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        private float _cooldown;
        private float _timer;

        public RangedAttackState(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = 0f;
            _cooldown = ResolveAttackCooldown(
                _stateMachine.RangedBehaviour);

            _stateMachine.RangedBehaviour.EnterRangedAttack();
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.RangedBehaviour;

            if (behaviour.ShouldStopRangedAttack())
            {
                _stateMachine.EnterMaintainDistance();
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                behaviour.UpdateRangedAttack();

                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            _stateMachine.RangedBehaviour.ExitRangedAttack();
        }

        private static float ResolveAttackCooldown(
            RangedEnemyBehaviour behaviour)
        {
            if (behaviour is IEnemyWeaponHolder holder)
                return holder.AttackCooldown;

            return 2f;
        }
    }
}
