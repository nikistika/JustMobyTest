using System;
using _Project.Logic.Gameplay.Service.InputForGameplay;
using UnityEngine;
using Zenject;

public class KeybordInput : IInput, ITickable
{
    public Action OnShoot { get; set; }

    private readonly int _buttonMouseForShoot = 0;

    public void Tick()
    {
        if (Input.GetMouseButtonDown(_buttonMouseForShoot))
        {
            OnShoot?.Invoke();
        }
    }

    public float GetAxisHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetAxisVertical()
    {
        return Input.GetAxis("Vertical");
    }

    public Vector3 GetRotationAxis()
    {
        return Input.mousePosition;
    }
}