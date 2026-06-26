using UnityEngine;
using UnityEngine.AI;
using Gameplay.Characters;
using Gameplay.Stats;
using Core.Combat;
using App.SaveLoad;
using Presentation.AI;
using App.Services;
using Gameplay.Combat;

namespace Presentation.Scene
{
    // Базовый класс представления врага
    // Связывает игровую логику врага с компонентами Unity
    public abstract class BaseEnemyView : MonoBehaviour, IDamageable
    {
        [Header("Base Enemy Stats")]

        // Базовые характеристики врага
        [SerializeField] protected int maxHP = 50;
        [SerializeField] protected int physicalDamage = 10;
        [SerializeField] protected int magicalDamage = 0;
        [SerializeField] protected Animator animator;

        // Время оглушения после получения урона
        [SerializeField] private float stunDuration = 0.6f;

        // Уникальный идентификатор для сохранения
        [SerializeField] private string enemyId;

        public Animator Animator => animator;
        public bool IsDead => enemy.IsDead;

        // Игровая сущность врага
        protected EnemyEntity enemy;

        private NavMeshAgent agent;
        private IAudioService _audioService;
        private EnemyDamageProcessor _damageProcessor;

        // Возвращает игровую сущность врага
        public EnemyEntity GetEntity()
        {
            return enemy;
        }

        protected virtual void Awake()
        {
            // Создаем характеристики врага из параметров инспектора
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);

            // Создаем игровую сущность врага
            enemy = new EnemyEntity(stats);

            // Подписываемся на событие смерти
            enemy.Died += OnEnemyDied;

            // Получаем необходимые Unity-компоненты
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();

            // Создаем обработчик получения урона
            _damageProcessor = new EnemyDamageProcessor();
        }

        protected virtual void OnDestroy()
        {
            if (enemy != null)
                enemy.Died -= OnEnemyDied;
        }

        private void Update()
        {
            UpdateAnimation();
        }

        // Обновляет анимацию движения врага
        private void UpdateAnimation()
        {
            if (animator == null || agent == null)
                return;

            float speed = agent.velocity.magnitude;

            animator.SetFloat("Speed", speed);
        }

        // Получает урон от системы боя
        public void ReceiveDamage(Damage damage)
        {
            // Для босса первое попадание запускает бой
            var boss = GetComponent<Presentation.AI.BossBehaviour>();

            if (boss != null)
            {
                boss.Activate();
            }

            // Передаем обработку урона отдельному классу
            _damageProcessor.ProcessDamage(
                this,
                damage,
                stunDuration);

            // Если враг погиб, дальнейшая обработка не требуется
            if (enemy.IsDead)
                return;

            // Запускаем анимацию получения урона
            if (animator != null)
                animator.SetTrigger("Hurt");
        }

        // Вызывается после смерти врага
        protected virtual void OnEnemyDied()
        {
            // Запускаем анимацию смерти
            if (animator != null)
            {
                animator.ResetTrigger("Hurt");
                animator.SetTrigger("Death");
            }

            // Полностью останавливаем навигацию врага
            if (agent != null)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }

            // Отключаем AI ближнего врага
            var behaviour = GetComponent<Presentation.AI.EnemyBehaviour>();

            if (behaviour != null)
                behaviour.enabled = false;

            // Отключаем AI дальнего врага
            var ranged = GetComponent<Presentation.AI.RangedEnemyBehaviour>();

            if (ranged != null)
                ranged.enabled = false;

            // Через несколько секунд скрываем объект со сцены
            StartCoroutine(DisableAfterDelay(5f));
        }

        // Базовая атака врага
        public virtual void Attack(Transform player)
        {
            Attack(player, 1f, false);
        }

        // Атака с множителем урона
        public virtual void Attack(Transform player, float damageMultiplier)
        {
            Attack(player, damageMultiplier, false);
        }

        // Выполняет атаку по игроку
        public virtual void Attack(Transform player, float damageMultiplier, bool isHeavy)
        {
            // Мертвый враг не может атаковать
            if (enemy.IsDead)
                return;

            // Запускаем нужную анимацию атаки
            if (animator != null)
            {
                if (isHeavy)
                    animator.SetTrigger("HeavyAttack");
                else
                    animator.SetTrigger("Attack");
            }

            var baseDamage = enemy.GetPhysicalDamage();

            // Увеличиваем или уменьшаем урон через множитель
            var damage = new Core.Combat.Damage(
                Mathf.RoundToInt(baseDamage.Value * damageMultiplier),
                baseDamage.Type
            );

            var playerController =
                player.GetComponent<Presentation.Player.PlayerController>();

            // Передаем урон игроку
            if (playerController != null)
                playerController.GetEntity().ReceiveDamage(damage);
        }

        // Возвращает идентификатор врага
        public string GetId()
        {
            return enemyId;
        }

        // Устанавливает идентификатор врага
        public void SetEnemyId(string id)
        {
            enemyId = id;
        }

        // Применяет данные сохранения
        public void ApplySaveData(EnemySaveData data)
        {
            // Останавливаем все активные корутины
            StopAllCoroutines();

            // Гарантируем, что объект активен после загрузки
            gameObject.SetActive(true);

            // Восстанавливаем позицию врага
            transform.position = new Vector3(
                data.PositionX,
                data.PositionY,
                data.PositionZ
            );

            // Восстанавливаем здоровье
            enemy.SetHP(data.CurrentHp);

            // Если враг был мертв во время сохранения,
            // восстанавливаем состояние смерти
            if (data.IsDead)
            {
                OnEnemyDied();
                return;
            }

            // Иначе полностью восстанавливаем врага
            Revive();
        }

        // Восстанавливает врага после загрузки сохранения
        private void Revive()
        {
            // Включаем NavMeshAgent и возвращаем его в рабочее состояние
            if (agent != null)
            {
                agent.enabled = true;

                // Перемещаем агента в сохраненную позицию
                agent.Warp(transform.position);

                agent.isStopped = false;
            }

            var baseBehaviour = GetComponent<BaseEnemyBehaviour>();

            if (baseBehaviour != null)
            {
                // Включаем AI врага
                baseBehaviour.enabled = true;

                // Возвращаем стартовое состояние машины состояний
                baseBehaviour.StateMachine.EnterDefaultState();
            }

            if (animator != null)
            {
                // Очищаем возможные старые триггеры анимации
                animator.ResetTrigger("Death");
                animator.ResetTrigger("Hurt");

                // Возвращаем анимацию ожидания
                animator.Play("Idle");
            }

            // Восстанавливаем состояние ярости босса
            var boss = GetComponent<Presentation.AI.BossBehaviour>();

            if (boss != null)
            {
                var entity = enemy;

                float hpPercent =
                    (float)entity.CurrentHP / entity.MaxHP;

                boss.SetEnraged(hpPercent < 0.5f);
            }
        }

        // Отключает объект через заданное время после смерти
        private System.Collections.IEnumerator DisableAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            gameObject.SetActive(false);
        }

        // Инициализирует аудиосервис
        public void InitializeAudio(IAudioService audioService)
        {
            _audioService = audioService;
        }
    }
}