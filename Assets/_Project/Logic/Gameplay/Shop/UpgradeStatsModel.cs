using System;
using UnityEngine;

namespace _Project.Logic.Gameplay.Shop
{
    [Serializable]
    public class UpgradeStatsModel
    {
        [field: Range(0, 100),Header("Upgrade Value in percentage")]
        [field: SerializeField]
        public int PercentValue { get; private set; }

        [field: SerializeField]
        public int Cost { get; private set; }
        [field: SerializeField]
        public int ThresholdValueUpgrade { get; private set; }
        [field: SerializeField]
        public int Level { get; private set; }
    }
}