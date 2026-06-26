using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Presentation.AI
{
    // Состояние атаки ближнего врага
    // Выполняет атаки по игроку с учетом времени перезарядки
    public class AttackState : IEnemyState
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        // Время между атаками
        private float _cooldown;

        // Текущий таймер до следующей атаки
        private float _timer;

        public AttackState(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        // Вызывается при входе в состояние атаки
        public void Enter()
        {
            // Первая атака выполняется сразу
            _timer = 0f;

            // Получаем кулдаун текущего оружия
            _cooldown = ResolveAttackCooldown(
                _stateMachine.EnemyBehaviour);

            // Переводим врага в режим атаки
            _stateMachine.EnemyBehaviour.EnterAttack();
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.EnemyBehaviour;

            // Если игрок вышел из радиуса атаки,
            // возвращаемся к преследованию
            if (behaviour.ShouldStopAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            // Уменьшаем таймер между атаками
            _timer -= Time.deltaTime;

            // Когда кулдаун закончился, выполняем новую атаку
            if (_timer <= 0f)
            {
                behaviour.UpdateAttack();

                // Запускаем кулдаун заново
                _timer = _cooldown;
            }
        }

        // Вызывается при выходе из состояния атаки
        public void Exit()
        {
            _stateMachine.EnemyBehaviour.ExitAttack();
        }

        // Определяет время перезарядки текущего оружия
        private static float ResolveAttackCooldown(
            EnemyBehaviour behaviour)
        {
            // Если враг использует систему оружия,
            // берем кулдаун из настроек оружия
            if (behaviour is IEnemyWeaponHolder holder)
                return holder.AttackCooldown;

            // Значение по умолчанию
            return 1f;
        }
    }
}