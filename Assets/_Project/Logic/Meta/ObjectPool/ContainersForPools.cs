using UnityEngine;

namespace _Project.Logic.Meta.ObjectPool
{
    public class ContainersForPools : MonoBehaviour
    {
        [field: SerializeField]
        public Transform ContainerForBullet { get; private set; }

        [field: SerializeField]
        public Transform ContainerForEnemy { get; private set; }
    }
}