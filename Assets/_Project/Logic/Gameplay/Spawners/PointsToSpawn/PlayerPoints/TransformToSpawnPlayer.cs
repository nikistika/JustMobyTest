using UnityEngine;

namespace _Project.Logic.Gameplay.Spawners.PointsToSpawn.PlayerPoints
{
    public class TransformToSpawnPlayer : MonoBehaviour
    {
        [field: SerializeField]
        public Transform PointToSpawn {get; private set; }
    }
}
