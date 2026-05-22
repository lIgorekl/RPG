using UnityEngine;
using UnityEngine.AI;
using Presentation.Scene;
using App.Services;
using Gameplay.AI;

namespace Presentation.AI
{
    // Базовое поведение всех врагов.
    // Содержит общую логику AI: state machine, NavMeshAgent и ссылку на игрока.
    public abstract class BaseEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        // Основные компоненты AI
        protected EnemyStateMachineBase _stateMachine;
        protected BaseEnemyView _enemyView;
        protected NavMeshAgent _agent;
        protected IAggroPolicy _aggroPolicy;
        protected EnemyWanderService _wanderService;
        protected EnemyMovementService _movementService;
        protected EnemyCombatEvaluator _combatEvaluator;
        private IGameModeService _gameModeService;

        [SerializeField] protected Transform _player;

        // Публичный доступ для состояний
        public Transform Player => _player;
        public Transform Self => transform;
        public BaseEnemyView EnemyView => _enemyView;
        public EnemyStateMachineBase StateMachine => _stateMachine;
        public NavMeshAgent Agent => _agent;
        public IGameModeService GameModeService => _gameModeService;
        public IAggroPolicy AggroPolicy => _aggroPolicy;
        public EnemyWanderService WanderService => _wanderService;
        public EnemyMovementService MovementService => _movementService;
        public EnemyCombatEvaluator CombatEvaluator => _combatEvaluator;

        // Радиус обнаружения задаётся в конкретных типах врагов
        public abstract float DetectionRadius { get; }

        public void Initialize(
            IGameModeService gameModeService)
        {
            _gameModeService = gameModeService;

            _wanderService = new EnemyWanderService();

            _movementService = new EnemyMovementService();

            _combatEvaluator = new EnemyCombatEvaluator();

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

            if (_agent != null && !_agent.isOnNavMesh)
            {
                if (UnityEngine.AI.NavMesh.SamplePosition(
                    transform.position,
                    out UnityEngine.AI.NavMeshHit hit,
                    2f,
                    UnityEngine.AI.NavMesh.AllAreas))
                {
                    _agent.Warp(hit.position);
                }
            }

            if (_gameModeService != null &&
                _gameModeService.CurrentMode == GameMode.Peaceful)
            {
                _aggroPolicy = new PeacefulAggroPolicy();
            }
            else
            {
                _aggroPolicy = new NormalAggroPolicy();
            }
        }

        protected virtual void Update()
        {
            if (_enemyView.IsDead)
                return;

            _stateMachine.Update();
        }
    }
}