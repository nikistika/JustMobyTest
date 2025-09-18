using System;
using _Project.Logic.Gameplay.Service.InputForGameplay;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.PlayerLogic
{
    public class PlayerController : ITickable, IInitializable, IDisposable
    {
        private readonly Player _player;
        private readonly IInput _input;
        private readonly Camera _camera;

        private Vector3 _directionToMove;
        private Vector3 _rotation;

        public PlayerController(Player player, IInput input, Camera camera)
        {
            _player = player;
            _input = input;
            _camera = camera;
        }

        public void Initialize()
        {
            _input.OnShoot = _player.Shoot;
        }

        public void Tick()
        {
            HandleWalkInput();
            HandleRotateInput();
            _player.Move(_directionToMove);
            _player.Rotate(_rotation);
        }

        public void Dispose()
        {
            _input.OnShoot -= _player.Shoot;
        }

        private void HandleWalkInput()
        {
            var horizontalAxis = _input.GetAxisHorizontal();
            var verticalAxis = _input.GetAxisVertical();

            _directionToMove = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        }

        private void HandleRotateInput()
        {
            Ray ray = _camera.ScreenPointToRay(_input.GetRotationAxis());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _rotation = hit.point - _player.transform.position;
                _rotation.y = 0;
                _rotation = _rotation.normalized;
                return;
            }

            _rotation = Vector3.zero;
        }
    }
}