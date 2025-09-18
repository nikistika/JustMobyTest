using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.PlayerLogic;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyDefault : EnemyAbstract
    {
        private Player _target;
        private Rigidbody _rigidbody;
        private GameTimeController _gameTimeController;

        [Inject]
        public void Construct(DeathView deathView, GameTimeController gameTimeController)
        {
            base.Construct(deathView);
            _gameTimeController = gameTimeController;
        }

        protected override void Behaviour()
        {
            if (_gameTimeController.GameActive)
            {
                var direction = (_target.transform.position - transform.position).normalized;

                _rigidbody.velocity = direction * Speed;
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }

        public override void SetupBehaviourDependency(Vector3 position, Player target)
        {
            transform.position = position;
            _target = target;
        }

        protected override void ResetVisual()
        {
            base.ResetVisual();
            _target = null;
        }

        protected override void Setup()
        {
            base.Setup();
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}