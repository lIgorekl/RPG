using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Presentation.AI
{
    // Состояние обычной атаки босса
    // Босс останавливается и периодически наносит удары игроку
    public class BossAttackState : IEnemyState
    {
        // Машина состояний босса
        private readonly BossStateMachine _stateMachine;

        // Время между атаками
        private float _cooldown;

        // Таймер до следующей атаки
        private float _timer;

        public BossAttackState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние атаки
        public void Enter()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Получаем кулдаун текущего оружия
            _cooldown = ResolveAttackCooldown(behaviour);

            // Запускаем таймер первой атаки
            _timer = _cooldown;

            // Во время атаки босс не должен двигаться
            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Если игрок вышел из радиуса атаки,
            // возвращаемся к преследованию
            if (!behaviour.ShouldAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            // Уменьшаем таймер.
            // В фазе ярости скорость уменьшается быстрее,
            // поэтому атаки происходят чаще
            _timer -=
                Time.deltaTime *
                behaviour.AttackSpeedMultiplier;

            // Когда кулдаун закончился
            if (_timer <= 0f)
            {
                // Проверяем еще раз,
                // находится ли игрок рядом
                if (!behaviour.ShouldAttack())
                {
                    _stateMachine.EnterChase();
                    return;
                }

                // Выполняем атаку
                PerformAttack(behaviour);

                // Запускаем новый кулдаун
                _timer = _cooldown;
            }
        }

        // Вызывается при выходе из состояния
        public void Exit()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            // Возобновляем движение босса
            behaviour.MovementService.Resume(
                behaviour.Agent);
        }

        // Выполняет обычную атаку босса
        private static void PerformAttack(BossBehaviour behaviour)
        {
            // Некоторые виды оружия используют тяжелую анимацию
            bool useHeavyAnimation =
                behaviour.CurrentWeapon?.UseHeavyAttackAnimation ?? false;

            behaviour.EnemyView.Attack(
                behaviour.Player,
                behaviour.AttackDamageMultiplier,
                useHeavyAnimation);
        }

        // Возвращает кулдаун текущего оружия
        private static float ResolveAttackCooldown(
            BossBehaviour behaviour)
        {
            if (behaviour is IEnemyWeaponHolder holder)
                return holder.AttackCooldown;

            // Значение по умолчанию
            return 1.5f;
        }
    }
}