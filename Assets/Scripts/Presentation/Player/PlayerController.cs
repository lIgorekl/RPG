using UnityEngine;
using Gameplay.Characters;
using Gameplay.Stats;
using UnityEngine.InputSystem;
using Core.Combat;
using Presentation.Combat;
using Core.Gameplay;
using App.SaveLoad;
using App;
using App.Services;

namespace Presentation.Player
{
    // Контроллер игрока
    // Обрабатывает движение, атаки, получение урона и сохранение данных
    public class PlayerController : MonoBehaviour
    {
        [Header("Base Stats")]
        
        // Базовые характеристики персонажа
        [SerializeField] private int maxHP = 100;
        [SerializeField] private int physicalDamage = 20;
        [SerializeField] private int magicalDamage = 15;

        // Камера игрока используется для движения и определения направления атак
        [SerializeField] private Camera playerCamera;

        // Параметры передвижения
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float rotationSpeed = 10f;

        // Время оглушения после получения урона
        [SerializeField] private float stunDuration = 0.2f;

        // Настройки магической и ближней атаки
        [SerializeField] private ProjectileView magicProjectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private SwordHitbox swordHitbox;

        // Кулдауны атак
        [SerializeField] private float magicCooldown = 2f;
        [SerializeField] private float meleeCooldown = 0.6f;

        // Подсистемы игрока, каждая отвечает за свою зону ответственности
        private PlayerMovement _movement;
        private PlayerInputReader _inputReader;
        private PlayerCombat _combat;
        private PlayerCombatController _combatController;
        private PlayerLifeController _lifeController;
        private PlayerSavePresenter _savePresenter;

        // Сервис воспроизведения звуков
        private IAudioService _audioService;

        // Свойства для UI кулдауна магии
        public bool IsMagicOnCooldown =>
            _combatController.IsMagicOnCooldown;

        public float MagicCooldownProgress =>
            _combatController.MagicCooldownProgress;

        // События для интерфейса, отображающего кулдаун
        public event System.Action MagicCooldownStarted;
        public event System.Action MagicCooldownFinished;

        // Unity-компоненты объекта игрока
        private CharacterController _characterController;
        private Animator _animator;

        // Основная игровая сущность игрока
        // Хранит характеристики и игровое состояние персонажа
        private PlayerEntity _player;

        private void Awake()
        {
            // Получаем обязательные Unity-компоненты
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();

            // Создаем подсистему движения
            _movement = new PlayerMovement(
                _characterController,
                playerCamera,
                _animator,
                walkSpeed,
                runSpeed,
                rotationSpeed);

            // Создаем подсистему боя
            _combat = new PlayerCombat(
                playerCamera,
                projectileSpawnPoint,
                magicProjectilePrefab,
                swordHitbox);

            // Отдельный класс для чтения пользовательского ввода
            _inputReader =
                new PlayerInputReader();

            // Создаем характеристики персонажа
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);

            // Создаем доменную сущность игрока
            // Именно она хранит игровое состояние персонажа
            _player = new PlayerEntity(stats);

            // Контроллер боевой системы
            _combatController =
                new PlayerCombatController(
                    _player,
                    _combat,
                    _animator,
                    playerCamera,
                    transform,
                    magicCooldown,
                    meleeCooldown);

            // Контроллер здоровья, смерти и оглушения
            _lifeController =
                new PlayerLifeController(
                    _player,
                    _animator,
                    stunDuration);

            // Отдельный объект для сохранения и загрузки игрока
            _savePresenter =
                new PlayerSavePresenter(
                    transform,
                    _player);

            // Подписка на изменение здоровья игрока
            // Сейчас используется только для отладки
            _player.HealthChanged += (current, max) =>
            {
                Debug.Log($"Player HP changed: {current}/{max}");
            };

            // Проксируем события кулдауна наружу,
            // чтобы UI не зависел от внутренней реализации боевой системы
            _combatController.MagicCooldownStarted +=
                () => MagicCooldownStarted?.Invoke();

            _combatController.MagicCooldownFinished +=
                () => MagicCooldownFinished?.Invoke();
        }

        private void Update()
        {
            // После смерти игрок больше не может выполнять действия
            if (_lifeController.IsDead)
                return;

            // Обновление логики здоровья и оглушения
            _lifeController.Update();

            // Во время оглушения управление блокируется
            if (_lifeController.IsStunned)
                return;

            // Считываем текущее состояние ввода
            PlayerInputData input =
                _inputReader.ReadInput();

            // Обновляем систему движения
            _movement.Update(input);

            // Обновляем систему боя
            _combatController.Update(input);
        }

        // Предоставляет доступ к игровой сущности игрока
        // Используется другими системами проекта
        public PlayerEntity GetEntity()
        {
            return _player;
        }

        private void OnDestroy()
        {
            // Освобождаем подписки и ресурсы контроллера жизни
            _lifeController?.Dispose();
        }

        // Применение загруженных данных сохранения
        public void ApplySaveData(PlayerSaveData data)
        {
            _savePresenter.ApplySaveData(data);
        }

        // Инициализация аудиосистемы после создания сервисов приложения
        public void InitializeAudio(IAudioService audioService)
        {
            _audioService = audioService;

            // Передаем аудиосервис в оружие ближнего боя
            swordHitbox.InitializeAudio(audioService);
        }

        // Создает объект данных для сохранения состояния игрока
        public PlayerSaveData CreateSaveData()
        {
            return _savePresenter.CreateSaveData();
        }
    }
}