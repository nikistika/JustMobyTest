using UnityEngine.SceneManagement;

namespace _Project.Logic.Gameplay.LoseControlling
{
    public class SceneRestarter
    {
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}