using _Project.Logic.Gameplay.PlayerLogic;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.Service.CameraFollower
{
    public class CameraFollowerService : ILateTickable
    {
        private Vector3 _offset = new Vector3(0f, 11f, -10f);
        private Camera _mainCamera;
        private Player _player;

        public CameraFollowerService(Camera mainCamera, Player player)
        {
            _mainCamera = mainCamera;
            _player = player;
        }

        public void LateTick()
        {
            Follow();
        }

        private void Follow()
        {
            _mainCamera.transform.position = _player.transform.position + _offset;
        }
    }
}