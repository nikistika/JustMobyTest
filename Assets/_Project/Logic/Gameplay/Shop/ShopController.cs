using System;
using UnityEngine;
using _Project.Logic.Gameplay.ConfigsScripts;
using _Project.Logic.Gameplay.PlayerLogic;
using Zenject;

namespace _Project.Logic.Meta.Shop
{
    public class ShopController : IInitializable
    {
        private readonly UpgradesConfig _config;
        private readonly Player _player;
        private readonly PlayerConfig _playerConfig;

        private int _damageLevel;
        private int _healthLevel;
        private int _speedLevel;

        public int CurrentDamageLevel => _damageLevel;
        public int CurrentHealthLevel => _healthLevel;
        public int CurrentSpeedLevel => _speedLevel;

        public int DamageUpgradeCost => _config.DamageUpgradeModel.Cost;
        public int HealthUpgradeCost => _config.HealthUpgradeModel.Cost;
        public int SpeedUpgradeCost => _config.SpeedUpgradeModel.Cost;

        private int _currentThresholdUpgradeDamageValueInPercent;
        private int _currentThresholdHealthUpgradeValueInPercent;
        private int _currentThresholdSpeedUpgradeValueInPercent;

        public ShopController(UpgradesConfig config, Player player, PlayerConfig playerConfig)
        {
            _config = config;
            _player = player;
            _playerConfig = playerConfig;
        }

        public void Initialize()
        {
            ResetSessionLevels();
        }

        public void UpgradeDamage()
        {
            UpgradeStat(
                _config.DamageUpgradeModel.ThresholdValueUpgrade,
                ref _currentThresholdUpgradeDamageValueInPercent,
                ref _damageLevel,
                _playerConfig.CurrentDamage,
                _config.DamageUpgradeModel.PercentValue,
                value => _player.IncreaseDamage((int)value)
            );
        }

        public void UpgradeHealth()
        {
            UpgradeStat(
                _config.HealthUpgradeModel.ThresholdValueUpgrade,
                ref _currentThresholdHealthUpgradeValueInPercent,
                ref _healthLevel,
                _playerConfig.MaxHealth,
                _config.HealthUpgradeModel.PercentValue,
                value => _player.IncreaseHealth((int)value)
            );
        }

        public void UpgradeSpeed()
        {
            UpgradeStat(
                _config.SpeedUpgradeModel.ThresholdValueUpgrade,
                ref _currentThresholdSpeedUpgradeValueInPercent,
                ref _speedLevel,
                _playerConfig.MaxSpeed,
                _config.SpeedUpgradeModel.PercentValue,
                value => _player.IncreaseSpeed(value)
            );
        }

        private void UpgradeStat(
            int thresholdValueUpgrade,
            ref int currentThresholdPercent,
            ref int currentLevel,
            float baseValue,
            int percentIncrease,
            Action<float> applyUpgrade)
        {
            if (CanUpgrade(thresholdValueUpgrade, currentThresholdPercent) == false)
            {
                Debug.Log("This maximum upgrade reached");
                return;
            }

            currentLevel++;
            
            var numericUpgradeValue = baseValue * (1 + percentIncrease / 100f) - baseValue;
            applyUpgrade(numericUpgradeValue);

            currentThresholdPercent += percentIncrease;
        }

        private bool CanUpgrade(int configThresholdValue, int currentThresholdValue)
        {
            return currentThresholdValue < configThresholdValue;
        }

        private void ResetSessionLevels()
        {
            _damageLevel = 0;
            _healthLevel = 0;
            _speedLevel = 0;
        }
    }
}