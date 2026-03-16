using UnityEngine;
using Presentation.Scene;
using UnityEngine.AI;

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
        private NavMeshAgent _agent;

        private void Awake()
        {
            _enemyView = GetComponent<BaseEnemyView>();
            _agent = GetComponent<NavMeshAgent>();

            _stateMachine = new EnemyStateMachine();
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;

            _stateMachine.ChangeState(new IdleState(this));
        }

        private void Update()
        {
            if (_enemyView.IsDead)
                return;

            if (_enemyView.IsStunned)
                return;

            _stateMachine.Update();
        }

        public float DetectionRadius => detectionRadius;
        public float AttackRadius => attackRadius;
        public float MoveSpeed => moveSpeed;
        public Transform Player => _player;
        public Transform Self => transform;
        public BaseEnemyView EnemyView => _enemyView;
        public EnemyStateMachine StateMachine => _stateMachine;
        public NavMeshAgent Agent => _agent;
    }
}