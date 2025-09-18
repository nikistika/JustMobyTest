using System;
using UnityEngine;

namespace _Project.Logic.Gameplay.Service.InputForGameplay
{
    //This class create for example 
    public class MobileInput : IInput
    {
        //Some code
        public Action OnShoot { get; set; }
        public float GetAxisHorizontal()
        {
            return 0; 
        }

        public float GetAxisVertical()
        {
            return 0;
        }

        public Vector3 GetRotationAxis()
        {
            return Vector3.zero;
        }
    }
}
