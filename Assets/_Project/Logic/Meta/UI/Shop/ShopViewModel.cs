using System;
using _Project.Logic.Gameplay.LoseControlling;
using _Project.Logic.Gameplay.PlayerLogic;
using _Project.Logic.Meta.Shop;
using UniRx;
using Zenject;

namespace _Project.Logic.Meta.UI.Shop
{
    public class ShopViewModel : IInitializable, IDisposable
    {
        private readonly WalletPlayer _wallet;
        private readonly ShopController _shopController;
        private readonly ShopView _view;
        private readonly GameTimeController _gameTimeController;

        private readonly ReactiveProperty<int> _points = new();
        private readonly ReactiveProperty<int> _healthCost = new();
        private readonly ReactiveProperty<int> _speedCost = new();
        private readonly ReactiveProperty<int> _damageCost = new();
        private readonly ReactiveProperty<int> _healthLevel = new();
        private readonly ReactiveProperty<int> _speedLevel = new();
        private readonly ReactiveProperty<int> _damageLevel = new();
        private readonly ReactiveProperty<bool> _canBuyUpgradeSpeed = new();
        private readonly ReactiveProperty<bool> _canBuyUpgradeDamage = new();
        private readonly ReactiveProperty<bool> _canBuyUpgradeHealth = new();
        
        private readonly ReactiveProperty<bool> _openWindow = new(false);

        private readonly ReactiveCommand _upgradeHealthCommand = new();
        private readonly ReactiveCommand _upgradeSpeedCommand = new();
        private readonly ReactiveCommand _upgradeDamageCommand = new();
        private readonly ReactiveCommand _exitCommand = new();
        private readonly ReactiveCommand _showShopCommand = new();
        private readonly ReactiveCommand _applyUpgradesCommand = new();

        private int _pendingLevelHealth = 0;
        private int _pendingLevelSpeed = 0;
        private int _pendingLevelDamage = 0;
        private int _pendingSpendPoints = 0;

        public ShopViewModel(GameTimeController gameTimeController, WalletPlayer wallet, ShopController shopManager, ShopView view)
        {
            _wallet = wallet;
            _shopController = shopManager;
            _view = view;
            _gameTimeController = gameTimeController;
        }

        public void Initialize()
        {
            _wallet.OnPointsChanged += UpdatePoints;
            SetupCommands();
            SetupBindings();
            UpdateAllValues();
            UpdateInteractButtons();
        }

        public void Dispose()
        {
            _wallet.OnPointsChanged -= UpdatePoints;

            _upgradeHealthCommand.Dispose();
            _upgradeSpeedCommand.Dispose();
            _upgradeDamageCommand.Dispose();
            _exitCommand.Dispose();
            _showShopCommand.Dispose();
            _applyUpgradesCommand.Dispose();

            _points.Dispose();
            _healthCost.Dispose();
            _speedCost.Dispose();
            _damageCost.Dispose();
            _healthLevel.Dispose();
            _speedLevel.Dispose();
            _damageLevel.Dispose();
        }

        private void UpdatePoints(int points)
        {
            _points.Value = points;
            UpdateInteractButtons();
        }

        private void SetupCommands()
        {
            _upgradeHealthCommand.Subscribe(_ => PreviewUpgradeHealth());
            _upgradeSpeedCommand.Subscribe(_ => PreviewUpgradeSpeed());
            _upgradeDamageCommand.Subscribe(_ => PreviewUpgradeDamage());
            _exitCommand.Subscribe(_ => ExitShop());
            _showShopCommand.Subscribe(_ => ShowShop());
            _applyUpgradesCommand.Subscribe(_ => ApplyUpgrades());
        }

