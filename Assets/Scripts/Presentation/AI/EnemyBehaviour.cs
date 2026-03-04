using UnityEngine;
using Presentation.Scene;

namespace Presentation.AI
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float attackRadius = 2f;
        [SerializeField] private float moveSpeed = 3f;

        private EnemyStateMachine _stateMachine;
        private Transform _player;
        private BaseEnemyView _enemyView;

        private void Awake()
        {
            _stateMachine = new EnemyStateMachine();
            _enemyView = GetComponent<BaseEnemyView>();
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;

            _stateMachine.ChangeState(new IdleState(this));
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public float DetectionRadius => detectionRadius;
        public float AttackRadius => attackRadius;
        public float MoveSpeed => moveSpeed;
        public Transform Player => _player;
        public Transform Self => transform;
        public BaseEnemyView EnemyView => _enemyView;
        public EnemyStateMachine StateMachine => _stateMachine;
    }
}