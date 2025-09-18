using System.Collections.Generic;
using _Project.Logic.Gameplay.ConfigsScripts;
using _Project.Logic.Gameplay.Enemy;
using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.PlayerLogic;
using _Project.Logic.Gameplay.PlayerLogic.Shooting;
using _Project.Logic.Gameplay.Service.CameraFollower;
using _Project.Logic.Gameplay.Service.InputForGameplay;
using _Project.Logic.Gameplay.Spawners;
using _Project.Logic.Gameplay.Spawners.PointsToSpawn.Enemy;
using _Project.Logic.Gameplay.Spawners.PointsToSpawn.PlayerPoints;
using _Project.Logic.Meta.Mobile;
using _Project.Logic.Meta.ObjectPool;
using _Project.Logic.Meta.Service.DeviceIdentifier;
using _Project.Logic.Meta.Shop;
using _Project.Logic.Meta.UI.Health;
using _Project.Logic.Meta.UI.Lose;
using _Project.Logic.Meta.UI.Shop;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Infrastructure.Installers.SceneInstallers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        private const int DEFAULT_SIZE_ENEMY_POOL = 10;
        private const int DEFAULT_SIZE_BULLET_POOL = 20;

        [SerializeField] private TransformToSpawnPlayer _pointToSpawnPlayer;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private MobileTools _mobileTools;
        [SerializeField] private List<EnemyAbstract> _enemyPrefabs;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private ContainersForPools _containersForPools;
        [SerializeField] private ContainerEnemyPoints _containerEnemyPoints;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private UpgradesConfig _upgradesConfig;
        [SerializeField] private Camera _camera;
        [SerializeField] private LoseView _loseView;
        [SerializeField] private PlayerConfig _playerConfig;
        private readonly List<ObjectPool<EnemyAbstract>> _enemiesPool = new();
        
        public override void InstallBindings()
        {
            BindCheckerDevice();
            BindStartGame();
            BindingPlayer();
            BindUILose();
            BindingUIPlayerStats();
            BindingUIShop();
        }

        private void BindUILose()
        {
            Container.Bind<LoseView>().FromComponentInNewPrefab(_loseView).AsSingle();
            Container.BindInterfacesAndSelfTo<LoseViewModel>().AsSingle().NonLazy();
        }
        
        private void CreateAndBindingPools()
        {
            var instantiator = Container.Resolve<IInstantiator>();
            foreach (var enemyAbstract in _enemyPrefabs)
            {
                var poolEnemy = CreateObjectPool<EnemyAbstract>(enemyAbstract,
                    DEFAULT_SIZE_ENEMY_POOL,
                    _containersForPools.ContainerForEnemy,
                    false,
                    instantiator);
                _enemiesPool.Add(poolEnemy);
            }

            Container.Bind<List<ObjectPool<EnemyAbstract>>>().FromInstance(_enemiesPool).AsSingle().NonLazy();

            var bullet = CreateObjectPool<Bullet>(_bulletPrefab,
                DEFAULT_SIZE_BULLET_POOL,
                _containersForPools.ContainerForBullet,
                false,
                instantiator);

            Container.Bind<ObjectPool<Bullet>>().FromInstance(bullet).AsSingle().NonLazy();
        }

        private void BindCheckerDevice()
        {
            Container.Bind<MobileTools>().FromInstance(_mobileTools).AsSingle();
            Container.Bind<DeviceIdentifier>().AsSingle().NonLazy();
        }

        private void BindStartGame()
        {
            Container.BindInterfacesAndSelfTo<GameTimeController>().AsSingle().NonLazy();
            Container.Bind<SceneRestarter>().AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsCached();
            Container.BindInterfacesAndSelfTo<CameraFollowerService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShopController>().AsSingle().NonLazy();
            Container.Bind<DeathView>().AsSingle().NonLazy();
            CreateAndBindingPools();
            var enemy = CastEnemyPoolToList();
            Container.Bind<List<EnemyAbstract>>().FromInstance(enemy).AsCached();
            Container.Bind<ContainerEnemyPoints>().FromInstance(_containerEnemyPoints).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.Bind<UpgradesConfig>().FromInstance(_upgradesConfig).AsCached();
        }

        private List<EnemyAbstract> CastEnemyPoolToList()
        {
            var enemies = CastPoolObjectInTType<EnemyAbstract>(_enemiesPool);
            return enemies;
        }
        
        private void BindingPlayer()
        {
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<Player>().FromComponentInNewPrefab(_playerPrefab)
                .UnderTransform(_pointToSpawnPlayer.PointToSpawn.transform).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WalletPlayer>().AsCached();

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
            }
            else
            {
                Container.BindInterfacesAndSelfTo<KeybordInput>().AsSingle();
            }

            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
        }

        private void BindingUIPlayerStats()
        {
            Container.BindInterfacesAndSelfTo<HealthViewModel>().AsSingle().NonLazy();
            Container.Bind<HealthView>().FromComponentInNewPrefab(_healthView).AsSingle();
        }

        private void BindingUIShop()
        {
            Container.Bind<ShopView>().FromComponentInNewPrefab(_shopView).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopViewModel>().AsSingle().NonLazy();
        }

        private ObjectPool<T> CreateObjectPool<T>(T prefab, int size, Transform container, bool autoExpand,
            IInstantiator instantiator) where T : MonoBehaviour
        {
            var objPool = new ObjectPool<T>(prefab, size, container, autoExpand, instantiator);
            return objPool;
        }

        private List<T> CastPoolObjectInTType<T>(List<ObjectPool<T>> objectPool) where T : MonoBehaviour
        {
            var castList = new List<T>();
            foreach (var ObjectPool in objectPool)
            {
                foreach (var creature in ObjectPool.Objects)
                {
                    var obj = creature as T;
                    castList.Add(obj);
                }
            }

            return castList;
        }
    }
}