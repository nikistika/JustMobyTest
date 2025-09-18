using UnityEngine;

namespace _Project.Logic.Gameplay.ConfigsScripts
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Gameplay/EnemyConfig")]
    public class EnemyConfig: ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field:SerializeField] public int Reward { get; private set; }
    }
}
