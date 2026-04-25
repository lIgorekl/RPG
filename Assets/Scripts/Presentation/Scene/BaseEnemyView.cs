using UnityEngine;
using UnityEngine.AI;
using Gameplay.Characters;
using Gameplay.Stats;
using Core.Combat;
using App.SaveLoad;
using Presentation.AI;

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
        public bool IsDead => enemy.IsDead;

        protected EnemyEntity enemy;

        private NavMeshAgent agent;

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
        }

        private void UpdateAnimation()
        {
            if (animator == null || agent == null)
                return;

            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
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

            var behaviour = GetComponent<BaseEnemyBehaviour>();

            if (behaviour != null)
            {
                behaviour.StateMachine.ChangeState(
                    new StunState(behaviour, stunDuration));
            }

            if (animator != null)
                animator.SetTrigger("Hurt");
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
            StopAllCoroutines();
            gameObject.SetActive(true);

            // 1. ПОЗИЦИЯ
            transform.position = new Vector3(
                data.PositionX,
                data.PositionY,
                data.PositionZ
            );

            // 2. HP
            enemy.SetHP(data.CurrentHp);

            // 3. СМЕРТЬ
            if (data.IsDead)
            {
                OnEnemyDied();
                return;
            }

            // 4. ВОССТАНОВЛЕНИЕ
            Revive();
        }

        private void Revive()
        {
            if (agent != null)
            {
                agent.enabled = true;
                agent.Warp(transform.position);
                agent.isStopped = false;
            }

            var baseBehaviour = GetComponent<BaseEnemyBehaviour>();

            if (baseBehaviour != null)
            {
                baseBehaviour.enabled = true;

                if (baseBehaviour is EnemyBehaviour melee)
                {
                    melee.StateMachine.ChangeState(new IdleState(melee));
                }
                else if (baseBehaviour is RangedEnemyBehaviour ranged)
                {
                    ranged.StateMachine.ChangeState(new RangedIdleState(ranged));
                }
            }

            if (animator != null)
            {
                animator.ResetTrigger("Death");
                animator.ResetTrigger("Hurt");
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