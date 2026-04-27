using UnityEngine;
using UnityEngine.AI;

namespace Presentation.AI
{
    public class BossBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackRadius = 3f;

        private bool _isActivated;
        private bool _isEnraged;


        public float AttackRadius => attackRadius;
        public override float DetectionRadius => detectionRadius;
        public bool IsActivated => _isActivated;
        public bool IsEnraged => _isEnraged;

        public float AttackSpeedMultiplier { get; private set; } = 1f;

        protected override void Start()
        {
            base.Start();
            _stateMachine.ChangeState(new BossIdleState(this));
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

        private void UpdatePhase()
        {
            var entity = EnemyView.GetEntity();

            float hpPercent = (float)entity.CurrentHP / entity.MaxHP;

            bool shouldBeEnraged = hpPercent < 0.5f;

            if (shouldBeEnraged != _isEnraged)
            {
                SetEnraged(shouldBeEnraged);

                if (shouldBeEnraged)
                {
                    Debug.Log("ENTER ENRAGE PHASE");
                    _stateMachine.ChangeState(new BossEnrageState(this));
                }
                else
                {
                    Debug.Log("EXIT ENRAGE PHASE");
                }
            }
        }

        public void SetEnraged(bool value)
        {
            _isEnraged = value;

            AttackSpeedMultiplier = value ? 2f : 1f;

            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = value ? Color.red : Color.white;
            }
        }
    }
}