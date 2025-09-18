using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Logic.Gameplay.Enemy;
using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.PlayerLogic;
using _Project.Logic.Gameplay.Spawners.PointsToSpawn.Enemy;
using _Project.Logic.Meta.ObjectPool;
using _Project.Logic.Meta.Service.RandomServiceWrap;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Logic.Gameplay.Spawners
{
    public class EnemySpawner : ITickable
    {
        private readonly List<ObjectPool<EnemyAbstract>> _enemyPools;
        private readonly IRandomService _randomService;
        private readonly ContainerEnemyPoints _container;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly Player _player;
        private readonly GameTimeController _gameTimeController;

        private bool _canSpawn = true;
        private float _spawnTimer = 5f;

        public EnemySpawner(List<ObjectPool<EnemyAbstract>> enemyPools, IRandomService randomService,
            ContainerEnemyPoints container, Player player, GameTimeController gameTimeController)
        {
            _enemyPools = enemyPools;
            _randomService = randomService;
            _container = container;
            _player = player;
            _gameTimeController = gameTimeController;
        }

        public void Tick()
        {
            if (_canSpawn && _gameTimeController.GameActive)
            {
                _ = Spawn();
            }
        }


        
        private async UniTask Spawn()
        {
            _canSpawn = false;

            var numberTypeEnemy = _randomService.GetRandomNumber(0, _enemyPools.Count);
            var indexSpawnPosition = _randomService.GetRandomNumber(0, _container.SpawnPoints.Count);
            var enemy = _enemyPools[numberTypeEnemy].GetObject();
            if (enemy == null)
            {
                _canSpawn = true;
                return;
            }
            enemy.SetupBehaviourDependency(_container.SpawnPoints[indexSpawnPosition].position, _player);
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnTimer), cancellationToken: _tokenSource.Token);
            _canSpawn = true;
        }
    }
}