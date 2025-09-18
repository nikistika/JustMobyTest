using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.Meta.UI.Lose
{
    public class LoseView : MonoBehaviour
    {
        public Button RestartButton;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
           gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}