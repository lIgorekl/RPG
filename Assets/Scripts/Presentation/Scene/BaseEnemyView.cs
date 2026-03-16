using UnityEngine;
using Gameplay.Characters;
using Gameplay.Stats;
using Core.Combat;
using UnityEngine.AI;

namespace Presentation.Scene
{
    public abstract class BaseEnemyView : MonoBehaviour, IDamageable
    {
        [Header("Base Enemy Stats")]
        [SerializeField] protected int maxHP = 50;
        [SerializeField] protected int physicalDamage = 10;
        [SerializeField] protected int magicalDamage = 0;
        [SerializeField] protected Animator animator;
        public Animator Animator => animator;

        private NavMeshAgent agent;
        private bool _isStunned;
        [SerializeField] private float stunDuration = 0.6f;
        private float _stunTimer;

        public bool IsStunned => _isStunned;

        protected EnemyEntity enemy;

        public EnemyEntity GetEntity()
        {
            return enemy;
        }

        private void Update()
        {
            if (animator != null && agent != null)
            {
                float speed = agent.velocity.magnitude;
                animator.SetFloat("Speed", speed);
            }

            if (_isStunned)
            {
                _stunTimer -= Time.deltaTime;

                if (_stunTimer <= 0f)
                {
                    _isStunned = false;

                    if (agent != null)
                    {
                        agent.isStopped = false;
                    }
                }
            }
        }

        protected virtual void Awake()
        {
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

        public void ReceiveDamage(Damage damage)
        {
            if (enemy.IsDead)
                return;

            enemy.ReceiveDamage(damage);

            // если враг умер — НЕ запускать Hurt
            if (enemy.IsDead)
                return;

            _isStunned = true;
            _stunTimer = stunDuration;

            if (agent != null)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }

            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        }

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

            var behaviour = GetComponent<Presentation.AI.EnemyBehaviour>();
            if (behaviour != null)
            {
                behaviour.enabled = false;
            }

            var ranged = GetComponent<Presentation.AI.RangedEnemyBehaviour>();
            if (ranged != null)
            {
                ranged.enabled = false;
            }

            Destroy(gameObject, 5f);
        }

        public virtual void Attack(Transform player)
        {
            if (enemy.IsDead)
                return;

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            var entity = GetEntity();
            var damage = entity.GetPhysicalDamage();

            var playerController =
                player.GetComponent<Presentation.Player.PlayerController>();

            if (playerController != null)
            {
                playerController.GetEntity().ReceiveDamage(damage);
            }
        }

        public bool IsDead => enemy.IsDead;
    }
}