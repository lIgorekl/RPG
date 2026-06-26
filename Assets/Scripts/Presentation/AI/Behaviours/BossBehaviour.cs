using UnityEngine;
using App.Services;
using Gameplay.Combat.Boss;
using Gameplay.Combat.Weapons;
using Presentation.Combat;

namespace Presentation.AI
{
    // Поведение босса
    // Управляет обнаружением игрока, атаками, фазами боя,
    // случайными вариантами оружия и стихии
    public class BossBehaviour : BaseEnemyBehaviour,
        IEnemyWeaponHolder,
        IBossVariantAssignable
    {
        // Базовый множитель урона тяжелой атаки
        private const float BaseHeavySlamDamageMultiplier = 2.5f;

        // Радиус обнаружения и атаки игрока
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackRadius = 3f;

        // Наборы возможного оружия и стихий босса
        [SerializeField] private BossWeaponLoadout weaponLoadout;
        [SerializeField] private BossElementLoadout elementLoadout;

        // Управляет визуальным отображением выбранной стихии
        [SerializeField] private BossElementVisualController elementVisuals;

        // Активирован ли босс
        private bool _isActivated;

        // Находится ли босс в фазе ярости
        private bool _isEnraged;

        // Были ли уже назначены случайные варианты
        private bool _variantsAssigned;

        // Текущее оружие босса
        private EnemyWeapon _currentWeapon;

        // Текущая стихия босса
        private BossElementConfig _currentElement;

        // Радиус атаки
        public float AttackRadius => attackRadius;

        // Радиус обнаружения игрока
        public override float DetectionRadius =>
            detectionRadius;

        // Активирован ли босс
        public bool IsActivated => _isActivated;

        // Находится ли босс в фазе ярости
        public bool IsEnraged => _isEnraged;

        // Множитель скорости атаки
        public float AttackSpeedMultiplier { get; private set; } = 1f;

        // Текущее оружие
        public EnemyWeapon CurrentWeapon => _currentWeapon;

        // Время между атаками
        public float AttackCooldown =>
            _currentWeapon?.AttackCooldown ?? 1.5f;

        // Множитель обычного урона
        public float AttackDamageMultiplier =>
            _currentWeapon?.DamageMultiplier ?? 1f;

        // Множитель тяжелой атаки
        public float HeavyAttackDamageMultiplier =>
            BaseHeavySlamDamageMultiplier * AttackDamageMultiplier;

        // Назначает оружие боссу
        public void SetWeapon(EnemyWeaponConfig config)
        {
            _currentWeapon =
                config != null ? new EnemyWeapon(config) : null;
        }

        // Назначает стихию и обновляет внешний вид босса
        public void SetElement(BossElementConfig config)
        {
            _currentElement = config;

            if (elementVisuals == null)
                elementVisuals =
                    GetComponent<BossElementVisualController>();

            elementVisuals?.ApplyElement(config);
        }

        // Случайным образом выбирает оружие и стихию
        public void AssignRandomVariants(System.Random random)
        {
            if (weaponLoadout != null)
                SetWeapon(weaponLoadout.PickRandom(random));

            if (elementLoadout != null)
                SetElement(elementLoadout.PickRandom(random));

            _variantsAssigned = true;
        }

        protected override void Start()
        {
            // Если варианты еще не назначены, выбираем случайные
            if (!_variantsAssigned)
                AssignRandomVariants(new System.Random());

            // Применяем выбранную стихию
            if (_currentElement != null)
                SetElement(_currentElement);

            // Создаем машину состояний босса
            _stateMachine =
                new BossStateMachine(this);

            base.Start();

            // Запускаем стартовое состояние
            ((BossStateMachine)_stateMachine).EnterIdle();
        }

        protected override void Update()
        {
            // Обновляем машину состояний
            base.Update();

            // Проверяем смену фазы боя
            UpdatePhase();
        }

        // Активирует босса
        // Используется после первого получения урона
        public void Activate()
        {
            _isActivated = true;
        }

        // Включает или отключает фазу ярости
        public void SetEnraged(bool value)
        {
            _isEnraged = value;

            // В ярости босс атакует быстрее
            AttackSpeedMultiplier =
                value ? 2f : 1f;

            // Меняем цвет модели для визуального эффекта
            var renderer =
                GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                renderer.material.color =
                    value ? Color.red : Color.white;
            }
        }

        // Проверяет необходимость перехода в фазу ярости
        private void UpdatePhase()
        {
            var entity = EnemyView.GetEntity();

            // Вычисляем процент оставшегося здоровья
            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            bool shouldBeEnraged =
                hpPercent < 0.5f;

            // Если состояние не изменилось — ничего не делаем
            if (shouldBeEnraged == _isEnraged)
                return;

            SetEnraged(shouldBeEnraged);

            // При входе в ярость запускаем соответствующее состояние
            if (shouldBeEnraged)
            {
                ((BossStateMachine)_stateMachine)
                    .EnterEnrage();
            }
        }

        // Проверяет, должен ли босс обнаружить игрока
        public bool ShouldDetectPlayer()
        {
            var mode =
                GameModeService.CurrentMode;

            // В обычном режиме реагирует на расстояние
            if (mode == GameMode.Normal)
            {
                return CombatEvaluator.IsTargetDetected(
                    Self,
                    Player,
                    DetectionRadius);
            }

            // В мирном режиме начинает бой только после активации
            if (mode == GameMode.Peaceful)
            {
                return IsActivated;
            }

            return false;
        }

        // Проверяет возможность начать атаку
        public bool ShouldAttack()
        {
            return CombatEvaluator.CanMeleeAttack(
                Self,
                Player,
                AttackRadius);
        }

        // Проверяет необходимость прекратить преследование
        public bool ShouldReturnToIdle()
        {
            return CombatEvaluator.IsTargetLost(
                Self,
                Player,
                DetectionRadius);
        }

        // Переводит босса в состояние оглушения
        public void EnterStunState(float duration)
        {
            ((BossStateMachine)_stateMachine)
                .EnterStun(duration);
        }
    }
}