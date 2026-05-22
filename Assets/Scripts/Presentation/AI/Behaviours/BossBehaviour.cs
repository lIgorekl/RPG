using UnityEngine;

namespace Presentation.AI
{
    public class BossBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackRadius = 3f;

        private bool _isActivated;
        private bool _isEnraged;

        public float AttackRadius => attackRadius;

        public override float DetectionRadius =>
            detectionRadius;

        public bool IsActivated => _isActivated;

        public bool IsEnraged => _isEnraged;

        public float AttackSpeedMultiplier { get; private set; } = 1f;

        protected override void Start()
        {
            _stateMachine =
                new BossStateMachine();

            base.Start();

            EnterIdle();
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
                EnterEnrage();
            }
        }

        /*
         * =========================
         * TRANSITIONS
         * =========================
         */

        public void EnterIdle()
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossIdle(this);
        }

        public void EnterChase()
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossChase(this);
        }

        public void EnterAttack()
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossAttack(this);
        }

        public void EnterHeavyAttack()
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossHeavyAttack(this);
        }

        public void EnterRecover()
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossRecover(this);
        }

        public void EnterStun(float duration)
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossStun(
                    this,
                    duration);
        }

        public void EnterEnrage()
        {
            ((BossStateMachine)_stateMachine)
                .EnterBossEnrage(this);
        }

        /*
         * =========================
         * LOGIC
         * =========================
         */

        public bool ShouldDetectPlayer()
        {
            var mode =
                GameModeService.CurrentMode;

            if (mode == App.Services.GameMode.Normal)
            {
                return CombatEvaluator.IsTargetDetected(
                    Self,
                    Player,
                    DetectionRadius);
            }

            if (mode == App.Services.GameMode.Peaceful)
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

        /*
         * =========================
         * STATE TICKS
         * =========================
         */

        public void TickIdle(
            ref float wanderTimer,
            float wanderDelay)
        {
            if (ShouldDetectPlayer())
            {
                EnterChase();
                return;
            }

            wanderTimer -= Time.deltaTime;

            if (wanderTimer <= 0f)
            {
                WanderService.TryWander(
                    Agent,
                    Self,
                    6f);

                wanderTimer = wanderDelay;
            }
        }

        public void TickChase()
        {
            if (ShouldReturnToIdle())
            {
                EnterIdle();
                return;
            }

            if (ShouldAttack())
            {
                if (Random.value > 0.5f)
                {
                    EnterHeavyAttack();
                }
                else
                {
                    EnterAttack();
                }

                return;
            }

            MovementService.RotateTo(
                Self,
                Player.position);

            MovementService.MoveTo(
                Agent,
                Player.position);
        }

        public void TickAttack(
            ref float timer,
            float cooldown)
        {
            if (!ShouldAttack())
            {
                EnterChase();
                return;
            }

            timer -=
                Time.deltaTime *
                AttackSpeedMultiplier;

            if (timer <= 0f)
            {
                EnemyView.Attack(
                    Player,
                    1f);

                timer = cooldown;
            }
        }

        public void TickHeavyAttack(
            ref float timer)
        {
            timer -=
                Time.deltaTime *
                AttackSpeedMultiplier;

            if (timer <= 0f)
            {
                EnemyView.Attack(
                    Player,
                    2.5f,
                    true);

                EnterRecover();
            }
        }

        public void TickRecover(
            ref float timer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                EnterChase();
            }
        }

        public void TickStun(
            ref float timer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                EnterChase();
            }
        }

        public void TickEnrage(
            ref float timer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                EnterChase();
            }
        }

        public void EnterStunState(float duration)
        {
            EnterStun(duration);
        }
    }
}