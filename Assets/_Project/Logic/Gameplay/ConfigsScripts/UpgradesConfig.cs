using _Project.Logic.Gameplay.Shop;
using UnityEngine;

namespace _Project.Logic.Gameplay.ConfigsScripts
{
    [CreateAssetMenu(fileName = "UpgradesConfig", menuName = "Meta/UpgradesConfig", order = 1)]
    public class UpgradesConfig : ScriptableObject
    {
        [field:SerializeField] public UpgradeStatsModel DamageUpgradeModel { get; private set; }
        [field:SerializeField] public UpgradeStatsModel HealthUpgradeModel { get; private set; }
        [field:SerializeField] public UpgradeStatsModel SpeedUpgradeModel { get; private set; }
        
    }
}