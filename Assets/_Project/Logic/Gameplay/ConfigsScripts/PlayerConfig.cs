using UnityEngine;

namespace _Project.Logic.Gameplay.ConfigsScripts
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Gameplay/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField]
        public int MaxHealth { get; private set; }
        [field: SerializeField]
        public float MaxSpeed { get; private set; }
        [field: SerializeField]
        public int CurrentDamage { get; private set; }

        [field: SerializeField]
        public float RotationSpeed { get; private set; }
    }
}