using UnityEngine;
using Gameplay.Combat.Weapons;

namespace Presentation.AI
{
    // Поведение врага ближнего боя
    // Управляет преследованием игрока и выполнением атак
    public class EnemyBehaviour : BaseCombatEnemyBehaviour,
        IEnemyWeaponHolder,
        IEnemyWeaponAssignable
    {
        // Радиус обнаружения игрока
        [SerializeField] private float detectionRadius = 10f;

        // Дистанция, на которой враг может атаковать
        [SerializeField] private float attackRadius = 2f;

        // Скорость передвижения врага
        [SerializeField] private float moveSpeed = 3f;

        // Набор доступного оружия
        [SerializeField] private EnemyWeaponLoadout weaponLoadout;

        // Текущее оружие врага
        private EnemyWeapon _currentWeapon;

        public EnemyWeapon CurrentWeapon => _currentWeapon;

        // Время между атаками
        public float AttackCooldown =>
            _currentWeapon?.AttackCooldown ?? 1f;

        // Назначает конкретное оружие врагу
        public void SetWeapon(EnemyWeaponConfig config)
        {
            _currentWeapon =
                config != null ? new EnemyWeapon(config) : null;
        }

        // Выбирает случайное оружие из доступного набора
        public void AssignRandomWeapon(System.Random random)
        {
            if (weaponLoadout == null)
                return;

            SetWeapon(weaponLoadout.PickRandom(random));
        }

        protected override void Start()
        {
            // Если оружие не выбрано, назначаем случайное
            if (_currentWeapon == null)
                AssignRandomWeapon(new System.Random());

            // Создаем машину состояний ближнего врага
            _stateMachine =
                new MeleeEnemyStateMachine(this);

            base.Start();

            // Переходим в начальное состояние ожидания
            ((MeleeEnemyStateMachine)_stateMachine)
                .EnterIdle();
        }

        // Радиус обнаружения игрока
        public override float DetectionRadius => detectionRadius;

        // Дистанция атаки
        public float AttackRadius => attackRadius;

        // Скорость перемещения
        public float MoveSpeed => moveSpeed;

        // Проверяет находится ли игрок в радиусе атаки
        public bool ShouldAttackPlayer()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance <= AttackRadius;
        }

        // Проверяет потерял ли враг игрока
        public bool ShouldReturnToIdle()
        {
            return !CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        // Преследует игрока
        public void UpdateChase()
        {
            // Перемещаем врага к игроку
            MovementService.MoveTo(
                Agent,
                Player.position);

            // Постоянно разворачиваем врага к цели
            RotateToPlayer();
        }

        // Проверяет нужно ли начинать преследование
        public bool ShouldChasePlayer()
        {
            // Если агрессия запрещена, не реагируем на игрока
            if (!AggroPolicy.CanAggro())
                return false;

            return CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        // Проверяет нужно ли завершить атаку
        public bool ShouldStopAttack()
        {
            float distance = Vector3.Distance(
                Self.position,
                Player.position);

            return distance > AttackRadius;
        }

        // Подготавливает врага к атаке
        public void EnterAttack()
        {
            if (Agent != null)
            {
                // Останавливаем движение во время атаки
                Agent.isStopped = true;
            }
        }

        // Завершает состояние атаки
        public void ExitAttack()
        {
            if (Agent != null)
            {
                // Возвращаем возможность двигаться
                Agent.isStopped = false;
            }
        }

        // Выполняет атаку по игроку
        public void UpdateAttack()
        {
            // Перед атакой всегда смотрим на цель
            RotateToPlayer();

            if (_currentWeapon != null)
            {
                // Используем параметры текущего оружия
                EnemyView.Attack(
                    Player,
                    _currentWeapon.DamageMultiplier,
                    _currentWeapon.UseHeavyAttackAnimation);
            }
            else
            {
                // Используем обычную атаку по умолчанию
                EnemyView.Attack(Player);
            }
        }

        // Переводит врага в состояние оглушения
        public override void EnterStunState(float duration)
        {
            ((MeleeEnemyStateMachine)_stateMachine)
                .EnterStun(duration);
        }
    }
}
