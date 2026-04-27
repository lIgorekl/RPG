using UnityEngine;
using UnityEngine.AI;
using Presentation.Scene;
using App;
using App.Services;

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
        protected IGameModeService _gameModeService;

        [SerializeField] protected Transform _player;

        // Публичный доступ для состояний
        public Transform Player => _player;
        public Transform Self => transform;
        public BaseEnemyView EnemyView => _enemyView;
        public EnemyStateMachine StateMachine => _stateMachine;
        public NavMeshAgent Agent => _agent;
        public IGameModeService GameModeService => _gameModeService;

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
            _gameModeService = GameEntryPoint.Instance.GetGameModeService();
        }

        protected virtual void Update()
        {
            if (_enemyView.IsDead)
                return;

            var entity = _enemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            float distance = Vector3.Distance(
                transform.position,
                _player.position);

            // УБЕГАЕМ ТОЛЬКО ЕСЛИ ИГРОК РЯДОМ
            float fleeEnterDistance = DetectionRadius;
            float fleeExitDistance = DetectionRadius * 1.5f;

            if (!(this is BossBehaviour))
            {
                if (hpPercent < 0.3f && distance <= fleeEnterDistance)
                {
                    if (!(_stateMachine.CurrentState is FleeState))
                    {
                        _stateMachine.ChangeState(new FleeState(this));
                    }

                    _stateMachine.Update();
                    return;
                }
            }

            _stateMachine.Update();
        }
    }
}