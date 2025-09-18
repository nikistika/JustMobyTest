using System;
using _Project.Logic.Gameplay.PlayerLogic;
using _Project.Logic.Meta.Shop;
using R3;
using Zenject;

namespace _Project.Logic.Meta.UI.Health
{
    public class HealthViewModel : IInitializable, IDisposable
    {
        private ReactiveProperty<float> _health = new ReactiveProperty<float>();
        private ReactiveProperty<float> _currentHealthText = new ReactiveProperty<float>();
        private ReactiveProperty<float> _maxHealthText = new ReactiveProperty<float>();

        private readonly Player _player;
        private readonly HealthView _healthView;

        public HealthViewModel(Player player, HealthView healthView)
        {
            _player = player;
            _healthView = healthView;
        }

        public void Initialize()
        {
            _player.OnHealthChanged += UpdateCurrentHealth;
            UpdateCurrentHealth(_player.CurrentMaxHealth, _player.CurrentMaxHealth);
            BindReactiveProperty();
        }


        public void Dispose()
        {
            _player.OnHealthChanged -= UpdateCurrentHealth;
            DisposeReactiveProperty();
        }

        private void DisposeReactiveProperty()
        {
            _health.Dispose();
            _currentHealthText.Dispose();
            _maxHealthText.Dispose();
        }

        private void BindReactiveProperty()
        {
            _health.Subscribe(x => _healthView.SetHealth(_health.Value));
            _currentHealthText.Subscribe(x => _healthView.SetCurrentHealthText(_currentHealthText.Value));
            _maxHealthText.Subscribe(x => _healthView.SetMaxHealthText(_maxHealthText.Value));
        }

        private void UpdateCurrentHealth(int currentValue, int maxValue)
        {
            _health.Value = (float)currentValue / maxValue;
            _currentHealthText.Value = currentValue;
            UpdateMaxHealth();
        }

        private void UpdateMaxHealth()
        {
            _maxHealthText.Value = _player.CurrentMaxHealth;
        }
    }
}