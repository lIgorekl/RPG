using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Presentation.AI
{
    // Состояние атаки дальнего врага
    // Выполняет периодическую стрельбу по игроку
    public class RangedAttackState : IEnemyState
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        // Время между атаками
        private float _cooldown;

        // Текущий таймер до следующего выстрела
        private float _timer;

        public RangedAttackState(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            // Позволяем атаковать сразу после входа в состояние
            _timer = 0f;

            // Получаем кулдаун текущего оружия
            _cooldown = ResolveAttackCooldown(
                _stateMachine.RangedBehaviour);

            // Подготавливаем врага к атаке
            _stateMachine.RangedBehaviour.EnterRangedAttack();
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.RangedBehaviour;

            // Если игрок вышел из подходящей дистанции,
            // возвращаемся к удержанию дистанции
            if (behaviour.ShouldStopRangedAttack())
            {
                _stateMachine.EnterMaintainDistance();
                return;
            }

            // Уменьшаем таймер следующей атаки
            _timer -= Time.deltaTime;

            // Когда кулдаун закончился, выполняем выстрел
            if (_timer <= 0f)
            {
                behaviour.UpdateRangedAttack();

                // Запускаем новый отсчет до следующей атаки
                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            // Завершаем состояние атаки
            _stateMachine.RangedBehaviour.ExitRangedAttack();
        }

        // Определяет время перезарядки оружия
        private static float ResolveAttackCooldown(
            RangedEnemyBehaviour behaviour)
        {
            // Если враг использует систему оружия,
            // берем кулдаун из текущего оружия
            if (behaviour is IEnemyWeaponHolder holder)
                return holder.AttackCooldown;

            // Значение по умолчанию
            return 2f;
        }
    }
}
