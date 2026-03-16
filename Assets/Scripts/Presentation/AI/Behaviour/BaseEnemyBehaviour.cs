using UnityEngine;
using UnityEngine.AI;
using Presentation.Scene;

namespace Presentation.AI
{
    // Базовое поведение всех врагов.
    // Содержит общую логику AI: state machine, NavMeshAgent и ссылку на игрока.
    public abstract class BaseEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        // Основные компоненты AI
        protected EnemyStateMachine _stateMachine;
        protected BaseEnemyView _enemyView;
        protected NavMeshAgent _agent;

        [SerializeField] protected Transform _player;

        // Публичный доступ для состояний
        public Transform Player => _player;
        public Transform Self => transform;
        public BaseEnemyView EnemyView => _enemyView;
        public EnemyStateMachine StateMachine => _stateMachine;
        public NavMeshAgent Agent => _agent;

        // Радиус обнаружения задаётся в конкретных типах врагов
        public abstract float DetectionRadius { get; }

        protected virtual void Awake()
        {
            _stateMachine = new EnemyStateMachine();

            _enemyView = GetComponent<BaseEnemyView>();
            _agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            // Проверяем, что ссылка на игрока установлена
            if (_player == null)
            {
                Debug.LogError($"{name}: Player reference not set!");
            }
        }

        protected virtual void Update()
        {
            // Если враг мёртв или оглушён — AI не работает
            if (_enemyView.IsDead)
                return;

            if (_enemyView.IsStunned)
                return;

            _stateMachine.Update();
        }
    }
}