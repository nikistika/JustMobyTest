using UnityEngine;

namespace _Project.Logic.Meta.Service.RandomServiceWrap
{
    public class RandomService : IRandomService

    {
        public int GetRandomNumber(int min, int max)
        {
            var number = Random.Range(min, max);
            return number;
        }
    }
}