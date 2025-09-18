using System.Collections.Generic;
using UnityEngine;

namespace _Project.Logic.Gameplay.Spawners.PointsToSpawn.Enemy
{
    public class ContainerEnemyPoints : MonoBehaviour
    {
        [field: SerializeField]
        public List<Transform> SpawnPoints { get; private set; }
    }
}