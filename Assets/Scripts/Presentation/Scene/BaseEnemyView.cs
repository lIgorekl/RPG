using UnityEngine;
using UnityEngine.AI;
using Gameplay.Characters;
using Gameplay.Stats;
using Core.Combat;
using App.SaveLoad;

namespace Presentation.Scene
{
    // View-слой врага.
    // Отвечает за связь доменной сущности EnemyEntity с Unity:
    // анимации, NavMeshAgent, получение урона и смерть.
    public abstract class BaseEnemyView : MonoBehaviour, IDamageable
    {
        [Header("Base Enemy Stats")]
        [SerializeField] protected int maxHP = 50;
        [SerializeField] protected int physicalDamage = 10;
        [SerializeField] protected int magicalDamage = 0;
        [SerializeField] protected Animator animator;

        [SerializeField] private float stunDuration = 0.6f;

        [SerializeField] private string enemyId;

        public Animator Animator => animator;
        public bool IsStunned => _isStunned;
        public bool IsDead => enemy.IsDead;

        protected EnemyEntity enemy;

        private NavMeshAgent agent;
        private bool _isStunned;
        private float _stunTimer;

        public EnemyEntity GetEntity()
        {
            return enemy;
        }

        protected virtual void Awake()
        {
            // Создаём доменную сущность врага
            var stats = new CharacterStats(maxHP, physicalDamage, magicalDamage);
            enemy = new EnemyEntity(stats);

            enemy.Died += OnEnemyDied;

            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void OnDestroy()
        {
            if (enemy != null)
                enemy.Died -= OnEnemyDied;
        }

        private void Update()
        {
            UpdateAnimation();
            UpdateStun();
        }

        private void UpdateAnimation()
        {
            if (animator == null || agent == null)
                return;

            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }

        private void UpdateStun()
        {
            if (!_isStunned)
                return;

            _stunTimer -= Time.deltaTime;

            if (_stunTimer <= 0f)
            {
                _isStunned = false;

                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.isStopped = false;
                }
            }
        }

        // Получение урона из системы боя
        public void ReceiveDamage(Damage damage)
        {
            if (enemy.IsDead)
                return;

            enemy.ReceiveDamage(damage);

            // если враг умер — не запускаем стан
            if (enemy.IsDead)
                return;

            ApplyStun();

            if (animator != null)
                animator.SetTrigger("Hurt");
        }

        private void ApplyStun()
        {
            _isStunned = true;
            _stunTimer = stunDuration;

            if (agent != null)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        // Вызывается когда HP врага = 0
        protected virtual void OnEnemyDied()
        {
            if (animator != null)
            {
                animator.ResetTrigger("Hurt");
                animator.SetTrigger("Death");
            }

            if (agent != null)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }

            // отключаем AI
            var behaviour = GetComponent<Presentation.AI.EnemyBehaviour>();
            if (behaviour != null)
                behaviour.enabled = false;

            var ranged = GetComponent<Presentation.AI.RangedEnemyBehaviour>();
            if (ranged != null)
                ranged.enabled = false;

            StartCoroutine(DisableAfterDelay(5f));
        }

        // Базовая атака врага (используется ближними врагами)
        public virtual void Attack(Transform player)
        {
            if (enemy.IsDead)
                return;

            if (animator != null)
                animator.SetTrigger("Attack");

            var damage = enemy.GetPhysicalDamage();

            var playerController =
                player.GetComponent<Presentation.Player.PlayerController>();

            if (playerController != null)
                playerController.GetEntity().ReceiveDamage(damage);
        }

        public string GetId()
        {
            return enemyId;
        }

        public void ApplySaveData(EnemySaveData data)
        {
            if (data.IsDead)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            Revive();

            transform.position = new Vector3(
                data.PositionX,
                data.PositionY,
                data.PositionZ
            );

            var entity = GetEntity();
            entity.SetHP(data.CurrentHp);
        }

        private void Revive()
        {
            // NavMeshAgent
            if (agent != null)
            {
                agent.enabled = true;
                agent.isStopped = false;
            }

            // Ближний ИИ
            var behaviour = GetComponent<Presentation.AI.EnemyBehaviour>();
            if (behaviour != null)
                behaviour.enabled = true;

            // Дальний ИИ
            var ranged = GetComponent<Presentation.AI.RangedEnemyBehaviour>();
            if (ranged != null)
                ranged.enabled = true;

            // Анимации
            if (animator != null)
            {
                animator.ResetTrigger("Death");
                animator.Play("Idle");
            }
        }

        private System.Collections.IEnumerator DisableAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            gameObject.SetActive(false);
        }
    }
}