using UnityEngine;

namespace Presentation.AI
{
    // Поведение врага ближнего боя.
    // Использует state machine из BaseEnemyBehaviour.
    public class EnemyBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float attackRadius = 2f;
        [SerializeField] private float moveSpeed = 3f;

        protected override void Start()
        {
            base.Start();

            // Начальное состояние врага
            _stateMachine.ChangeState(new IdleState(this));
        }

        // Радиус, на котором враг начинает реагировать на игрока
        public override float DetectionRadius => detectionRadius;

        // Дистанция атаки
        public float AttackRadius => attackRadius;

        // Скорость перемещения
        public float MoveSpeed => moveSpeed;
    }
}