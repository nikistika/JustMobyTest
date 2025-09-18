using System;
using _Project.Logic.Gameplay.ConfigsScripts;
using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.PlayerLogic.Shooting;
using _Project.Logic.Meta.ObjectPool;
using _Project.Logic.Meta.Service.TimeForInteract;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public event Action<int, int> OnHealthChanged;
        public event Action OnDead;

        [SerializeField] private ShootPoint _shootPoint;

        private PlayerConfig _playerConfig;
        private ObjectPool<Bullet> _bulletPool;
        private ITimeService _timeService;
        private GameTimeController _gameTimeController;
        private int _currentHealth;
        private float _rotationSpeed;
        private Rigidbody _rigidbody;

        public float CurrentSpeed { get; private set; }
        public int CurrentDamage { get; private set; }
        public int CurrentMaxHealth { get; private set; }

        [Inject]
        public void Construct(ITimeService timeService, ObjectPool<Bullet> bulletPool, PlayerConfig playerConfig,
            GameTimeController gameTimeController)
        {
            _timeService = timeService;
            _bulletPool = bulletPool;
            _playerConfig = playerConfig;
            _gameTimeController = gameTimeController;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetupPlayerStats();
        }


        public void Move(Vector3 movement)
        {
            if (movement == Vector3.zero)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            if (_gameTimeController.GameActive)
            {
                Vector3 force = movement.normalized * (CurrentSpeed * _timeService.GetDeltaTime());
                _rigidbody.AddForce(force, ForceMode.VelocityChange);
            }

            if (_rigidbody.velocity.sqrMagnitude > CurrentSpeed * CurrentSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * CurrentSpeed;
            }
        }

        public void Rotate(Vector3 rotation)
        {
            if (rotation == Vector3.zero && _gameTimeController.GameActive)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(rotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                _rotationSpeed * _timeService.GetDeltaTime());
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                Debug.LogWarning("Damage is negative");
                return;
            }

            if (_gameTimeController.GameActive)
            {
                _currentHealth = Mathf.Max(_currentHealth - damage, 0);
                OnHealthChanged?.Invoke(_currentHealth, CurrentMaxHealth);
            }


            if (_currentHealth == 0)
            {
                OnDead?.Invoke();
            }
        }

        private void SetupPlayerStats()
        {
            CurrentSpeed = _playerConfig.MaxSpeed;
            CurrentMaxHealth = _playerConfig.MaxHealth;
            CurrentDamage = _playerConfig.CurrentDamage;
            _currentHealth = CurrentMaxHealth;
            _rotationSpeed = _playerConfig.RotationSpeed;
        }

        public void IncreaseHealth(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Health upgrade is negative");
                return;
            }

            _currentHealth += amount;
            CurrentMaxHealth += amount;
            OnHealthChanged?.Invoke(_currentHealth, CurrentMaxHealth);
        }

        public void IncreaseDamage(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Damage upgrade is negative");
                return;
            }

            CurrentDamage += amount;
        }

        public void IncreaseSpeed(float amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Speed upgrade is negative");
                return;
            }

            CurrentSpeed += amount;
        }

        public void Shoot()
        {
            if (_gameTimeController.GameActive)
            {
                var bullet = _bulletPool.GetObject();
                if (bullet == null)
                {
                    Debug.LogWarning("Bullet Reload");
                    return;
                }

                bullet.Setup(CurrentDamage, _shootPoint.transform.position, _shootPoint.transform.rotation);
            }
        }
    }
}