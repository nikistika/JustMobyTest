using System;
using System.Collections.Generic;
using _Project.Logic.Gameplay.Enemy;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.PlayerLogic
{
    public class WalletPlayer : IInitializable, IDisposable
    {
        public event Action<int> OnPointsChanged;

        private int Points { get; set; }

        private readonly List<EnemyAbstract> _enemies;

        public WalletPlayer(List<EnemyAbstract> enemies)
        {
            _enemies = enemies;
        }

        public void Initialize()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].OnDeath += AddPoints;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].OnDeath -= AddPoints;
            }
        }

        private void AddPoints(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive");

            Points += amount;
            OnPointsChanged?.Invoke(Points);
        }

        private bool CanSpend(int amount) => Points >= amount;

        public bool Spend(int amount)
        {
            if (CanSpend(amount) == false)
            {
                Debug.LogWarning($"Not enough points! Current: {Points}, Required: {amount}");
                return false;
            }

            Points -= amount;
            OnPointsChanged?.Invoke(Points);
            return true;
        }

        public int GetPoints()
        {
            return Points;
        }
    }
}