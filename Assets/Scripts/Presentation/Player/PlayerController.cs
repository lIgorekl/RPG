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
    // Главный контроллер игрока.
    // Координирует движение, атаки, кулдауны и получение урона.
    public class PlayerController : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] private int maxHP = 100;
        [SerializeField] private int physicalDamage = 20;
        [SerializeField] private int magicalDamage = 15;

        [SerializeField] private Camera playerCamera;

        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private float stunDuration = 0.2f;

        [SerializeField] private ProjectileView magicProjectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private SwordHitbox swordHitbox;

        [SerializeField] private float magicCooldown = 2f;
        [SerializeField] private float meleeCooldown = 0.6f;

        // Подсистемы игрока
        private PlayerMovement _movement;
        private PlayerInputReader _inputReader;
        private PlayerCombat _combat;
        private PlayerCombatController _combatController;
        private PlayerLifeController _lifeController;
        private PlayerSavePresenter _savePresenter;

        private IAudioService _audioService;

        public bool IsMagicOnCooldown =>
            _combatController.IsMagicOnCooldown;

        public float MagicCooldownProgress =>
            _combatController.MagicCooldownProgress;

        public event System.Action MagicCooldownStarted;
        public event System.Action MagicCooldownFinished;

        // Unity компоненты
        private CharacterController _characterController;
        private Animator _animator;

        // Доменная сущность игрока (геймплейная логика)
        private PlayerEntity _player;

        private void Awake()
        {
            // Получаем Unity компоненты
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();

            // Инициализация систем игрока
            _movement = new PlayerMovement(
                _characterController,
                playerCamera,
                _animator,
                walkSpeed,
                runSpeed,
                rotationSpeed);

            _combat = new PlayerCombat(
                playerCamera,
                projectileSpawnPoint,
                magicProjectilePrefab,
                swordHitbox);

            _inputReader =
                new PlayerInputReader();

            // Создаём игровую сущность игрока
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);
            _player = new PlayerEntity(stats);

            _combatController =
                new PlayerCombatController(
                    _player,
                    _combat,
                    _animator,
                    playerCamera,
                    transform,
                    magicCooldown,
                    meleeCooldown);

            _lifeController =
                new PlayerLifeController(
                    _player,
                    _animator,
                    stunDuration);

            _savePresenter =
                new PlayerSavePresenter(
                    transform,
                    _player);

            _player.HealthChanged += (current, max) =>
            {
                Debug.Log($"Player HP changed: {current}/{max}");
            };

            _combatController.MagicCooldownStarted +=
                () => MagicCooldownStarted?.Invoke();

            _combatController.MagicCooldownFinished +=
                () => MagicCooldownFinished?.Invoke();
        }

        private void Update()
        {
            if (_lifeController.IsDead)
                return;

            _lifeController.Update();

            if (_lifeController.IsStunned)
                return;

            PlayerInputData input =
                _inputReader.ReadInput();

            // Движение игрока
            _movement.Update(input);
            _combatController.Update(input);
        }

        // Возвращает доменную сущность игрока
        public PlayerEntity GetEntity()
        {
            return _player;
        }

        private void OnDestroy()
        {
            _lifeController?.Dispose();
        }

        public void ApplySaveData(PlayerSaveData data)
        {
            _savePresenter.ApplySaveData(data);
        }

        public void InitializeAudio(IAudioService audioService)
        {
            _audioService = audioService;

            swordHitbox.InitializeAudio(audioService);
        }

        public PlayerSaveData CreateSaveData()
        {
            return _savePresenter.CreateSaveData();
        }
    }
}