        private void SetupBindings()
        {
            _view.UpgradeHealthButton.OnClickAsObservable()
                .Subscribe(_ => _upgradeHealthCommand.Execute());

            _view.UpgradeSpeedButton.OnClickAsObservable()
                .Subscribe(_ => _upgradeSpeedCommand.Execute());

            _view.UpgradeDamageButton.OnClickAsObservable()
                .Subscribe(_ => _upgradeDamageCommand.Execute());

            _view.ExitButton.OnClickAsObservable()
                .Subscribe(_ => _exitCommand.Execute());

            _view.ShowShopButton.OnClickAsObservable()
                .Subscribe(_ => _showShopCommand.Execute());

            _view.ApplyUpgradeButton.OnClickAsObservable()
                .Subscribe(_ => _applyUpgradesCommand.Execute());

            _points.Subscribe(x => _view.SetPoints(x));
            _healthLevel.Subscribe(x => _view.SetLevelHealth(x));
            _speedLevel.Subscribe(x => _view.SetLevelSpeed(x));
            _damageLevel.Subscribe(x => _view.SetLevelDamage(x));
            _healthCost.Subscribe(x => _view.SetCostHealth(x));
            _speedCost.Subscribe(x => _view.SetCostSpeed(x));
            _damageCost.Subscribe(x => _view.SetCostDamage(x));
            _canBuyUpgradeDamage.Subscribe(x => _view.UpgradeDamageButton.interactable = x);
            _canBuyUpgradeSpeed.Subscribe(x => _view.UpgradeSpeedButton.interactable = x);
            _canBuyUpgradeHealth.Subscribe(x => _view.UpgradeHealthButton.interactable = x);
        }

        private void ExitShop()
        {
            _view.HideShop();
            ResetPendingValue();
            UpdateAllValues();
            UpdateInteractButtons();
            _gameTimeController.StartGame();
        }

        private void ShowShop()
        {
            _view.ShowShop();
            _gameTimeController.StopGame();
        }

        private void ApplyUpgrades()
        {
            _wallet.Spend(_pendingSpendPoints);

            for (int i = 0; i < _pendingLevelDamage; i++)
            {
                _shopController.UpgradeDamage();
            }

            for (int i = 0; i < _pendingLevelSpeed; i++)
            {
                _shopController.UpgradeSpeed();
            }

            for (int i = 0; i < _pendingLevelHealth; i++)
            {
                _shopController.UpgradeHealth();
            }

            ExitShop();
        }

        private void ResetPendingValue()
        {
            _pendingLevelHealth = 0;
            _pendingLevelSpeed = 0;
            _pendingLevelDamage = 0;
            _pendingSpendPoints = 0;
        }

        private void UpdateAllValues()
        {
            _points.Value = _wallet.GetPoints();
            _healthLevel.Value = _shopController.CurrentHealthLevel;
            _speedLevel.Value = _shopController.CurrentSpeedLevel;
            _damageLevel.Value = _shopController.CurrentDamageLevel;
            _healthCost.Value = _shopController.HealthUpgradeCost;
            _speedCost.Value = _shopController.SpeedUpgradeCost;
            _damageCost.Value = _shopController.DamageUpgradeCost;
        }

        private void PreviewUpgradeHealth()
        {
            PreviewUpgrade(_shopController.HealthUpgradeCost,
                ref _pendingLevelHealth,
                _shopController.CurrentHealthLevel,
                _healthLevel);
        }

        private void PreviewUpgradeSpeed()
        {
            PreviewUpgrade(_shopController.SpeedUpgradeCost,
                ref _pendingLevelSpeed,
                _shopController.CurrentSpeedLevel,
                _speedLevel);
        }

        private void PreviewUpgradeDamage()
        {
            PreviewUpgrade(_shopController.DamageUpgradeCost,
                ref _pendingLevelDamage,
                _shopController.CurrentDamageLevel,
                _damageLevel);
        }

        private bool HasBuy(int cost)
        {
            return (_wallet.GetPoints() - _pendingSpendPoints) >= cost;
        }

        private void PreviewUpgrade(int cost, ref int pendingLevel, int currentLevel,
            ReactiveProperty<int> levelProperty)
        {
            if (HasBuy(cost) == false)
            {
                return;
            }

            pendingLevel++;
            levelProperty.Value = currentLevel + pendingLevel;
            _pendingSpendPoints += cost;
            _points.Value = _wallet.GetPoints() - _pendingSpendPoints;
            UpdateInteractButtons();
        }

        private void UpdateInteractButtons()
        {
            _canBuyUpgradeDamage.Value = HasBuy(_shopController.DamageUpgradeCost);
            _canBuyUpgradeHealth.Value = HasBuy(_shopController.HealthUpgradeCost);
            _canBuyUpgradeSpeed.Value = HasBuy(_shopController.SpeedUpgradeCost);
        }
    }
}