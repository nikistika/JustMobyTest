using _Project.Logic.Gameplay.Enemy;
using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.Service.TimeForInteract;
using _Project.Logic.Meta.Service.TimeForInteract;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.PlayerLogic.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private int _damage;
        private ITimeService _timeService;
        private GameTimeController _gameTimeController;

        private readonly float _distanceToFade = 50f;
        private Vector3 _startPosition;

        [Inject]
        public void Construct(ITimeService timeService, GameTimeController gameTimeController)
        {
            _timeService = timeService;
            _gameTimeController = gameTimeController;
        }
        
        private void Update()
        {
            if (_gameTimeController.GameActive)
            {
                transform.Translate(Vector3.forward * (_speed * _timeService.GetDeltaTime()));
                CheakDistanceToFade(); 
            }
        }

        private void CheakDistanceToFade()
        {
            var distance = _startPosition - transform.position;
            if (Vector3.SqrMagnitude(distance) > _distanceToFade * _distanceToFade)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out EnemyAbstract enemy))
            {
                enemy.TakeDamage(_damage);
                gameObject.SetActive(false);
            }
        }

        public void Setup(int damage, Vector3 position, Quaternion rotation)
        {
            _damage = damage;
            transform.position = position;
            transform.rotation = rotation;
            _startPosition = position;
        }
    }
}