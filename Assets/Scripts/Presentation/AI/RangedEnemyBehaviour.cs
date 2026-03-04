using UnityEngine;
using Presentation.Scene;

namespace Presentation.AI
{
    public class RangedEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 12f;
        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private float moveSpeed = 3f;
        public Presentation.Player.PlayerController PlayerController => _playerController;
        private Presentation.Player.PlayerController _playerController;

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
            _playerController = _player.GetComponent<Presentation.Player.PlayerController>();
            _stateMachine.ChangeState(new RangedIdleState(this));
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public Transform Player => _player;
        public Transform Self => transform;
        public float DetectionRadius => detectionRadius;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public float MoveSpeed => moveSpeed;
        public BaseEnemyView EnemyView => _enemyView;
        public EnemyStateMachine StateMachine => _stateMachine;
    }
}