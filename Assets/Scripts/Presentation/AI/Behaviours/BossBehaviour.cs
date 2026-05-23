using UnityEngine;
using App.Services;
using Gameplay.Combat.Boss;
using Gameplay.Combat.Weapons;
using Presentation.Combat;

namespace Presentation.AI
{
    public class BossBehaviour : BaseEnemyBehaviour,
        IEnemyWeaponHolder,
        IBossVariantAssignable
    {
        private const float BaseHeavySlamDamageMultiplier = 2.5f;

        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackRadius = 3f;
        [SerializeField] private BossWeaponLoadout weaponLoadout;
        [SerializeField] private BossElementLoadout elementLoadout;
        [SerializeField] private BossElementVisualController elementVisuals;

        private bool _isActivated;
        private bool _isEnraged;
        private bool _variantsAssigned;
        private EnemyWeapon _currentWeapon;

        public float AttackRadius => attackRadius;

        public override float DetectionRadius =>
            detectionRadius;

        public bool IsActivated => _isActivated;

        public bool IsEnraged => _isEnraged;

        public float AttackSpeedMultiplier { get; private set; } = 1f;

        public EnemyWeapon CurrentWeapon => _currentWeapon;

        public float AttackCooldown =>
            _currentWeapon?.AttackCooldown ?? 1.5f;

        public float AttackDamageMultiplier =>
            _currentWeapon?.DamageMultiplier ?? 1f;

        public float HeavyAttackDamageMultiplier =>
            BaseHeavySlamDamageMultiplier * AttackDamageMultiplier;

        public void SetWeapon(EnemyWeaponConfig config)
        {
            _currentWeapon =
                config != null ? new EnemyWeapon(config) : null;
        }

        public void SetElement(BossElementConfig config)
        {
            if (elementVisuals == null)
                elementVisuals =
                    GetComponent<BossElementVisualController>();

            elementVisuals?.ApplyElement(config);
        }

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
            if (!_variantsAssigned)
                AssignRandomVariants(new System.Random());

            _stateMachine =
                new BossStateMachine(this);

            base.Start();

            ((BossStateMachine)_stateMachine).EnterIdle();
        }

        protected override void Update()
        {
            base.Update();

            UpdatePhase();
        }

        public void Activate()
        {
            _isActivated = true;
        }

        public void SetEnraged(bool value)
        {
            _isEnraged = value;

            AttackSpeedMultiplier =
                value ? 2f : 1f;

            var renderer =
                GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                renderer.material.color =
                    value ? Color.red : Color.white;
            }
        }

        private void UpdatePhase()
        {
            var entity = EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            bool shouldBeEnraged =
                hpPercent < 0.5f;

            if (shouldBeEnraged == _isEnraged)
                return;

            SetEnraged(shouldBeEnraged);

            if (shouldBeEnraged)
            {
                ((BossStateMachine)_stateMachine)
                    .EnterEnrage();
            }
        }

        public bool ShouldDetectPlayer()
        {
            var mode =
                GameModeService.CurrentMode;

            if (mode == GameMode.Normal)
            {
                return CombatEvaluator.IsTargetDetected(
                    Self,
                    Player,
                    DetectionRadius);
            }

            if (mode == GameMode.Peaceful)
            {
                return IsActivated;
            }

            return false;
        }

        public bool ShouldAttack()
        {
            return CombatEvaluator.CanMeleeAttack(
                Self,
                Player,
                AttackRadius);
        }

        public bool ShouldReturnToIdle()
        {
            return CombatEvaluator.IsTargetLost(
                Self,
                Player,
                DetectionRadius);
        }

        public void EnterStunState(float duration)
        {
            ((BossStateMachine)_stateMachine)
                .EnterStun(duration);
        }
    }
}
