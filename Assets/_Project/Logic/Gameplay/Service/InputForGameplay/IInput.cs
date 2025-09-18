using System;
using UnityEngine;

namespace _Project.Logic.Gameplay.Service.InputForGameplay
{
    public interface IInput
    {
        Action OnShoot { get; set; }
        float GetAxisHorizontal();
        float GetAxisVertical();
        Vector3 GetRotationAxis();
    }
}