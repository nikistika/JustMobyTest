using UnityEngine;
using Zenject;

namespace _Project.Logic.Gameplay.LoseControlling
{
    public class GameTimeController : IInitializable
    {
        public bool GameActive { get; private set; }
        
        public void Initialize()
        {
            StartGame();
        }
        
        public void StartGame()
        {
            GameActive = true;
        }

        public void StopGame()
        {
            GameActive = false;
            Debug.Log("Game Over");
        }
    }
}