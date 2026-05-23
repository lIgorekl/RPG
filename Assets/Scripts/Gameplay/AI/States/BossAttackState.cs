using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Presentation.AI
{
    public class BossAttackState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _cooldown;
        private float _timer;

        public BossAttackState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            _cooldown = ResolveAttackCooldown(behaviour);
            _timer = _cooldown;

            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            if (!behaviour.ShouldAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            _timer -=
                Time.deltaTime *
                behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                if (!behaviour.ShouldAttack())
                {
                    _stateMachine.EnterChase();
                    return;
                }

                PerformAttack(behaviour);
                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Resume(
                behaviour.Agent);
        }

        private static void PerformAttack(BossBehaviour behaviour)
        {
            bool useHeavyAnimation =
                behaviour.CurrentWeapon?.UseHeavyAttackAnimation ?? false;

            behaviour.EnemyView.Attack(
                behaviour.Player,
                behaviour.AttackDamageMultiplier,
                useHeavyAnimation);
        }

        private static float ResolveAttackCooldown(
            BossBehaviour behaviour)
        {
            if (behaviour is IEnemyWeaponHolder holder)
                return holder.AttackCooldown;

            return 1.5f;
        }
    }
}
