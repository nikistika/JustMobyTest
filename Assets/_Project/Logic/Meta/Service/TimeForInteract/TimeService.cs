using _Project.Logic.Meta.Service.TimeForInteract;
using UnityEngine;

namespace _Project.Logic.Gameplay.Service.TimeForInteract
{
    public class TimeService : ITimeService
    {
        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }


        public float GetFixedDeltaTime()
        {
            return Time.fixedDeltaTime;
        }
    }
}