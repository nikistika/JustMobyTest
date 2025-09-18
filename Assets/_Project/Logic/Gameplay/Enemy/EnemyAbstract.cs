using System;
using System.Collections;
using _Project.Logic.Gameplay.ConfigsScripts;
using _Project.Logic.Gameplay.PlayerLogic;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.Enemy
{
    public abstract class EnemyAbstract : MonoBehaviour
    {
        public Action<int> OnDeath;

        [SerializeField] private EnemyConfig _config;

        protected float Speed;

        private WaitForSeconds _timeCooldownAttack;
        private DeathView _deathView;
        private int _currentHealth;
        private int _damage;
        private int _reward;
        private bool _isDead;
        private bool _isAttackCooldown;

        [Inject]
        public void Construct(DeathView deathView)
        {
            _deathView = deathView;
        }

        private void Start()
        {
            Setup();
            _timeCooldownAttack = new WaitForSeconds(_config.AttackCooldown);
        }

        private void OnEnable()
        {
            ResetVisual();
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                if (_isAttackCooldown == false)
                {
                    player.TakeDamage(_damage);
                    StartCoroutine(StartDamageTimer());
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isDead)
            {
                return;
            }

            Behaviour();
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                Debug.LogWarning("Damage is negative");
                return;
            }

            _currentHealth = Mathf.Max(_currentHealth - damage, 0);

            if (_currentHealth == 0 && !_isDead)
            {
                _isDead = true;
                OnDeath?.Invoke(_reward);
                _deathView.InvokeDeathVisual(this);
            }
        }

        private void OnDisable()
        {
            ResetVisual();
        }

        protected abstract void Behaviour();

        protected virtual void Setup()
        {
            _currentHealth = _config.MaxHealth;
            Speed = _config.Speed;
            _damage = _config.Damage;
            _reward = _config.Reward;
        }

        protected virtual void ResetVisual()
        {
            _isDead = false;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            gameObject.GetComponent<Collider>().enabled = true;
        }


        public abstract void SetupBehaviourDependency(Vector3 position, Player target);

        private IEnumerator StartDamageTimer()
        {
            _isAttackCooldown = true;
            yield return _timeCooldownAttack;
            _isAttackCooldown = false;

        }
    }
}