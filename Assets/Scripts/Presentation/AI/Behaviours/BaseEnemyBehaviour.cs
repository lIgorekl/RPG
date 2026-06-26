using UnityEngine;
using UnityEngine.AI;
using Presentation.Scene;
using App.Services;
using Gameplay.AI;

namespace Presentation.AI
{
    // Базовый класс для всех типов врагов
    // Хранит общие компоненты и управляет работой AI через машину состояний
    public abstract class BaseEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        // Машина состояний врага
        protected EnemyStateMachineBase _stateMachine;

        // Представление врага
        protected BaseEnemyView _enemyView;

        // Компонент навигации Unity
        protected NavMeshAgent _agent;

        // Политика агрессии врага
        protected IAggroPolicy _aggroPolicy;

        // Сервис случайного перемещения
        protected EnemyWanderService _wanderService;

        // Сервис движения врага
        protected EnemyMovementService _movementService;

        // Сервис проверки боевых условий
        protected EnemyCombatEvaluator _combatEvaluator;

        private IGameModeService _gameModeService;

        // Ссылка на игрока
        [SerializeField] protected Transform _player;

        // Публичный доступ к основным объектам для состояний AI
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

        // Радиус обнаружения задается конкретным типом врага
        public abstract float DetectionRadius { get; }

        // Устанавливает ссылку на игрока
        public void SetPlayer(Transform player)
        {
            _player = player;
        }

        // Инициализирует зависимости AI
        public void Initialize(
            IGameModeService gameModeService)
        {
            _gameModeService = gameModeService;

            // Создаем сервис случайного патрулирования
            _wanderService = new EnemyWanderService();

            // Создаем сервис перемещения
            _movementService = new EnemyMovementService();

            // Создаем сервис проверки боевых условий
            _combatEvaluator = new EnemyCombatEvaluator();

            // Получаем представление врага
            _enemyView = GetComponent<BaseEnemyView>();

            // Получаем NavMeshAgent для навигации
            _agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            // Проверяем наличие ссылки на игрока
            if (_player == null)
            {
                Debug.LogError($"{name}: Player reference not set!");
            }

            // Проверяем находится ли агент на NavMesh
            if (_agent != null && !_agent.isOnNavMesh)
            {
                // Если нет, пытаемся найти ближайшую точку NavMesh
                if (UnityEngine.AI.NavMesh.SamplePosition(
                    transform.position,
                    out UnityEngine.AI.NavMeshHit hit,
                    2f,
                    UnityEngine.AI.NavMesh.AllAreas))
                {
                    // Перемещаем агента на найденную точку
                    _agent.Warp(hit.position);
                }
            }

            // Выбираем логику агрессии в зависимости от режима игры
            if (_gameModeService != null &&
                _gameModeService.CurrentMode == GameMode.Peaceful)
            {
                // Мирный режим — враги не атакуют игрока
                _aggroPolicy = new PeacefulAggroPolicy();
            }
            else
            {
                // Обычный режим — враги реагируют на игрока
                _aggroPolicy = new NormalAggroPolicy();
            }
        }

        protected virtual void Update()
        {
            // Мертвые враги больше не обновляют AI
            if (_enemyView.IsDead)
                return;

            // Передаем обновление текущему состоянию
            _stateMachine.Update();
        }
    }
}